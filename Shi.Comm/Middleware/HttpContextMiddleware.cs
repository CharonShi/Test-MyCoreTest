using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shi.Comm.Middleware
{
    public class HttpContextMiddleware
    {

        public readonly RequestDelegate _next;


        public HttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var method = context.Request.Method;

            var query = new object();

            var api = new ApiRequestInputViewModel
            {
                HttpType = context.Request.Method,
                Query = context.Request.QueryString.Value,
                RequestUrl = context.Request.Path,
                RequestName = "",
                RequestIP = context.Request.Host.Value
            };


            var request = context.Request.Body;
            var response = context.Response.Body;


            var queryData = context.Request.QueryString;

            //using (var newRequest = new MemoryStream())
            //{
            //    context.Request.Body = newRequest;

            //    using (var newResponse = new MemoryStream())
            //    {
            //        context.Response.Body = newResponse;


            //        //using (var reader = new StreamReader(newRequest))
            //        //{
            //        //    api.Body =await reader.ReadToEndAsync();
            //        //    if (string.IsNullOrEmpty(api.Body))
            //        //    {
            //        //        await _next.Invoke(context);
            //        //    }

            //        //}
            //        //using(var writer = new StreamWriter(newRequest))
            //        //{
            //        //    await writer.WriteAsync(api.Body);
            //        //    await writer.FlushAsync();
            //        //    newResponse.Position = 0;
            //        //    context.Request.Body = newRequest;
            //        //    await _next(context);
            //        //}
            //        //using(var reader = new StreamReader(newResponse))
            //        //{
            //        //    api.ResponseBody =await reader.ReadToEndAsync();
            //        //}
            //        //using(var writer = new StreamWriter(newResponse))
            //        //{

            //        //}
            //    }

            //}

            //context.Response.OnCompleted(() =>
            //{
            //    return Task.CompletedTask;
            //});


            switch (method)
            {
                case "GET": query = context.Request.QueryString; break;
                case "DELETE": query = context.Request.QueryString; break;
                case "POST": query = context.Request.Body; break;
                case "PUT": query = context.Request.Body; break;
                default: query = null; break;
            }

            await _next(context);
        }


    }

    class ApiRequestInputViewModel
    {

        public string HttpType { get; set; }
        public string Query { get; set; }
        public string RequestUrl { get; set; }
        public string RequestName { get; set; }
        public string RequestIP { get; set; }
        public string Body { get; set; }
        public string ResponseBody { get; set; }
    }



}
