using System;
using System.Linq;
using System.Text;

namespace Shi.Models.DBModel
{
    ///<summary>
    ///
    ///</summary>
    public partial class userpic
    {
           public userpic(){


           }
           /// <summary>
           /// Desc:图片id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int PicId {get;set;}

           /// <summary>
           /// Desc:用户id
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UserId {get;set;}

    }
}
