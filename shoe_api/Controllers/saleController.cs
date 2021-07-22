using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class saleController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        //查询客户订单
        [HttpPost]
        public BaseDataTables customer_order([FromBody] GetDataTablesMessage obj)
        {
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;
            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }
            //根据对应页码和条数进行查询
            var list1 = (from pp in db.order
                        join od in db.customer
             on pp.customer_id equals od.customer_id
                        select new
                        {
                            orderr_id = pp.orderr_id,
                            customer_name = od.customer_name,
                            order_starttime = pp.order_starttime,
                            order_endtime = pp.order_endtime,
                            person_handling = pp.person_handling,
                            order_paid = pp.order_paid,
                            order_unpaid = pp.order_unpaid,
                            order_status = pp.order_status
                        } into q
                        where (q.orderr_id.Contains(info)||q.customer_name.Contains(info)||
                        q.order_starttime.ToString().Contains(info) || q.order_endtime.ToString().Contains(info) ||
                        q.person_handling.Contains(info) || q.order_paid.ToString().Contains(info) ||
                        q.order_unpaid.ToString().Contains(info) || q.order_status==(info))
                        orderby q.order_starttime descending
                        select q).Skip(obj.start).Take(obj.length);
            //查询数据表总共有多少条记录
            int rows1 = db.order.ToList().Count;
            //记录过滤后的条数
            int rows2 = rows1;
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
        //查询客户订单详情
        [HttpGet]
        public string customer_order_detail(string id)
        {
            var list1 = db.Database.SqlQuery<customer_order_details_Result>("exec customer_order_details "+id).ToList();
            int s = list1.Count();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //订单ID
        [HttpGet]
        public string customerID()
        {
            var list1 = from o in db.order
                        select new { order_id = o.orderr_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        [HttpPost]
        //新增出库详情
        public int add_out_repertory(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //先新增计划表
            string q = json1.Root["proid"].ToString();
            out_repertory pp = new out_repertory();
            pp.orderr_id = q;
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.out_time = ((DateTime)json1.Root["time2"]);
            pp.person_handling = json1.Root["person_handling"].ToString();
            pp.end_price = json1.Root["price"].ToString() ;
            //首先新增计划表数据
            db.out_repertory.Add(pp);
            //保存数据
            db.SaveChanges();

            //订单状态修改
            //修改订单的处理状态
            order o = db.order.FirstOrDefault(p => p.orderr_id == q);
            o.orderr_id = q;
            o.order_status = "已出售";
            db.Entry(o).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return 2;
        }
        //出库详情
        [HttpGet]
        public string select_customer_details(int id)
        {
            var list1 = db.Database.SqlQuery<select_customer_details_Result>("exec select_customer_details " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //销售订单的查询
        [HttpGet]
        public string sale_order(string id)
        {
            var list1 = db.Database.SqlQuery<select_order_details_cust_pro_Result>("exec select_order_details_cust_pro " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
    }
}
