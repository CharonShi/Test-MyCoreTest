using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shi.Comm.Middleware
{
    public class Middleware1
    {
        private readonly RequestDelegate _next;

        public Middleware1(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            await _next(context);//把context传进去执行下一个中间件


            //HttpContent cont = new FormUrlEncodedContent(new Dictionary<string, string>());


            await context.Response.WriteAsync("<p>Response1</p>");//响应出去时逻辑，为了验证顺序性，输出一句话
        }



    }
}
