using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{

    [RoutePrefix("Money")]
    public class MoneyController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        //财务收入
        [HttpPost]
        public BaseDataTables Money_in([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;

            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();

            Pagedata.draw = obj.draw;

            //查询条件
            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            //根据对应页码和条数进行查询
            var list1 = db.select_in_money().ToList().Where(p => p.out_in_id.Contains(info) || p.out_in_money_id.ToString().Contains(info) || p.product_type.Contains(info));
            //查询数据表总共有多少条记录
            int rows1 = db.select_in_money().ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //if (obj.search.value != null)
            //{
            //    rows2 = db.materials_plan.Where(a => a.name == obj.search.value).ToList().Count;
            //    list1 = db.materials_plan.ToList();
            //}

            /// <summary>
            /// 即没有过滤的记录数（数据库里总共记录数）
            /// </summary>
            Pagedata.recordsTotal = rows1;

            /// <summary>
            /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
            /// </summary>
            Pagedata.recordsFiltered = rows2;

            Pagedata.data = list1;

            return Pagedata;
        }
        //财务支出
        [HttpPost]
        public BaseDataTables Money_out([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;

            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();

            Pagedata.draw = obj.draw;

            //查询条件
            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            //根据对应页码和条数进行查询
            var list1 = db.select_out_money().ToList().Where(p => p.out_in_id.ToString().Contains(info));
            //查询数据表总共有多少条记录
            int rows1 = db.select_out_money().ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //if (obj.search.value != null)
            //{
            //    rows2 = db.materials_plan.Where(a => a.name == obj.search.value).ToList().Count;
            //    list1 = db.materials_plan.ToList();
            //}

            /// <summary>
            /// 即没有过滤的记录数（数据库里总共记录数）
            /// </summary>
            Pagedata.recordsTotal = rows1;

            /// <summary>
            /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
            /// </summary>
            Pagedata.recordsFiltered = rows2;

            Pagedata.data = list1;

            return Pagedata;
        }
    }
}
