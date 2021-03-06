﻿
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Query.DataSource
{
    public interface IDataSourceAppService
    {

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateFolderOutput> CreateFolder(CreateFolderInput input);

        /// <summary>
        /// 刪除空文件夾
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteFolder(int id);

        /// <summary>
        /// 分页获取指定文件夹下数据源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetALlOutput> GetALl(GetALlInput input);

        /// <summary>
        /// 获取数据源和字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataSourceDto> Get(int id);

        /// <summary>
        /// 重命名数据源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RenameOutput> Rename(RenameInput input);

        /// <summary>
        /// 添加或修改数据源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SaveOutput> Save(SaveInput input);

        /// <summary>
        /// 删除数据源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(DeleteInput input);

        /// <summary>
        /// 获取数据库表架构和字段
        /// </summary>
        /// <returns></returns>
        Task<List<DataSourceSchemaDto>> GetSchemas(DataSourceType dataSourceType);
        /// <summary>
        /// 根据查询生成字段定义
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetQueryColumns(GetQueryColumnsInput input);
        /// <summary>
        /// 获取数据源预览数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetQueryDataOutput> GetPreviewData(GetQueryDataInput input);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="queryAll"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<GetQueryDataOutput> GetQueryData(int id, bool queryAll = false, int skipCount = 0, int maxResultCount = int.MaxValue);


    }




}