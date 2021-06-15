using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class customerController : ApiController
    {
        ShoeEntities1 db = new ShoeEntities1();
        //[HttpGet]
        ////客户信息查询
        //public string cut()
        //{
        //    //防止序列化恶性循环===========================
        //    db.Configuration.ProxyCreationEnabled = false;
        //    //==================================================
        //    var info = db.customer.ToList();
        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
        //    return json;
        //}

        //public string odr() 
        //{
        //    //防止序列化恶性循环===========================
        //    db.Configuration.ProxyCreationEnabled = false;
        //    //==================================================
        //    var info1 = db.order.ToList();
        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(info1);
        //    return json;
        //}


        //查询客户信息
        [HttpPost]
        public BaseDataTables Cut([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info = "";
            if (obj.search.value!=null)
            {
                info = obj.search.value;
            }

            var list1 = db.customer.ToList().Where(c => c.customer_id.ToString().Contains(info)
            || c.customer_name.ToString().Contains(info) || c.customer_phone.ToString().Contains(info)
            || c.customer_linkman.ToString().Contains(info) || c.customer_email.ToString().Contains(info)
            || c.customer_address.ToString().Contains(info) || c.customer_fax.ToString().Contains(info)
            || c.customer_postcode.ToString().Contains(info) || c.bank.ToString().Contains(info)
            || c.bank_account.ToString().Contains(info));

            //查询数据表总共有多少条记录
            int rows1 = db.customer .ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //var list=new List<admin>();
            //if (obj.search.value != null)
            //{
            //    rows2 = db.admin.Where(a => a.name == obj.search.value).ToList().Count;
            //    list1 = db.xp_adminPage(obj.length, obj.start, obj.search.value).ToList();
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

        //查询客户订单信息
        [HttpPost]
        public BaseDataTables Ord([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info1 = "";
            if (obj.search.value!=null)
            {
                info1 = obj.search.value;
            }

            var list1 = from pp in db.order
                        join od in db.customer
             on pp.customer_id equals od.customer_id
                        select new
                        {
                            orderr_id = pp.orderr_id,
                            customer_name = od.customer_name,
                            order_starttime = pp.order_starttime,
                            order_endtime = pp.order_endtime,
                            operator_per = pp.operator_per,
                            order_paid = pp.order_paid,
                            order_unpaid = pp.order_unpaid,
                            order_status = pp.order_status
                        } into q
                        where (q.orderr_id.Contains(info1))
                        select q;

            //查询数据表总共有多少条记录
            int rows1 = db.order.ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //var list=new List<admin>();
            //if (obj.search.value != null)
            //{
            //    rows2 = db.admin.Where(a => a.name == obj.search.value).ToList().Count;
            //    list1 = db.xp_adminPage(obj.length, obj.start, obj.search.value).ToList();
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
