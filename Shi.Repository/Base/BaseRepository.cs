using System;
using System.Collections.Generic;
using System.Text;
using Shi.IRepository.Base;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Shi.Models.DBModel;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Shi.Comm;
using CommonHelper;
using System.Data;
using System.Transactions;
using DapperExtensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Shi.Models;

namespace Shi.Repository.Base
{
    public class BaseRepository : IBaseRepository
    {

        private static DapperHelper Dapper { get; set; }
        private static IConfiguration Configuration { get; set; }
        public static string ConnectionString { get; set; }

        public BaseRepository()
        {
            Configuration = AppConfig.Configuration;
            //连接字符串通过Configuration从配置文件appsettings.Development.json中获取
            //Configuration在AppConfig中初始化了
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = Configuration.GetConnectionString("ShiDB");
            }
            if (Dapper == null)
            {
                Dapper = new DapperHelper(ConnectionString, AppConfig.AppSettingsStr, DatabaseType.MySql);
            }
        }

        //获取MySql的连接数据库对象。MySqlConnection
        public MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            //打开连接
            connection.Open();
            //关闭连接
            connection.Close();
            return connection;
        }


        #region 查询

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetEntity<TEntity>(string id) where TEntity : class
        {
            var entity = Dapper.Get<TEntity>(id);
            return entity;
        }
        public TEntity GetEntity<TEntity>(string sql, object predicates) where TEntity : class
        {
            var entity = Dapper.Query<TEntity>(sql, predicates).FirstOrDefault();
            return entity;
        }
        public TEntity GetEntity<TEntity>(string sql, params IPredicate[] predicate) where TEntity : class
        {
            var entity = Dapper.Query<TEntity>(sql, predicate).FirstOrDefault();
            return entity;
        }


        /// <summary>
        /// 查询数组
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetList<TEntity>(string sql) where TEntity : class
        {
            var res = Dapper.Query<TEntity>(sql);
            return res;
        }

        /// <summary>
        /// 查询数组
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="predicates">参数对象</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetList<TEntity>(string sql, object predicates) where TEntity : class
        {
            var res = Dapper.Query<TEntity>(sql, predicates);
            return res;
        }

        /// <summary>
        /// 查询数组
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="predicate">参数类</param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetList<TEntity>(string sql, params IPredicate[] predicate) where TEntity : class
        {
            var predi = Predicates.Group(GroupOperator.And, predicate);
            var res = Dapper.Query<TEntity>(sql, predi);
            return res;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pIndx"></param>
        /// <param name="pSize"></param>
        /// <param name="predicate"></param>
        /// <param name="StorfieldName"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PagerObj<TEntity> GetPager<TEntity>(int pIndx, int pSize, string StorfieldName = "", bool isAsc = true, object predicate = null) where TEntity : class
        {
            long total = 0;
            List<ISort> sort = new List<ISort>();
            if (!string.IsNullOrWhiteSpace(StorfieldName))
            {
                Sort s = new Sort();
                s.PropertyName = StorfieldName;
                s.Ascending = isAsc;
                sort.Add(s);
            }
            var rowData = Dapper.GetPage<TEntity>(predicate, sort, pIndx, pSize);

            PagerObj<TEntity> pager = new PagerObj<TEntity>()
            {
                rowTotal = total,
                rowData = rowData,
                pIndex = pIndx,
                pSize = pSize
            };

            return pager;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pIndx"></param>
        /// <param name="pSize"></param>
        /// <param name="predicate"></param>
        /// <param name="StorfieldName"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PagerObj<TEntity> GetPager<TEntity>(int pIndx, int pSize, string StorfieldName = "", bool isAsc = true, params IPredicate[] predicate) where TEntity : class
        {
            long total = 0;
            List<ISort> sort = new List<ISort>();
            if (!string.IsNullOrWhiteSpace(StorfieldName))
            {
                Sort s = new Sort();
                s.PropertyName = StorfieldName;
                s.Ascending = isAsc;
                sort.Add(s);
            }
            var rowData = Dapper.GetPageList<TEntity>(pIndx, pSize, out total, predicate, sort);

            PagerObj<TEntity> pager = new PagerObj<TEntity>()
            {
                rowTotal = total,
                rowData = rowData,
                pIndex = pIndx,
                pSize = pSize
            };

            return pager;
        }

        public PagerObj<TEntity> GetPager<TEntity>(string TabName, object Param, string StorName = "", string ColField = "") where TEntity : class
        {
            var sql = "select ";

            if (!string.IsNullOrWhiteSpace(ColField))
            {
                sql += ColField;
            }
            else
            {
                sql += "*";
            }

            sql += " where 1=1";

            if (!string.IsNullOrWhiteSpace(StorName))
            {
                sql += StorName;
            }

            var sqlCount = $"select count(*) from {TabName} where 1=1 ";
            var page = new PagerObj<TEntity>();
            page.rowData = Dapper.Query<TEntity>(sql, Param);
            page.rowTotal = Dapper.QueryFirst(sqlCount, Param);

            return page;
        }


        #endregion

        #region 操作

        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            var result = Dapper.Insert<TEntity>(entity);
            return result;
        }

        public void Insert<TEntity>(TEntity[] entity) where TEntity : class
        {
            try
            {
                Dapper.Insert<TEntity>(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("批量新增实体失败:" + ex);
            }

        }

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                return Dapper.Update<TEntity>(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("修改实体失败:" + ex);
            }
        }

        public bool Update<TEntity>(TEntity[] entity) where TEntity : class
        {
            try
            {
                return Dapper.Update<TEntity>(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("批量修改实体失败:" + ex);
            }
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                return Dapper.Delete<TEntity>(entity);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("删除实体失败:" + ex);
            }
        }

        public bool Delete<TEntity>(TEntity[] entity) where TEntity : class
        {
            try
            {
                return Dapper.Delete<TEntity>(entity);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("批量删除实体失败:" + ex);
            }
        }


        public void InsertTran<TEntity>(TEntity[] entity) where TEntity : class
        {
            IDbTransaction tran = Dapper.TranStart();
            try
            {
                Dapper.Execute<TEntity>("", null, tran);
                Dapper.Execute<TEntity>("");
                Dapper.TranCommit(tran);
            }
            catch (Exception ex)
            {
                Dapper.TranRollBack(tran);
                throw new ApplicationException("批量新增实体失败:" + ex);
            }

        }

        #endregion
    }
}
