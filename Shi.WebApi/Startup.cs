using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shi.Comm;
using Microsoft.OpenApi.Models;
using Shi.Comm.Middleware;
using Shi.Comm.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            if (env != null)
            {
                AppConfig.Init(env.EnvironmentName);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();//启用内存缓存
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(120); //session过期时间
                option.Cookie.Name = "ShiCookie";
                option.Cookie.HttpOnly = true;  //设置在浏览器不能通过js获得该cookie的值
            });//启用session

            //配置跨域处理，允许所有来源：
            services.AddCors(options =>
                options.AddPolicy("ShiCors",
                p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
            );

            services.AddMvc(fit =>
            {
                fit.Filters.Add(new RequeryFilter()); //请求参数拦截器
                fit.Filters.Add(typeof(ExceptionFilter)); //异常捕捉拦截器
            });

            //jwt 验证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //验证密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.siginKey)),

                    ValidateIssuer = true, // 验证发证地址
                    ValidIssuer = JWT.issuerUrl,

                    ValidateAudience = true, // 验证收证地址
                    ValidAudience = JWT.audienceUrl,
                };
            });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Shi.WebApi", Version = "v1" }); //swagger
            });

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("ShiCors");//必须位于UserMvc之前 

            app.UseHttpContextMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            //app.UseMiddleware<Middleware1>();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Shi.WebApi");
                s.RoutePrefix = string.Empty;
            });




            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
