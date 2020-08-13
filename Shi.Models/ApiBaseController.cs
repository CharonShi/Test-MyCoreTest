using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Shi.Comm;
using Shi.Models.DBModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Shi.Models
{
    public class ApiBaseController : ControllerBase
    {
        private static IMemoryCache _cache;

        protected static List<user> Users = new List<user>();

        protected readonly HttpContextAccessor _httpContextAccessor;

        protected void CurrUser()
        {
            HttpContext.Session.GetString("");
        }
        public ApiBaseController()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        protected user User
        {
            get
            {
                var token = HttpContext.Request.Headers["ShiToken"];

                var userinfo = JWT.GetInfoByToken(token);
                var id = Convert.ToInt32(userinfo.FindFirst(p => p.Type == JwtRegisteredClaimNames.Sid)?.Value);
                return Users.Find(p => p.Id == id);
            }
        }






    }
}
