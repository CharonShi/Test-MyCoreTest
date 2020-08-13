using System;
using System.Linq;
using System.Text;

namespace Shi.Models.DBModel
{
    ///<summary>
    ///成就表
    ///</summary>
    public partial class achievement
    {
           public achievement(){

            this.AchiName =Convert.ToString("0");
            this.Conditon =Convert.ToInt64("0");
            this.CreateDate =Convert.ToDateTime("0000-00-00 00:00:00");

           }
           /// <summary>
           /// Desc:成就名称
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public string AchiName {get;set;}

           /// <summary>
           /// Desc:达成条件(储存量)
           /// Default:0
           /// Nullable:False
           /// </summary>           
           public long Conditon {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:0000-00-00 00:00:00
           /// Nullable:False
           /// </summary>           
           public DateTime CreateDate {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

    }
}
