/* 2020-5-25
 * 使用SqlSugar自动生成实体
 * SqlSugar会自动检测数据库，表和字段更新、新增、删除，但是不能检测到表被删除
 * 所有数据库某个表删除后，需要手动到项目内将对应实体删除
 * 性能方面还未做测试
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Shi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //通过Configuration获取SqlSugar的配置信息
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();
            //获取连接字符串
            var conn = config.GetSection("ConnectionStrings:ShiDB").Value;
            string path = string.Empty;

            //获取将实体保存的相对路径
            var relativePath = config.GetSection("SqlSugarSettings:RelativePath").Value;

            //自动找最外层并 找到更外层 方便附加到其他项目中
            if (!string.IsNullOrEmpty(relativePath))
            {
                //获取当前应用程序的工作目录
                var basePath = new DirectoryInfo(Directory.GetCurrentDirectory());
                while ((basePath.FullName.Contains(@"\Debug") || basePath.FullName.Contains(@"\bin")) && !string.IsNullOrEmpty(basePath.FullName))
                {
                    //获取指定子目录的父目录
                    basePath = basePath.Parent;
                }
                //将两个字符串合成一个路径
                path = Path.Combine(basePath.Parent.FullName, relativePath);

            }

            //获取将实体保存的全路径地址 配置之后 相对路径地址失效
            var fullPath = config.GetSection("SqlSugarSettings:FullPath").Value;
            if (!string.IsNullOrEmpty(fullPath))
            {
                path = fullPath;
            }

            InitModel(conn, config.GetSection("SqlSugarSettings:NameSpace").Value, path, config.GetSection("SqlSugarSettings:GenerateTables").Value);

            //继续往下执行，进入Startup
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn">链接字符串</param>
        /// <param name="namespaceStr">实体类所在的命名空间</param>
        /// <param name="path">实体类所在的地址</param>
        /// <param name="genaratetable">需要新增或更新的表名</param>
        public static void InitModel(string conn, string namespaceStr, string path, string genaratetable)
        {
            try
            {
                //获取需要新增或更新的表名，没有设置的话将检索全部的表
                var tableName = genaratetable.Split(',').ToList();
                if (string.IsNullOrWhiteSpace(genaratetable))
                {
                    tableName = new List<string>();
                }
                for (int i = 0; i < tableName.Count; i++)
                {
                    tableName[i] = tableName[i].ToLower();
                }

                //获取配置完成的SqlSugarClient
                var sugar = GetInstance(conn).DbFirst.SettingClassTemplate(old =>
                {
                    return old.Replace("{Namespace}", namespaceStr);
                });
                
                if (tableName.Count > 0)
                {
                    sugar.Where(it => tableName.Contains(it.ToLower())).IsCreateDefaultValue();
                }
                //未设置表名
                else
                {
                    sugar.IsCreateDefaultValue();
                }
                //过滤BaseEntity中存在的字段
                //var pros = typeof(BaseEntity).GetProperties();
                //var list = new List<SqlSugar.IgnoreColumn>();
                var tables = sugar.ToClassStringList().Keys;
                //foreach (var item in pros)
                //{
                //    foreach (var table in tables)
                //    {
                //        list.Add(new SqlSugar.IgnoreColumn() { EntityName = table, PropertyName = item.Name });
                //    }
                //}
                //suger.Context.IgnoreColumns.AddRange(list);
                sugar.CreateClassFile(path);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static SqlSugarClient GetInstance(string conn)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conn,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                IsShardSameThread = true //设为true相同线程是同一个SqlSugarClient
            });
            db.Ado.IsEnableLogEvent = true;

            return db;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
