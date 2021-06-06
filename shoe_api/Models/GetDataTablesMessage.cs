using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shoe_api.Models
{
    /// <summary>
    /// 接收DataTables回传数据model
    /// </summary>
    public class GetDataTablesMessage
    {
        /// <summary>
        /// DataTables请求和返回都是固定的值
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 从哪行开始
        /// </summary>
        public int start { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 查询集合
        /// </summary>
        public search search { get; set; }

        /// <summary>
        /// 排序集合
        /// </summary>
        public List<order> order { get; set; }

        /// <summary>
        /// 列集合
        /// </summary>
        public List<columns> columns { get; set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public int parameter1 { get; set; }
        /// <summary>
        /// 参数2
        /// </summary>
        public int parameter2 { get; set; }
        /// <summary>
        /// 参数3
        /// </summary>
        public int parameter3 { get; set; }
        /// <summary>
        /// 参数4
        /// </summary>
        public int parameter4 { get; set; }
    }

    /// <summary>
    /// 列
    /// </summary>
    public class columns
    {
        /// <summary>
        /// 列值
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否单列查询
        /// </summary>
        public bool searchable { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool orderable { get; set; }

        /// <summary>
        /// 查询实体
        /// </summary>
        public search search { get; set; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    partial class order
    {
        /// <summary>
        /// 
        /// </summary>
        public int column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string dir { get; set; }
    }

    /// <summary>
    /// DataTables 列查询值
    /// </summary>
    public class search
    {
        /// <summary>
        /// 查询值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 正则
        /// </summary>
        public bool regex { get; set; }
    }
}