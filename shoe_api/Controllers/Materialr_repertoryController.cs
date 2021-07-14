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
    [RoutePrefix("Materialr_repertory")]//路由
    public class Materialr_repertoryController : ApiController
    {
        ShoeEntities db = new ShoeEntities();


        //材料库存
        [Route("POSTAllDataKC")]
        [HttpPost]
        public BaseDataTables YuanLiaokc([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询条件
                string info = "";
                if (obj.search.value != null)
                {
                    info = obj.search.value;
                }
                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageCaiLiao(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageCaiLiao(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageCaiLiao(-1, -1, info).ToList().Count();
                }

                ///返回参数
                /// <summary>
                /// 即没有过滤的记录数（数据库里总共记录数）
                /// </summary>
                Pagedata.recordsTotal = rows1;

                /// <summary>
                /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
                /// </summary>
                Pagedata.recordsFiltered = rows2;

                Pagedata.data = list1;
            }
            catch (Exception mes)
            {
                Pagedata.error = mes.ToString();
            }

            return Pagedata;
        }

        //获取材料类型显示在下拉框
        [HttpGet]
        public string Materialr_Type()
        {
            var info = from m in db.materialr_type
                       select new { materialr_type_name = m.materialr_type_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //获取产品类型显示在下拉框（新增材料类型需要）
        [HttpGet]
        public string Product_Type()
        {
            var info = from m in db.product_type
                       select new { product_type_name = m.product_type_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //新增材料类型,重新查询返回数据
        [HttpGet]
        public string add_materialr_type(string materialr_type_name, string product_type_name)
        {
            //根据名称查询材料id
            var pt = db.product_type.Where(p => p.product_type_name == product_type_name).FirstOrDefault();

            //创建对象,新增
            materialr_type mt = new materialr_type();
            mt.materialr_type_name = materialr_type_name;
            mt.product_type_id = pt.product_type_id;
            db.materialr_type.Add(mt);
            db.SaveChanges();

            //查询最新数据返回
            var info = from m in db.materialr_type
                       select new { materialr_type_name = m.materialr_type_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }


        //新增材料信息
        [HttpPost]
        public string add_materialr(string json)
        {
            //序列化
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            //先根据材料类型名获取，材料id
            string materialr_type_name = json1.Root["materialr_type_name"].ToString();
            var mtype = db.materialr_type.Where(mt => mt.materialr_type_name == materialr_type_name).FirstOrDefault();
            //首先新增材料信息
            materialr materialr = new materialr();
            materialr.materialr_details_name = json1.Root["materialr_details_name"].ToString();
            materialr.materialr_type_id = mtype.materialr_type_id;
            materialr.materialr_details_price = (decimal)json1.Root["materialr_details_price"];
            materialr.material_supplier = json1.Root["material_supplier"].ToString();
            materialr.pro_guige = json1.Root["pro_guige"].ToString();
            db.materialr.Add(materialr);
            db.SaveChanges();

            //查询最新的材料
            var mtss = db.materialr.OrderByDescending(x => x.materialr_details_id).FirstOrDefault();
            //再新增库存信息
            materialr_epertory mep = new materialr_epertory();
            mep.materialr_details_id = mtss.materialr_details_id;
            mep.materialr_num= (int)json1.Root["materialr_num"];
            db.materialr_epertory.Add(mep);
            db.SaveChanges();


            return "trues";
        }

        //材料入库详情

        [HttpPost]
        public BaseDataTables select_in_materialr([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询条件
                string info = "";
                if (obj.search.value != null)
                {
                    info = obj.search.value;
                }
                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageRuku(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageRuku(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageRuku(-1, -1, info).ToList().Count();
                }

                ///返回参数
                /// <summary>
                /// 即没有过滤的记录数（数据库里总共记录数）
                /// </summary>
                Pagedata.recordsTotal = rows1;

                /// <summary>
                /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
                /// </summary>
                Pagedata.recordsFiltered = rows2;

                Pagedata.data = list1;
            }
            catch (Exception mes)
            {
                Pagedata.error = mes.ToString();
            }

            return Pagedata;
        }

        //查询订单编号
        [HttpGet]
        public string orderid(string ids)
        {
            var info = from p in db.materials_order
                       where (p.materials_order_id==ids)
                       select new { materialr_plan_id = p.materialr_plan_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //新增库存（材料入库）
        [HttpPost]
        public string add_materialr_epertory(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增材料入库详情
            string ids = json1.Root["materials_order__id"].ToString();
            in_materialr pp = new in_materialr();
            pp.materials_order__id = ids;
            pp.operator_per = json1.Root["operator_per"].ToString();
            pp.person_handling = json1.Root["person_handling"].ToString();
            pp.in_time = json1.Root["time"].ToString();
            //首先新增材料入库详情
            db.in_materialr.Add(pp);
            //保存数据
            db.SaveChanges();

            //入库完成后删除本条数据
            materials_quality_testing o = db.materials_quality_testing.FirstOrDefault(p => p.materialrs_order_id == ids);
            db.materials_quality_testing.Remove(o);
            db.SaveChanges();

            //先循环查找是否存在此库存，无则新增，有则修改

            foreach (var item in array)
            {
                int materid = int.Parse(item["materialr_details_id"].ToString());
                int mater_num = int.Parse(item["materialr_details_num"].ToString());
                //条件成立说明存在此库存，否则就新增库存
                if (db.materialr_epertory.Where(m=>m.materialr_details_id== materid).ToList().Count()>0)
                {
                    var me = db.materialr_epertory.Where(m => m.materialr_details_id == materid).FirstOrDefault();
                    me.materialr_num = me.materialr_num + mater_num;
                    db.Entry(me).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    materialr_epertory me = new materialr_epertory();
                    me.materialr_details_id = materid;
                    me.materialr_num = mater_num;
                    db.materialr_epertory.Add(me);
                    db.SaveChanges();
                }
            }

            return "0";
        }

        //查询
        [HttpPost]
        //材料入库单
        public BaseDataTables select_in_materialr_order([FromBody] GetDataTablesMessage obj)
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
            var list1 = from p in db.in_materialr
                        select p into q
                        where (q.in_materialr_id.ToString().Contains(info))
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



        //材料出库详情
        [HttpPost]
        public BaseDataTables select_out_material_details([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询条件
                string info = "";
                if (obj.search.value != null)
                {
                    info = obj.search.value;
                }
                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageChuku(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageChuku(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageChuku(-1, -1, info).ToList().Count();
                }

                ///返回参数
                /// <summary>
                /// 即没有过滤的记录数（数据库里总共记录数）
                /// </summary>
                Pagedata.recordsTotal = rows1;

                /// <summary>
                /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
                /// </summary>
                Pagedata.recordsFiltered = rows2;

                Pagedata.data = list1;
            }
            catch (Exception mes)
            {
                Pagedata.error = mes.ToString();
            }

            return Pagedata;
        }
    }
}
