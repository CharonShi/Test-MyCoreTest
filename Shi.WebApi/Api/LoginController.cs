using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shi.Models;
using Shi.Models.DBModel;
using Shi.Service;
using System.Web;
using Shi.Comm;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Shi.WebApi.Api
{

    [Route("api/[controller]")]
    public class LoginController : ApiBaseController
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(string UserName, string Password)
        {

            if (string.IsNullOrWhiteSpace(UserName))
            {
                return Ok(ResData.Error("账号不能为空"));
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                return Ok(ResData.Error("密码不能为空"));
            }
            UserBus bus = new UserBus();
            var u = bus.Login(UserName);
            if (u == null)
            {
                return Ok(ResData.Error("账号密码错误"));
            }
            var pwd = PwdHelper.AESDecryptV(u.Password);
            if (!Password.Equals(pwd))
            {
                return Ok(ResData.Error("账号密码错误"));
            }

            var json = JsonSerializer.Serialize(u);

            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sid,$"{u.Id}"),
            };

            var token = JWT.CreateToken(claims);
            Users.Add(u);

            return Ok(new ResData(token));
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register(user u)
        {
            if (string.IsNullOrWhiteSpace(u.UserName))
            {
                return Ok(ResData.Error("账号不能为空"));
            }
            if (string.IsNullOrWhiteSpace(u.Password))
            {
                return Ok(ResData.Error("密码不能为空"));
            }
            if (string.IsNullOrWhiteSpace(u.NikeName))
            {
                return Ok(ResData.Error("昵称不能为空"));
            }
            if (string.IsNullOrWhiteSpace(u.Phone))
            {
                return Ok(ResData.Error("手机号不能为空"));
            }

            //加密
            var AESPwd = PwdHelper.AESEncryptV(u.Password);

            u.Password = AESPwd;
            u.CreateDate = DateTime.Now;
            u.AchievementLevel = 1;
            u.Achievement = UserBus.GetAchi(u.AchievementLevel).AchiName;
            u.BoxSize = 2147483648;

            UserBus bus = new UserBus();
            var bol = bus.InsertUser(u);
            if (!bol)
            {
                return Ok(ResData.Error("新增失败"));
            }
            return Ok(new ResData());
        }



    }
}