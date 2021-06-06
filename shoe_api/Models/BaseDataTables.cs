using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shoe_api.Models
{
    /// <summary>
    /// DataTables返回实体
    /// </summary>
    public class BaseDataTables
    {
        /// <summary>
        /// Datatables发送的draw是多少那么服务器就返回多少
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 即没有过滤的记录数（数据库里总共记录数）
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// 对象数组
        /// </summary>
        public IEnumerable data { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string error { get; set; }
    }
}