using Shi.Models;
using Shi.Models.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shi.Service
{
    public class PictrueBus : BusBase
    {

        public static List<picture> GetPicPage(int pIndx, int pSize, int userId)
        {
            var pred = new
            {
                UserId = userId
            };

            var list = DbBase.GetPager<userpic>(pIndx, pSize).rowData.ToList();
            var llist = DbBase.GetPager<picture>("picture",pred);

            var ids = from p in list select p.PicId;
            var ids_ = string.Join(',', ids);
            if (ids.Count() > 0)
            {
                var sql = "select * from picture where Id in (" + ids_ + ")";
                var page = DbBase.GetList<picture>(sql);
                return page.ToList();
            }
            return null;
        }

        public static void InsertPicture(picture[] pic)
        {
            DbBase.Insert<picture>(pic);
        }


    }
}
