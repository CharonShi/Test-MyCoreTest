using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shi.Comm
{
    /// <summary>
    /// 未实现 暂使用token生成机制
    /// </summary>
    public class JWT
    {

        public static string siginKey = "Asten-1998-Aichijingdexia-Charon-2020"; // 密钥

        public static string issuerUrl = "http://localhost:53333"; // 发证地址

        public static string audienceUrl = "http://192.168.2.141:2525"; // 收证地址



        /// <summary>
        /// 生成token
        /// </summary>
        /// <returns></returns>
        public static string CreateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siginKey));

            var token = new JwtSecurityToken(
                issuer: issuerUrl,
                audience: audienceUrl,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }


        public static ClaimsPrincipal GetInfoByToken(string token)
        {

            var handler = new JwtSecurityTokenHandler();

            return handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(siginKey)),
                ValidateLifetime = false
            }, out SecurityToken validatedToken);
        }


    }
}
