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

    [RoutePrefix("Buy_Manages")]//路由
    public class Buy_ManageController : ApiController
    {
        //模型类
        ShoeEntities db = new ShoeEntities();

        //接口一、查询采购计划
        [Route("PostAllData_jihua")]
        [HttpPost]
        public BaseDataTables Select_jihua([FromBody] GetDataTablesMessage obj)
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
                var list1 = db.xp_SelectPageJihua(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageJihua(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageJihua(-1, -1, info).ToList().Count();
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

        //接口二、查询采购里面的详情按钮（包含：当前订单对应所购买的原材料）
        [HttpPost]
        public BaseDataTables Select_jihua_detail([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageJihua_detail(obj.start, obj.length, obj.detail_id.ToString()).ToList();

                //查询数据表总共有多少条记录
                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageJihua_detail(-1, -1, obj.detail_id.ToString()).ToList().Count();

                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;

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


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Shan(int id)
        {
            //先查然后删除
            var matp = db.materials_plan.FirstOrDefault(m => m.materialr_plan_id == id);
            if (matp!=null)
            {
                db.materials_plan.Remove(matp);
            }
            var del = db.SaveChanges();
            return del;
        }
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Shans([FromUri] dynamic ids)
        {
            string str = Convert.ToString(ids);
            //先查然后删除
            List<string> userIdList = str.Split(',').ToList();
            //循环修改id涉及到用户的状态
            foreach (string item in userIdList)
            {
                materials_plan mapl = new materials_plan() { materialr_plan_id = Convert.ToInt32(item) };
                db.Entry(mapl).State = System.Data.Entity.EntityState.Deleted;
            }
            var del = db.SaveChanges();
            return del;
        }


        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public materials_plan Huo(int id)
        {
            return db.materials_plan.Find(id);
        }


        //查询所有材料名字，用来显示新增材料的下拉框内
        [HttpGet]
        public string Materials()
        {
            var info = from m in db.materialr
                       select new { materialr_details_name = m.materialr_details_name };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //查询所有材料规格，用来显示新增材料规格的下拉框内
        [HttpPost]
        public string pro_guige([FromBody] materialr ma)
        {
            var info = from m in db.materialr
                       where (m.materialr_details_name.Contains(ma.materialr_details_name))
                       select new { pro_guige = m.pro_guige };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //根据材料名查询产品信息
        [HttpPost]
        public string mat_name_info([FromBody] materialr ma)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var info = from m in db.materialr
                       where (m.materialr_details_name.Contains(ma.materialr_details_name) && m.pro_guige == ma.pro_guige)
                       select m;
            int id = info.Count();
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }


        //新增采购计划
        [HttpPost]
        public int add_plan(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增采购计划表
            materials_plan mp = new materials_plan();
            mp.operator_per = json1.Root["operator_per"].ToString();
            mp.person_handling = json1.Root["person_handling"].ToString();
            mp.materials_plan_time = ((DateTime)json1.Root["materials_plan_time"]);
            mp.status = json1.Root["status"].ToString();
            //首先新增计划表数据
            db.materials_plan.Add(mp);
            //保存数据
            db.SaveChanges();

            //查询计划ID
            
            var list = db.materials_plan.OrderByDescending(x => x.materialr_plan_id).FirstOrDefault();
            //新增计划详情
            foreach (var jObject in array)
            {
                materials_plan_details mpd = new materials_plan_details();
                //赋值属性

                mpd.materialr_plan_id = list.materialr_plan_id;
                mpd.materialr_details_num = int.Parse(jObject["materialr_details_num"].ToString());
                mpd.materialr_details_id = int.Parse(jObject["materialr_details_id"].ToString());
                //再新增详情表数据
                db.materials_plan_details.Add(mpd);
                //保存数据
                db.SaveChanges();
            }           
            return 0;
        }

        ////接口三、查询采购订单
        [Route("PostAllData_caigou_order")]
        [HttpPost]
        public BaseDataTables Select_Caigou([FromBody] GetDataTablesMessage obj)
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
                var list1 = db.xp_SelectPageCaigou(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageCaigou(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageCaigou(-1, -1, info).ToList().Count();
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

        //查询采购里面的详情按钮（包含：当前订单对应所购买的原材料）
        [HttpPost]
        public BaseDataTables Select_Caigou_Detail([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageCaigou_Detail(obj.start, obj.length, obj.detail_ids.ToString()).ToList();

                //查询数据表总共有多少条记录
                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageCaigou_Detail(-1, -1, obj.detail_ids.ToString()).ToList().Count();

                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;

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

        //查询订单编号，用来显示在新增计划的那个下拉框里(未处理的订单号)
        [HttpGet]
        public string weichuli_order()
        {
            var info = from p in db.materials_plan
                       where (p.status == "未处理")
                       select new { materialr_plan_id = p.materialr_plan_id };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        //查询采购订单详情
        [HttpGet]
        public string Select_jihua_details(string materialr_plan_id)
        {
            var list = db.xp_SelectPageJihua_detail(-1,-1, materialr_plan_id);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        //新增采购订单
        [HttpPost]
        public int add_Order_plan(string json)
        {
            JObject json1 = (JObject)JsonConvert.DeserializeObject(json);
            JArray array = (JArray)json1["infoList"];
            //先新增采购订单表
            materials_order mp = new materials_order();

            //获取当前时间戳作为订单号
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            long times = ((long)timeSpan.TotalMilliseconds/1000);//采购订单ID
            mp.materials_order_id="X"+times.ToString();

            mp.operator_per = json1.Root["operator_per"].ToString();
            mp.person_handling = json1.Root["person_handling"].ToString();
            mp.materialr_plan_id = (int)(json1.Root["materialr_plan_id"]);
            mp.status = json1.Root["status"].ToString();
            //首先新增计划表数据
            db.materials_order.Add(mp);
            //保存数据
            db.SaveChanges();


            //新增计划详情
            foreach (var jObject in array)
            {
                buy_materials_details mpd = new buy_materials_details();
                //赋值属性

                mpd.materialrs_order_id = "X"+ times.ToString();
                mpd.materialr_details_num = int.Parse(jObject["materialr_details_num"].ToString());
                mpd.materialr_details_id = int.Parse(jObject["materialr_details_id"].ToString());
                //再新增详情表数据
                db.buy_materials_details.Add(mpd);
                //保存数据
                db.SaveChanges();
            }

            //修改为materialr_plan_id的状态信息
            int materialr_plan_ids = (int)json1.Root["materialr_plan_id"];
            var mpl = db.materials_plan.Where(x => x.materialr_plan_id== materialr_plan_ids).FirstOrDefault();
            mpl.status = "已处理";
            db.Entry(mpl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return 0;
        }


    }
}
