using System;
using System.Collections.Generic;
using System.Text;

namespace Shi.Models
{
    public class PagerObj<TEntity>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int pIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int pSize { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public long rowTotal { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<TEntity> rowData { get; set; }
    }
}
