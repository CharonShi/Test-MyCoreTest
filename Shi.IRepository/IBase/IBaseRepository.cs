using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shi.IRepository.Base
{
    public interface IBaseRepository
    {
        bool Delete<TEntity>(TEntity entity) where TEntity : class;
        bool Delete<TEntity>(TEntity[] entity) where TEntity : class;
        TEntity GetEntity<TEntity>(string id) where TEntity : class;
        TEntity GetEntity<TEntity>(string sql, object predicates) where TEntity : class;
        TEntity GetEntity<TEntity>(string sql, params IPredicate[] predicate) where TEntity : class;
        IEnumerable<TEntity> GetList<TEntity>(string sql) where TEntity : class;
        IEnumerable<TEntity> GetList<TEntity>(string sql, object predicates) where TEntity : class;
        IEnumerable<TEntity> GetList<TEntity>(string sql, params IPredicate[] predicate) where TEntity : class;
        Models.PagerObj<TEntity> GetPager<TEntity>(string TabName, object Param, string StorName = "", string ColField = "") where TEntity : class;
        Models.PagerObj<TEntity> GetPager<TEntity>(int pIndx, int pSize, string StorfieldName = "", bool isAsc = true, object predicate = null) where TEntity : class;
        Models.PagerObj<TEntity> GetPager<TEntity>(int pIndx, int pSize, string StorfieldName = "", bool isAsc = true, params IPredicate[] predicate) where TEntity : class;
        int Insert<TEntity>(TEntity obj) where TEntity : class;
        void Insert<TEntity>(TEntity[] obj) where TEntity : class;
        void InsertTran<TEntity>(TEntity[] obj) where TEntity : class;
        bool Update<TEntity>(TEntity obj) where TEntity : class;
        bool Update<TEntity>(TEntity[] obj) where TEntity : class;
    }
}
