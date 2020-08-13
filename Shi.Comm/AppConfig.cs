using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Shi.Comm
{
    public class AppConfig
    {
        static AppConfig appConfig = new AppConfig();
        public static AppConfig Data
        {
            get
            {
                return appConfig;
            }
        }

        /// <summary>
        /// 用来获取程序配置属性(appsettings.Development(或者其他).json)的基类
        /// </summary>
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// 获取当前使用的配置文件名称
        /// </summary>
        public static string AppSettingsStr { get; set; }

        public static void Init(string nev) //在Startup中,程序运行最开始的时候被调用，参数nev被传入
        {
            //初始化初始化Configuration，已配置文件appsettings.Development.json关联，
            //之后可用直接通过Configuration获取配置文件信息
            Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource
            {
                Path = $"appsettings.{nev}.json",
                ReloadOnChange = true
            }).Build();

            AppSettingsStr = $"appsettings.{nev}.json";
        }









    }
}
