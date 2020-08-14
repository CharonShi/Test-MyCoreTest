using System;
using System.Linq;
using System.Text;

namespace Shi.Models.DBModel
{
    ///<summary>
    ///图片表
    ///</summary>
    public partial class picture
    {
           public picture(){


           }
           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateDate {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:图片介绍
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string PicInfo {get;set;}

           /// <summary>
           /// Desc:图片名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string PicName {get;set;}

           /// <summary>
           /// Desc:图片地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string PicPath {get;set;}

           /// <summary>
           /// Desc:图片大小
           /// Default:
           /// Nullable:False
           /// </summary>           
           public long PicSize {get;set;}

           /// <summary>
           /// Desc:图片类型
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int PicType {get;set;}

           /// <summary>
           /// Desc:图片地址
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Url {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UserId {get;set;}

    }
}
