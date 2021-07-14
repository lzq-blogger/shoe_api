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
    public class pro_repertoryController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        [HttpPost]
        //库存查询
        public BaseDataTables select_product([FromBody] GetDataTablesMessage obj)
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
            var list1 = from p in db.product
                        join pr in db.pro_repertory on
             p.product_id equals pr.product_id
                        select new
                        {
                            repertory_id = pr.repertory_id,
                            product_name = p.product_name,
                            product_type = p.product_type,
                            product_price = p.product_price,
                            pro_repertory_num = pr.pro_repertory_num,
                            pro_guige = p.pro_guige
                        } into q
                        where (q.product_name.Contains(info))
                        select q;
            //查询数据表总共有多少条记录
            int rows1 = db.product.ToList().Count;
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
        //产品出库单
        [HttpPost]
        //产品出库单
        public BaseDataTables select_out_product([FromBody] GetDataTablesMessage obj)
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
            var list1 = from p in db.out_repertory
                        select p into q
                        where (q.orderr_id.Contains(info))
                        select q;
            //查询数据表总共有多少条记录
            int rows1 = db.out_repertory.ToList().Count;
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
        //出库详情
        [HttpGet]
        public string select_out_product_details(int id)
        {
            var list1 = db.Database.SqlQuery<select_out_repertory_Result>("exec select_out_repertory " + id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list1);
        }
        [HttpPost]
        //产品入库
        public BaseDataTables select_in_product([FromBody] GetDataTablesMessage obj)
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
            var list1 = db.Database.SqlQuery<in_repertory_detail_Result>("exec in_repertory_detail").ToList();
            //查询数据表总共有多少条记录
            int rows1 = list1.Count;
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
        //新增产品入库
        [HttpPost]
        public int add_in_repertory(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //先新增计划表
            int ids = int.Parse(json1.Root["pro_pro_id"].ToString());
            in_repertory pp = new in_repertory();
            pp.pro_production_id = ids;
            pp.operator_per = json1.Root["person1"].ToString();
            pp.person_handling = json1.Root["person2"].ToString();
            pp.in_time = json1.Root["time"].ToString();
            //首先新增计划表数据
            db.in_repertory.Add(pp);
            //保存数据
            db.SaveChanges();
            
            //入库完成后删除本条数据
            product_quality_testing o = db.product_quality_testing.FirstOrDefault(p => p.pro_production_id ==ids);
            db.product_quality_testing.Remove(o);
            db.SaveChanges();

            //产品库存里面对应的进行新增
            //先查一下数据
            string name = json1.Root["name"].ToString();
            string guige = json1.Root["guige"].ToString();
            int num = int.Parse(json1.Root["num"].ToString());
            product pps = db.product.FirstOrDefault(p=>p.product_name==name&&p.pro_guige==guige);
            pro_repertory pr = db.pro_repertory.FirstOrDefault(p => p.product_id == pps.product_id);
            if (pr!=null)
            {
                pr.repertory_id = pr.repertory_id;
                pr.pro_repertory_num = pr.pro_repertory_num+num;
                db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                pro_repertory prr = new pro_repertory();
                prr.product_id = pps.product_id;
                prr.pro_repertory_num = num;
                db.pro_repertory.Add(prr);
            }
            db.SaveChanges();
            return 0;
        }
        //查询
        [HttpPost]
        //产品入库单
        public BaseDataTables select_in_product_order([FromBody] GetDataTablesMessage obj)
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
            var list1 = from p in db.in_repertory
                        select p into q
                        where (q.pro_production_id.ToString().Contains(info))
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
    }
}
