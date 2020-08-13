using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Shi.IRepository.Base;
using Shi.Models;
using Shi.Models.DBModel;
using Shi.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shi.Service
{
    public class UserBus : BusBase
    {

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool InsertUser(user u)
        {
            return DbBase.Insert<user>(u) > 0;
        }

        /// <summary>
        /// 查询成就
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static achievement GetAchi(int id)
        {
            return DbBase.GetEntity<achievement>(id.ToString());
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public user Login(string UserName)
        {
            var sql = "select * from user where UserName = @name";
            var pred = new
            {
                name = UserName
            };

            return DbBase.GetEntity<user>(sql, pred);
        }


    }
}
