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
            || c.customer_name.ToString().Contains(info) || c.customer_linkman.ToString().Contains(info));

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
                        where (q.orderr_id.ToString().Contains(info1))
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

        //客户订单详情

        [HttpGet]
        public string order_details(int id)
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
        //查询所有产品名字，用来显示新增产品的下拉框内
        [HttpGet]
        public string pro_plan_name()
        {
            var info = from p in db.product
                       select new { product_name = p.product_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
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
        //新增生产计划
        [HttpPost]
        public int add_plan(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增计划表
            product_plan pp = new product_plan();
            pp.order_id = json1.Root["order_id"].ToString();
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.product_time = ((DateTime)json1.Root["product_time"]);
            pp.pro_status = json1.Root["status"].ToString();
            pp.product_end_time = (DateTime)json1.Root["product_end_time"];
            //首先新增计划表数据
            db.product_plan.Add(pp);
            //保存数据
            db.SaveChanges();
            //查询计划ID
            string q = json1.Root["order_id"].ToString();
            List<product_plan> list = db.product_plan.Where(p => p.order_id == q).ToList();
            //新增计划详情
            foreach (var jObject in array)
            {
                product_plan_details ppd = new product_plan_details();
                //赋值属性
                string s = list[0].product_plan_id.ToString();
                ppd.product_plan_id = int.Parse(s);
                ppd.product_details_num = int.Parse(jObject["product_details_num"].ToString());
                ppd.product_id = int.Parse(jObject["product_id"].ToString());
                //再新增详情表数据
                db.product_plan_details.Add(ppd);
                //保存数据
                db.SaveChanges();
            }
            //修改订单的处理状态
            order o = db.order.FirstOrDefault(p => p.orderr_id == q);
            o.orderr_id = q;
            o.order_status = "处理中";
            db.Entry(o).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return 0;
        }
    }
}
