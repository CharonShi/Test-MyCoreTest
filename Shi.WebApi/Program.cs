/* 2020-5-25
 * ʹ��SqlSugar�Զ�����ʵ��
 * SqlSugar���Զ�������ݿ⣬����ֶθ��¡�������ɾ�������ǲ��ܼ�⵽��ɾ��
 * �������ݿ�ĳ����ɾ������Ҫ�ֶ�����Ŀ�ڽ���Ӧʵ��ɾ��
 * ���ܷ��滹δ������
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
            //ͨ��Configuration��ȡSqlSugar��������Ϣ
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();
            //��ȡ�����ַ���
            var conn = config.GetSection("ConnectionStrings:ShiDB").Value;
            string path = string.Empty;

            //��ȡ��ʵ�屣������·��
            var relativePath = config.GetSection("SqlSugarSettings:RelativePath").Value;

            //�Զ�������㲢 �ҵ������ ���㸽�ӵ�������Ŀ��
            if (!string.IsNullOrEmpty(relativePath))
            {
                //��ȡ��ǰӦ�ó���Ĺ���Ŀ¼
                var basePath = new DirectoryInfo(Directory.GetCurrentDirectory());
                while ((basePath.FullName.Contains(@"\Debug") || basePath.FullName.Contains(@"\bin")) && !string.IsNullOrEmpty(basePath.FullName))
                {
                    //��ȡָ����Ŀ¼�ĸ�Ŀ¼
                    basePath = basePath.Parent;
                }
                //�������ַ����ϳ�һ��·��
                path = Path.Combine(basePath.Parent.FullName, relativePath);

            }

            //��ȡ��ʵ�屣���ȫ·����ַ ����֮�� ���·����ַʧЧ
            var fullPath = config.GetSection("SqlSugarSettings:FullPath").Value;
            if (!string.IsNullOrEmpty(fullPath))
            {
                path = fullPath;
            }

            InitModel(conn, config.GetSection("SqlSugarSettings:NameSpace").Value, path, config.GetSection("SqlSugarSettings:GenerateTables").Value);

            //��������ִ�У�����Startup
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn">�����ַ���</param>
        /// <param name="namespaceStr">ʵ�������ڵ������ռ�</param>
        /// <param name="path">ʵ�������ڵĵ�ַ</param>
        /// <param name="genaratetable">��Ҫ��������µı���</param>
        public static void InitModel(string conn, string namespaceStr, string path, string genaratetable)
        {
            try
            {
                //��ȡ��Ҫ��������µı�����û�����õĻ�������ȫ���ı�
                var tableName = genaratetable.Split(',').ToList();
                if (string.IsNullOrWhiteSpace(genaratetable))
                {
                    tableName = new List<string>();
                }
                for (int i = 0; i < tableName.Count; i++)
                {
                    tableName[i] = tableName[i].ToLower();
                }

                //��ȡ������ɵ�SqlSugarClient
                var sugar = GetInstance(conn).DbFirst.SettingClassTemplate(old =>
                {
                    return old.Replace("{Namespace}", namespaceStr);
                });
                
                if (tableName.Count > 0)
                {
                    sugar.Where(it => tableName.Contains(it.ToLower())).IsCreateDefaultValue();
                }
                //δ���ñ���
                else
                {
                    sugar.IsCreateDefaultValue();
                }
                //����BaseEntity�д��ڵ��ֶ�
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
                IsShardSameThread = true //��Ϊtrue��ͬ�߳���ͬһ��SqlSugarClient
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
