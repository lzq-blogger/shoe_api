using shoe_api.Models;
using System;
using System.Collections.Generic;
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
        [HttpGet]
        public string pro_plan()
        {
            var info = from pp in db.product_plan
                       join od in db.order_details
            on pp.order_details_id equals od.order_details_id
                       select new {
                           product_plan_id = pp.product_plan_id,
                           order_id = od.order_id,
                           product_plan_num = pp.product_plan_num,
                           operator_per = pp.operator_per,
                           product_time = pp.product_time,
                           product_end_time = pp.product_end_time,
                       };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
        //查询订单编号，用来显示在新增计划的那个下拉框里
        //生产计划查询
        [HttpGet]
        public string pro_plan_order()
        {
            var info = from pp in db.order
                       select new
                       {
                           order_id = pp.orderr_id
                       };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
        //新增生产计划
        [HttpPost]
        public int add_plan([FromBody] product_plan pp)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (pp.order_details_id.ToString() == null)
            {
                return 0;
            }
            if (pp.product_plan_num.ToString() == null ||
              pp.operator_per == null ||
              pp.product_time == null ||
              pp.product_end_time == null)
            {
                return 1;
            }
            //新增数据
            db.product_plan.Add(pp);
            //保存数据
            db.SaveChanges();
            return 2;
        }
        [HttpPost]
        //新增领料单
        public int add_get_materials([FromBody] get_materials gm)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (gm.product_plan_id.ToString() == null)
            {
                return 0;
            }
            if (gm.get_department.ToString() == null ||
              gm.operator_per == null ||
              gm.status == null ||
              gm.get_time == null)
            {
                return 1;
            }
            //新增数据
            db.get_materials.Add(gm);
            //保存数据
            db.SaveChanges();
            return 2;
        }
        [HttpPost]
        //添加材料
        public int add_materials([FromBody] materialr_details md)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (md.w_materials_id.ToString() == null)
            {
                return 0;
            }
            if (md.materialr_epertory_id.ToString() == null)
            {
                return 1;
            }
            //新增数据
            db.materialr_details.Add(md);
            //保存数据
            db.SaveChanges();
            return 2;
        }
        [HttpPost]
        //产品登记表
        public int add_product([FromBody] pro_production pp)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (pp.product_plan_id.ToString() == null)
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
    }
}
