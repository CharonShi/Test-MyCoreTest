using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shi.Comm.Filters
{
    /// <summary>
    /// 请求参数过滤器
    /// </summary>
    public class RequeryFilter : ActionFilterAttribute
    {


        private static IMemoryCache _cache;

        static RequeryFilter()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }


        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var request = context.HttpContext.Request;
            
            var path = request.Path.Value;
            var pathController = path.Split('/')[2]; //获取控制器名称

            if (!request.Headers.ContainsKey("ShiToken") && !IsPublicController(pathController))
            {
                throw new Exception("请求头必须携带ShiToken");
            }

            if (request != null)
            {
                var queryString = request.QueryString.ToString();

                queryString = queryString.Replace("a", "");

                var query = request.Query as IEnumerable<KeyValuePair<string, StringValues>>;

                //同一IP在1秒内多次进行除Get外的请求，将报请求频繁
                if (request.Method.ToUpper() != "GET")
                {
                    var OutTime = _cache.Get(request.Host.Host);
                    if (OutTime != null)
                    {
                        throw new Exception("请求频繁");
                    }
                    else
                    {
                        _cache.Set(request.Host.Host, request.Host.Host, new TimeSpan(0, 0, 1));
                    }
                }

                switch (context.HttpContext.Request.Method.ToUpper())
                {
                    case "POST":
                        //if (request.ContentType.IndexOf("application/json") < 0)
                        //{
                        //    var form = request.Form as IEnumerable<KeyValuePair<string, StringValues>>;
                        //    foreach (KeyValuePair<string, StringValues> item in form)
                        //    {
                        //        if (IsSqlAttack(item.Value))
                        //        {
                        //            throw new Exception("请求参数包含敏感词");
                        //        }
                        //    }
                        //}
                        ; break;
                    case "GET":
                        foreach (KeyValuePair<string, StringValues> item in query)
                        {
                            if (IsSqlAttack(item.Value))
                            {
                                throw new Exception("请求参数包含敏感词");
                            }
                        }
                        ; break;
                    case "PUT":; break;
                    case "DELETE":; break;
                    default:; break;
                }
            }
            return base.OnActionExecutionAsync(context, next);
        }

        private bool IsSqlAttack(string qString)
        {
            qString = qString.Trim();
            qString = qString.ToUpper();
            string[] sqlA = new string[] {
                "EXEC","INSERT","SELECT","DROP",
                "DELETE","UPDATE","CHR","MID",
                "MASTER","TRUNCATE","CHAR",
                "DECLARE","JOIN","DBCC","SYSOBJECT",
                ";","--","'" };

            foreach (var item in sqlA)
            {
                if (qString.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPublicController(string controller)
        {
            string[] publicC = new string[]
            {
                "Login"
            };

            foreach (var item in publicC)
            {
                if (controller.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 在执行操作方法后调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 在执行操作方法前调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 在执行操作结果后调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 在执行操作结果前调用
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }

    }


}
