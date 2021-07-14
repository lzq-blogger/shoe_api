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
    public class qualityController : ApiController
    {
        ShoeEntities db = new ShoeEntities();

        //查询待检产品
        [HttpPost]
        public BaseDataTables CpSc([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            var list1 = db.pro_production.ToList().Where(p => (p.status.ToString().Contains(info)
            || p.pro_production_id.ToString().Contains(info) || p.operator_per.Contains(info)) && p.status == "未质检");

            //查询数据表总共有多少条记录
            int rows1 = db.pro_production.ToList().Count;

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

        //查询产品质检单
        [HttpPost]
        public BaseDataTables CpZj([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            var list1 = db.product_quality_testing.Where(p => p.quality_testing_id.ToString().Contains(info)
            || p.operator_per.Contains(info) || p.result.Contains(info));

            //查询数据表总共有多少条记录
            int rows1 = db.product_quality_testing.ToList().Count;

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

        //查询材料质检单
        [HttpPost]
        public BaseDataTables YlZj([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            var list1 = db.materials_quality_testing.ToList().Where(p => p.quality_testing_id.ToString().Contains(info)
            || p.operator_per.Contains(info) || p.result.Contains(info));

            //查询数据表总共有多少条记录
            int rows1 = db.materials_quality_testing.ToList().Count;

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

        //查询原料入库订单
        [HttpPost]
        public BaseDataTables YlRk([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            BaseDataTables Pagedata = new BaseDataTables();
            Pagedata.draw = obj.draw;

            string info = "";
            if (obj.search.value != null)
            {
                info = obj.search.value;
            }

            var list1 = db.materials_order.ToList().Where(p => (p.materials_order_id.ToString().Contains(info)
            || p.operator_per.Contains(info) || p.status.Contains(info) || p.person_handling.Contains(info)) && p.status == "未质检");

            //查询数据表总共有多少条记录
            int rows1 = db.materials_order.ToList().Count;

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
        
        
        [HttpPost]
        //新增产品质检单
        public int addproduct_quality_testing(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //新增客户信息表
           int pro_production_id= int.Parse(json1.Root["pro_production_id"].ToString());
            product_quality_testing pp = new product_quality_testing();
            pp.pro_production_id = pro_production_id;
            pp.quality_testing_time = DateTime.Parse(json1.Root["quality_testing_time"].ToString());
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.result = json1.Root["result"].ToString();
            //首先新增领料单表数据
            db.product_quality_testing.Add(pp);
            //保存数据
            db.SaveChanges();

            //查询生产编号为pro_production_id对应的状态修改
            var pro_obj = db.pro_production.Where(x => x.pro_production_id == pro_production_id).FirstOrDefault();
            pro_obj.status = "已质检";
            db.Entry(pro_obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return 0;
        }
        [HttpPost]
        //新增材料质检单
        public int addselect_product_materials(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //新增客户信息表
            string materialrs_order_id = (json1.Root["materials_order_id"].ToString());
            materials_quality_testing pp = new materials_quality_testing();
            pp.materialrs_order_id = materialrs_order_id;
            pp.quality_testing_time = DateTime.Parse(json1.Root["quality_testing_time"].ToString());
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.result = json1.Root["result"].ToString();
            //首先新增领料单表数据
            db.materials_quality_testing.Add(pp);
            //保存数据
            db.SaveChanges();

            //查询生产编号为pro_production_id对应的状态修改
            var pro_obj = db.materials_order.Where(x => x.materials_order_id == materialrs_order_id).FirstOrDefault();
            pro_obj.status = "已质检";
            db.Entry(pro_obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return 0;
        }
        //待检产品详情
        [HttpGet]
        public string plan_pro_detile(int id)
        {
            var list1 = db.Database.SqlQuery<select_product_pro_plan_Result>("exec select_product_pro_plan " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        //待检材料详情
        [HttpGet]
        public string order_details(int id)
        {
            var list1 = db.Database.SqlQuery<select_materialrs_details_Result>("exec select_materialrs_details " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }

        //材料质检单详情
        //public string zhijianorder_details(int id)
        //{
        //    var list1 = db.Database.SqlQuery<select_materialrs_details_Result>("exec select_materialrs_details " + id).ToList();
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        //}
    }
}
