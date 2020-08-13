using System;
using System.Collections.Generic;
using System.Text;

namespace Shi.Models
{
    public class ResData
    {

        public int Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }


        private const int OK_CODE = 1; // 成功操作
        private const int ERR = 0; //失败操作        
        private const int NO_ACCESS = 101; //没有操作权限
        private const int OVER_TIME = 102; //登录超时
        private const int NO_TOKEN = 103; //没有TOKEN
        private const int RE_SUBMIT = 104; //重复刷新
        private const int SYSTEM_ERROR = 500; //系统严重错误

        public ResData()
        {
            Code = ResData.OK_CODE;
            Msg = "OK";
        }

        public ResData(object data)
        {
            Data = data;
            Code = OK_CODE;
        }

        public static ResData Error(string msg = "系统错误")
        {
            return new ResData { Code = ResData.ERR, Msg = msg };
        }

    }
}
