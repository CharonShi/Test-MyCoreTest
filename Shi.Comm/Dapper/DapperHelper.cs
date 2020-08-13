using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Microsoft.Extensions.Configuration;

namespace CommonHelper
{
    /// <summary>
    /// dapper 帮助类
    /// </summary>
    public class DapperHelper : IDapperHelper, IDisposable
    {
        private string ConnectionString = string.Empty;
        private Database Connection = null;
        /// <summary>
        /// 初始化 若不传则默认从appsettings.json读取Connections:DefaultConnect节点
        /// 传入setting:xxx:xxx形式 则会从指定的配置文件中读取内容
        /// 直接传入连接串则
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="jsonConfigFileName"> 配置文件名称</param>
        public DapperHelper(string conn = "", string jsonConfigFileName = "appsettings.json", DatabaseType databaseType = DatabaseType.MySql)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile(jsonConfigFileName, optional: true)
                            .Build();
            if (string.IsNullOrEmpty(conn))
            {
                conn = config.GetSection("ConnectionStrings:ShiDB").Value;
            }
            else if (conn.StartsWith("SqlSugarSettings:"))
            {
                conn = config.GetSection(conn.Substring(8)).Value;
            }
            ConnectionString = conn;

            //调用控制连接不同数据库的库，获取连接数据库的database类
            Connection = ConnectionFactory.CreateConnection(ConnectionString, databaseType);
        }
        public Database GetConnection()
        {
            return Connection;
        }
        /// <summary>
        /// 创建一个事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction TranStart()
        {
            if (Connection.Connection.State == ConnectionState.Closed)
                Connection.Connection.Open();
            return Connection.Connection.BeginTransaction();
        }
        /// <summary>
        /// 事务回滚
        /// </summary>
        /// <param name="tran"></param>
        public void TranRollBack(IDbTransaction tran)
        {
            tran.Rollback();
            if (Connection.Connection.State == ConnectionState.Open)
                tran.Connection.Close();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tran"></param>
        public void TranCommit(IDbTransaction tran)
        {
            tran.Commit();
            if (Connection.Connection.State == ConnectionState.Open)
                tran.Connection.Close();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="obj">实体类</param>
        /// <param name="tran">使用事务提交</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Delete<T>(T obj, IDbTransaction tran = null, int? commandTimeout = null) where T : class
        {
            return Connection.Delete(obj, tran, commandTimeout);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="list">实体类数组</param>
        /// <param name="tran">使用事务提交</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Delete<T>(IEnumerable<T> list, IDbTransaction tran = null, int? commandTimeout = null) where T : class
        {
            return Connection.Delete(list, tran, commandTimeout);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="id">id</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public T Get<T>(string id, IDbTransaction tran = null, int? commandTimeout = null) where T : class
        {
            return Connection.Get<T>(id, tran, commandTimeout);
        }
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="predicate">谓词</param>
        /// <param name="sort">排序？</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">是否缓冲</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, IDbTransaction tran = null, int? commandTimeout = null, bool buffered = true) where T : class
        {
            return Connection.GetList<T>(predicate, sort, tran, commandTimeout, buffered);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="predicate">谓词</param>
        /// <param name="sort">排序</param>
        /// <param name="page">页码</param>
        /// <param name="pagesize">每页大小</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">是否缓冲</param>
        /// <returns></returns>
        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int pagesize, IDbTransaction tran = null, int? commandTimeout = null, bool buffered = true) where T : class
        {
            return Connection.GetPage<T>(predicate, sort, page, pagesize, tran, commandTimeout, buffered);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount, object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class
        {
            if (sort == null)
            {
                sort = new List<ISort>();
            }
            IEnumerable<T> entityList = Connection.Connection.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null, buffered);
            allRowsCount = Connection.Connection.Count<T>(predicate);
            return entityList;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="obj">实体类</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public dynamic Insert<T>(T obj, IDbTransaction tran = null, int? commandTimeout = null) where T : class
        {
            return Connection.Insert(obj, tran, commandTimeout);
        }
        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="list">实体类数组</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        public void Insert<T>(IEnumerable<T> list, IDbTransaction tran = null, int? commandTimeout = null) where T : class
        {
            Connection.Insert(list, tran, commandTimeout);
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="obj">实体类</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="ignoreAllKeyProperties">是否忽视所有字段的内容</param>
        /// <returns></returns>
        public bool Update<T>(T obj, IDbTransaction tran = null, int? commandTimeout = null, bool ignoreAllKeyProperties = true) where T : class
        {
            return Connection.Update(obj, tran, commandTimeout, ignoreAllKeyProperties);
        }
        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="list">实体类数组</param>
        /// <param name="tran">使用事务执行</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="ignoreAllKeyProperties">是否忽视所有字段的内容</param>
        public bool Update<T>(IEnumerable<T> list, IDbTransaction tran = null, int? commandTimeout = null, bool ignoreAllKeyProperties = true) where T : class
        {
            return Connection.Update(list, tran, commandTimeout, ignoreAllKeyProperties);
        }
        /// <summary>
        /// 执行查询，返回类型为T的数据
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="sql">sql字符串</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">使用事务提交</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">解释类型</param>
        /// <returns></returns>
        public List<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType).AsList();
        }
        /// <summary>
        /// 参数化SQL执行
        /// </summary>
        /// <typeparam name="T">实体（表名）</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">使用事务提交</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">解释类型</param>
        /// <returns></returns>
        public int Execute<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
