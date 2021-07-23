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
    public class customerController : ApiController
    {
        
        ShoeEntities db = new ShoeEntities();
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
            || c.customer_name.ToString().Contains(info) ||
            c.customer_linkman.ToString().Contains(info)).Skip(obj.start).Take(obj.length);

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

            var list1 = (from pp in db.order
                        join od in db.customer
             on pp.customer_id equals od.customer_id
                        select new
                        {
                            orderr_id = pp.orderr_id,
                            customer_name = od.customer_name,
                            order_starttime = ((DateTime)pp.order_starttime),
                            order_endtime = ((DateTime)pp.order_endtime),
                            operator_per = pp.operator_per,
                            order_paid = pp.order_paid,
                            order_unpaid = pp.order_unpaid,
                            order_status = pp.order_status
                        } into q
                        where (q.orderr_id.ToString().Contains(info1))
                         orderby q.order_endtime descending
                         select q).Skip(obj.start).Take(obj.length);

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

        //客户订单详情
        [HttpGet]
        public string order_details(string id)
        {
            var list1 = db.Database.SqlQuery<select_Pro_order_datails_Result >("exec select_Pro_order_datails " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //查询要修改的客户信息
        [HttpPost]
        public string Updatecustomer([FromBody]customer cc)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = from c in db.customer
                       where (c.customer_id == cc.customer_id)
                       select c;
            int s = list.Count();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list); 
        }
        //修改客户信息
        [HttpPost]
        public string UpCustomer(string json) 
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //db.Configuration.ProxyCreationEnabled = false;
            int id = int.Parse(json1.Root["customer_id"].ToString());
            customer cu = db.customer.FirstOrDefault(p => p.customer_id == id);
            cu.customer_id = id;
            cu.customer_name = json1.Root["customer_name"].ToString();
            cu.customer_address = json1.Root["customer_address"].ToString();
            cu.bank = json1.Root["bank"].ToString();
            cu.bank_account = json1.Root["bank_account"].ToString();
            cu.customer_email = json1.Root["customer_email"].ToString();
            cu.customer_fax = json1.Root["customer_fax"].ToString();
            cu.customer_linkman = json1.Root["customer_linkman"].ToString();
            cu.customer_phone = json1.Root["customer_phone"].ToString();
            cu.customer_postcode = json1.Root["customer_postcode"].ToString();
            db.Entry(cu).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return "";
        }
        [HttpPost]
        //新增客户信息
        public int addcustomer(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //新增客户信息表
            customer pp = new customer();
            pp.customer_name = json1.Root["customer_name"].ToString();
            pp.customer_address = json1.Root["customer_address"].ToString();
            pp.bank = json1.Root["bank"].ToString();
            pp.bank_account = json1.Root["bank_account"].ToString();
            pp.customer_email = json1.Root["customer_email"].ToString();
            pp.customer_fax = json1.Root["customer_fax"].ToString();
            pp.customer_linkman = json1.Root["customer_linkman"].ToString();
            pp.customer_phone = json1.Root["customer_phone"].ToString();
            pp.customer_postcode = json1.Root["customer_postcode"].ToString();
            //首先新增领料单表数据
            db.customer.Add(pp);
            //保存数据
            db.SaveChanges();
            return 0;
        }
        //查询所有产品规格，用来显示新增产品规格的下拉框内
        [HttpPost]
        public string pro_guige([FromBody] product pp)
        {
            var info = from p in db.product
                       where (p.product_name.Contains(pp.product_name))
                       select new { pro_guige = p.pro_guige };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }
        //根据产品名查询产品信息
        [HttpPost]
        public string pro_name_info([FromBody] product pp)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var info = from p in db.product
                       where (p.product_name.Contains(pp.product_name) && p.pro_guige == pp.pro_guige)
                       select p;
            int id = info.Count();
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }
        //新增客户订单
        [HttpPost]
        public int add_customer_order(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增计划表+
            string custemer_name = json1.Root["customer_name"].ToString();
            //根据名字查询用户ID
            string orderrid = (((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000) * 1000).ToString();
            int custmerid = db.customer.Where
                (p=>p.customer_name.Contains(custemer_name)).ToList()[0].customer_id;
            order pp = new order();
            pp.orderr_id = "X"+orderrid;
            pp.customer_id = custmerid;
            pp.order_starttime = (DateTime)json1.Root["order_starttime"];
            pp.order_endtime = ((DateTime)json1.Root["order_endtime"]);
            pp.person_handling = json1.Root["person_handling"].ToString();
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.order_paid = decimal.Parse(json1.Root["order_paid"].ToString());
            pp.order_unpaid = decimal.Parse(json1.Root["order_unpaid"].ToString());
            pp.order_delivery_way = json1.Root["order_delivery_way"].ToString();
            pp.order_status = json1.Root["order_status"].ToString();
            //首先新增订单表数据
            db.order.Add(pp);
            //保存数据
            db.SaveChanges();
            //新增订单详情
            foreach (var jObject in array)
            {
                order_details ppd = new order_details();
                //赋值属性
                ppd.order_id = "X" + orderrid;
                ppd.product_id = int.Parse(jObject["product_id"].ToString());
                ppd.quantity = int.Parse(jObject["product_details_num"].ToString());
                ppd.order_details_money = 41700;
                   //decimal.Parse((int.Parse(jObject["product_details_num"].ToString())
                   // * int.Parse(jObject["pro_price"].ToString())).ToString());
                //再新增详情表数据
                db.order_details.Add(ppd);
                //保存数据
                db.SaveChanges();
            }
            return 0;
        }

        //查询所有客户名
        [HttpGet]
        public string add_name()
        {
            var info = from p in db.customer
                       select new { customer_name = p.customer_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }
    }
}
