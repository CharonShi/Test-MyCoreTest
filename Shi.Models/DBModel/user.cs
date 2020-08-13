using System;
using System.Linq;
using System.Text;

namespace Shi.Models.DBModel
{
    ///<summary>
    ///用户表
    ///</summary>
    public partial class user
    {
           public user(){

            this.Achievement =Convert.ToString("");
            this.AchievementLevel =Convert.ToInt32("1");
            this.BoxSize =Convert.ToInt64("2147483648");

           }
           /// <summary>
           /// Desc:成就
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Achievement {get;set;}

           /// <summary>
           /// Desc:成就等级
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public int AchievementLevel {get;set;}

           /// <summary>
           /// Desc:所持空间大小
           /// Default:2147483648
           /// Nullable:False
           /// </summary>           
           public long BoxSize {get;set;}

           /// <summary>
           /// Desc:注册时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateDate {get;set;}

           /// <summary>
           /// Desc:电子邮箱
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string EMail {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:最后登录时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? LastLoginTime {get;set;}

           /// <summary>
           /// Desc:昵称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string NikeName {get;set;}

           /// <summary>
           /// Desc:密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Password {get;set;}

           /// <summary>
           /// Desc:手机号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Phone {get;set;}

           /// <summary>
           /// Desc:用户头像
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string UserHeadImg {get;set;}

           /// <summary>
           /// Desc:账户
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

    }
}
