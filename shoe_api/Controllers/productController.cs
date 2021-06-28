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
                        join od in db.product_plan_details
             on pp.product_plan_id equals od.product_plan_id
                        select new
                        {
                            product_plan_id = pp.product_plan_id,
                            order_id = od.order_id,
                            operator_per = pp.operator_per,
                            product_time = pp.product_time,
                            product_end_time = pp.product_end_time,
                            status = pp.status
                        } into q
                        where (q.product_plan_id.ToString().Contains(info))
                        select q;
            //查询数据表总共有多少条记录
            int rows1 = db.product_plan.ToList().Count;
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
        //查询订单编号，用来显示在新增计划的那个下拉框里
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
        public int add_plan([FromBody] product_plan pp)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (pp.order_details_id.ToString() == null)
            {
                return 0;
            }
            if (
              pp.operator_per == null ||
              pp.product_time == null ||
              pp.product_end_time == null)
            {
                return 1;
            }
            //if (pp.product_plan_num.ToString() == null ||
            //  pp.operator_per == null ||
            //  pp.product_time == null ||
            //  pp.product_end_time == null)
            //{
            //    return 1;
            //}
            //新增数据
            db.product_plan.Add(pp);
            //保存数据
            db.SaveChanges();
            return 2;
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
                    get_materials_id = pp.get_materials_id,
                    product_plan_id = pp.product_plan_id,
                    get_department = pp.get_department,
                    operator_per = pp.operator_per,
                    status = pp.status,
                    get_time = pp.get_time
                } into p
                where (p.get_materials_id.ToString().Contains(info))
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
    }
}
