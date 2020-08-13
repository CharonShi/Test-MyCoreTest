using Shi.IRepository.Base;
using Shi.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shi.Service
{
    public class BusBase
    {
        public static IBaseRepository DbBase;
        public BusBase()
        {
            DbBase = new BaseRepository();
        }






    }
}
