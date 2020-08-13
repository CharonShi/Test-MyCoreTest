using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shi.Comm.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Content = "服务器错误：" + context.Exception.Message,
                ContentType = "text/html;charset=utf-8"
            };

            context.ExceptionHandled = true;
        }


        public Task OnExceptionAsync(ExceptionContext context)
        {

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Content = "服务器错误（异步）：" + context.Exception.Message,
                ContentType = "text/html;charset=utf-8"
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

    }
}
