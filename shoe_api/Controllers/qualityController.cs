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

        //查询产品生产
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

            var list1 = db.pro_production.ToList().Where(p => p.pro_production_id.ToString().Contains(info)
            || p.pro_production_dep.Contains(info)
            || p.operator_per.Contains(info) || p.product_time.ToString().Contains(info)
            || p.status.Contains(info));
           
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

            var list1 = db.product_quality_testing.ToList().Where(p => p.quality_testing_id.ToString().Contains(info)
            || p.pro_production_id.ToString().Contains(info) || p.quality_testing_time.ToString().Contains(info)
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
            || p.in_materialr_id.ToString().Contains(info) || p.quality_testing_time.ToString().Contains(info)
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

            var list1 = db.in_materialr.ToList().Where(p => p.in_materialr_id.ToString().Contains(info)
            || p.materialr_plan_id.ToString().Contains(info) 
            || p.operator_per.Contains(info) || p.status.Contains(info));

            //查询数据表总共有多少条记录
            int rows1 = db.in_materialr.ToList().Count;

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
