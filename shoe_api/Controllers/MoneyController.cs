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
        //财务支出收入详情
        [Route("POSTAllDataMoney")]
        [HttpPost]
        public BaseDataTables Money_out_in([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;

            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();

            Pagedata.draw = obj.draw;

            //查询条件
            string info = obj.search.value;

            //根据对应页码和条数进行查询
            //var list1 = from pp in db.order
            //            join od in db.customer
            // on pp.customer_id equals od.customer_id
            //            select new
            //            {
            //                orderr_id = pp.orderr_id,
            //                customer_name = od.customer_name,
            //                order_starttime = pp.order_starttime,
            //                order_endtime = pp.order_endtime,
            //                operator_per = pp.operator_per,
            //                order_paid = pp.order_paid,
            //                order_unpaid = pp.order_unpaid,
            //                order_status = pp.order_status
            //            } into q
            //            where (q.orderr_id.Contains(info))
            //            select q; ;
            var list1 = db.out_in_money.ToList();
            //查询数据表总共有多少条记录
            int rows1 = db.out_in_money.ToList().Count;

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
