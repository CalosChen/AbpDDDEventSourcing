using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Castle.Facilities.Logging;
using EP.Commons.AspNetCore;
using EP.Commons.CAP;
using EP.Commons.Core;
using EP.Commons.LoadBalance;
using EP.Query.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EP.Commons.Exceptionless;
using Exceptionless;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System.IO;
using EP.Commons.Core.Configuration;
using Abp.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore.SignalR.Hubs;
using EP.Query.DataSource.Options;

namespace EP.Query.WebApi.Startup
{
    public class Startup
    {
        private string _defaultCorsPolicyName = "localhost";

        private const string QueryDBFilterSectionName = "SchemaFilters";

        private readonly IConfigurationRoot _appConfiguration;

        private readonly IConfigurationBuilder _appConfigurationBuilder;


        //private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.GetConfiguration(env.ContentRootPath, env.EnvironmentName);
            _appConfigurationBuilder = AppConfigurations.GetConfigurationBuilder(env.ContentRootPath, env.EnvironmentName);

            _defaultCorsPolicyName = _appConfiguration["App:CorsOrigins"].ToString();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<SchemaFiltersSection>(_appConfiguration.GetSection(QueryDBFilterSectionName));

            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            );

            services.AddAspDotCoreAllServices<QueryDbContext>(_appConfiguration, _appConfigurationBuilder);//,
                //new Assembly[] { Assembly.GetAssembly(typeof(QueryApplicationModule)), Assembly.GetAssembly(typeof(QueryWebApiModule)) });

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Query API", Version = "v1" });
                //Locate the XML file being generated by ASP.NET...
                var xmlFiles = new List<string>() { $"{typeof(QueryApplicationModule).Assembly.GetName().Name}.xml", $"{typeof(QueryWebApiModule).Assembly.GetName().Name}.xml" };
                xmlFiles.ForEach(xmlFile =>
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        //... and tell Swagger to use those XML comments.
                        options.IncludeXmlComments(xmlPath);
                    }
                });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<QueryWebApiModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Castle.Core.Logging.ILoggerFactory loggerFactory)
        {
            app.UseAbp(o => o.UseAbpRequestLocalization = false); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "Query API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("EP.Query.WebApi.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAspDotCoreFeatures<QueryDbContext>(_appConfiguration);
        }
    }
}