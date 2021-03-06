﻿// <auto-generated />
using System;
using EP.Query.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EP.Query.Migrations
{
    [DbContext(typeof(QueryDbContext))]
    partial class QueryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EP.Query.DataSource.DataSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnName("creator_user_id")
                        .HasColumnType("bigint(20)");

                    b.Property<int>("DataSourceFolderId")
                        .HasColumnName("datasource_folder_id")
                        .HasColumnType("int(11)");

                    b.Property<DateTime?>("LastModificationTime");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnName("last_modifier_user_id")
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Remark")
                        .HasColumnName("remark")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("SourceContent")
                        .HasColumnName("source_content")
                        .HasColumnType("varchar(500)");

                    b.Property<int?>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("int(11)");

                    b.Property<sbyte>("Type")
                        .HasColumnName("type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceFolderId");

                    b.ToTable("datasouces");
                });

            modelBuilder.Entity("EP.Query.DataSource.DataSourceField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int(11)");

                    b.Property<int>("DataSourceId")
                        .HasColumnName("datasource_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("DisplayText")
                        .HasColumnName("display_text")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Filter")
                        .HasColumnName("filter")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("int(11)");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceId");

                    b.ToTable("datasource_fields");
                });

            modelBuilder.Entity("EP.Query.DataSource.DataSourceFolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnName("creator_user_id")
                        .HasColumnType("bigint(20)");

                    b.Property<int>("DataSourceCount")
                        .HasColumnName("datasource")
                        .HasColumnType("int(11)");

                    b.Property<int?>("DataSourceFolderId");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnName("last_modification_time")
                        .HasColumnType("datetime");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnName("last_modifier_user_id")
                        .HasColumnType("bigint(20)");

                    b.Property<int>("Level")
                        .HasColumnName("level")
                        .HasColumnType("int(11)");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(40)");

                    b.Property<int?>("ParentId")
                        .HasColumnName("parent_id")
                        .HasColumnType("int(11)");

                    b.Property<int?>("TenantId")
                        .HasColumnName("tenant_id")
                        .HasColumnType("int(11)");

                    b.HasKey("Id");

                    b.HasIndex("DataSourceFolderId");

                    b.ToTable("datasource_folders");
                });

            modelBuilder.Entity("EP.Query.DataSource.DataSource", b =>
                {
                    b.HasOne("EP.Query.DataSource.DataSourceFolder", "DataSourceFolder")
                        .WithMany("DataSources")
                        .HasForeignKey("DataSourceFolderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EP.Query.DataSource.DataSourceField", b =>
                {
                    b.HasOne("EP.Query.DataSource.DataSource", "DataSource")
                        .WithMany("DataSourceFields")
                        .HasForeignKey("DataSourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EP.Query.DataSource.DataSourceFolder", b =>
                {
                    b.HasOne("EP.Query.DataSource.DataSourceFolder")
                        .WithMany("SubDataSourceFolders")
                        .HasForeignKey("DataSourceFolderId");
                });
#pragma warning restore 612, 618
        }
    }
}
