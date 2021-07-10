using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class productController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        //生产计划查询
        [HttpPost]
        public BaseDataTables pro_plan([FromBody] GetDataTablesMessage obj)
        {
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;
            string info = "";
            if (obj.search.value!=null)
            {
                info = obj.search.value;
            }
            //根据对应页码和条数进行查询
            var list1 = from pp in db.product_plan
                        select new
                        {
                            product_plan_id = pp.product_plan_id,
                            order_id = pp.order_id,
                            operator_per = pp.operator_per,
                            product_time = pp.product_time,
                            product_status=pp.pro_status,
                            product_end_time = pp.product_end_time,

                        } into q
                        where (q.product_plan_id.ToString().Contains(info))
                        select q;
            //查询数据表总共有多少条记录
            int rows1 = list1.ToList().Count;
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
        //查询生产计划详情存储过程
        [HttpGet]
        public string pro_plan_detail(int id)
        {
            var list1 = db.Database.SqlQuery<pro_plan_details_Result>("exec pro_plan_details " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //查询订单编号，用来显示在新增计划的那个下拉框里(未处理的订单号)
        [HttpGet]
        public string weichuli_order()
        {
            var info = from p in db.order
                       where(p.order_status== "未处理")
                       select new { order_id = p.orderr_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
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
                       where(p.product_name.Contains(pp.product_name))
                       select new { pro_guige = p.pro_guige };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //根据产品名查询产品信息
        [HttpPost]
        public string pro_name_info([FromBody]product pp)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var info = from p in db.product
                       where (p.product_name.Contains(pp.product_name)&&p.pro_guige==pp.pro_guige)
                       select p;
            int id = info.Count();
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }
        //生产计划查询
        [HttpPost]
        public BaseDataTables pro_plan_order([FromBody] GetDataTablesMessage obj)
        {
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;
            //根据对应页码和条数进行查询
            var list1 = from pp in db.order
                        select new
                        {
                            order_id = pp.orderr_id
                        };
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
            List<product_plan> list = db.product_plan.Where(p => p.order_id ==q).ToList();
            //新增计划详情
            foreach (var jObject in array)
            {
                product_plan_details ppd = new product_plan_details();
                //赋值属性
                string s = list[0].product_plan_id.ToString();
                ppd.product_plan_id = int.Parse(s);
                ppd.product_details_num=int.Parse(jObject["product_details_num"].ToString());
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
        //查询领料单
        [HttpPost]
        public BaseDataTables select_get_materials([FromBody] GetDataTablesMessage obj)
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
            var list1 = from pp in db.get_materials
                select new
                {
                    w_materials_id = pp.w_materials_id,
                    product_plan_id = pp.product_plan_id,
                    get_department = pp.get_department,
                    operator_per = pp.operator_per,
                    status = pp.status,
                    get_time = pp.get_time
                } into p
                where (p.w_materials_id.ToString().Contains(info))
                select p;
          //查询数据表总共有多少条记录
          int rows1 = db.get_materials.ToList().Count;
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
        //查询领料单详情存储过程
        [HttpGet]
        public string select_get_materials_detail(int id)
        {
            var list1 = db.Database.SqlQuery<materialr_details_materialr_Result>("exec materialr_details_materialr " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //获取未处理的生产计划ID
        [HttpGet]
        public string weichuli_planid()
        {
            var info = from p in db.product_plan
                       where (p.pro_status == "未处理")
                       select new { product_plan_id = p.product_plan_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //查询所有原料名字，用来显示新增原料的下拉框内
        [HttpGet]
        public string materials_name()
        {
            var info = from p in db.materialr
                       select new { materialr_details_name = p.materialr_details_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //查询所有材料规格，用来显示新增材料规格的下拉框内
        [HttpPost]
        public string materials_guige([FromBody] materialr m)
        {
            var info = from mm in db.materialr
                       where (mm.materialr_details_name.Contains(m.materialr_details_name))
                       select new { pro_guige = mm.pro_guige };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //根据材料名查询材料信息
        [HttpPost]
        public string materials_name_info([FromBody] materialr m)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var info = from mm in db.materialr
                       join mt in db.materialr_type on
                       mm.materialr_type_id equals mt.materialr_type_id
                       where (mm.materialr_details_name.Contains(m.materialr_details_name) && mm.pro_guige == m.pro_guige)
                       select new {
                           materialr_details_id = mm.materialr_details_id,
                           materialr_details_name = mm.materialr_details_name,
                           materialr_type_name = mt.materialr_type_name,
                           materialr_details_price = mm.materialr_details_price,
                           material_supplier = mm.material_supplier,
                           pro_guige = mm.pro_guige,
                       };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }
        //查询生产单
        [HttpPost]
        public BaseDataTables select_pro_product([FromBody] GetDataTablesMessage obj)
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
            var list1 = from pp in db.pro_production
                        select pp;
            //查询数据表总共有多少条记录
            int rows1 = db.pro_production.ToList().Count;
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
        [HttpPost]
        //新增领料单
        public int add_get_materials(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增领料单表
            get_materials pp = new get_materials();
            pp.product_plan_id = int.Parse(json1.Root["product_plan_id"].ToString());
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.get_department = json1.Root["get_department"].ToString();
            pp.status = json1.Root["status"].ToString();
            pp.get_time = (DateTime)json1.Root["get_time"];
            //首先新增领料单表数据
            db.get_materials.Add(pp);
            //保存数据
            db.SaveChanges();
            //查询计划ID
            int q = int.Parse(json1.Root["product_plan_id"].ToString());
            List<get_materials> list = db.get_materials.Where(p => p.product_plan_id == q).ToList();
            //新增计划详情
            foreach (var jObject in array)
            {
                materialr_details ppd = new materialr_details();
                //赋值属性
                int s = int.Parse(list[0].w_materials_id.ToString());
                ppd.w_materials_id = s;
                ppd.materialr_details_num = int.Parse(jObject["materialr_num"].ToString());
                ppd.materialr_details_id = int.Parse(jObject["materialr_details_id"].ToString());
                //再新增详情表数据
                db.materialr_details.Add(ppd);
                //保存数据
                db.SaveChanges();
            }
            //修改订单的处理状态
            product_plan o = db.product_plan.FirstOrDefault(p => p.product_plan_id == q);
            o.product_plan_id = q;
            o.pro_status = "处理中";
            db.Entry(o).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return 0;
        }
        [HttpPost]
        //产品登记表
        public int add_product([FromBody] pro_production pp)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (pp.product_plan_details_id.ToString() == null)
            {
                return 0;
            }
         
            if (pp.pro_production_dep.ToString() == null||
                pp.operator_per.ToString() == null ||
                pp.product_time.ToString() == null )
            {
                return 1;
            }
            //新增数据
            db.pro_production.Add(pp);
            //保存数据
            db.SaveChanges();
            return 2;
        }
        [HttpGet]
        //查询生产计划详情
        public string select_pro_plan_details(int id)
        {
            var list1 = db.Database.SqlQuery<select_pro_plan_details_Result>("exec select_pro_plan_details " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //查询处理中的生产计划ID
        [HttpGet]
        public string select_pro_plan_details_id()
        {
            var list1 = from ppd in db.product_plan
                        where (ppd.pro_status.Contains("处理中"))
                        select new { product_plan_details_id=ppd.product_plan_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
    }
}
