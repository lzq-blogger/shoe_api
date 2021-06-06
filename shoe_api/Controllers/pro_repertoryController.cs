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
                            pro_repertory_num = pr.pro_repertory_num
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
        [HttpPost]
        //产品入库单（质检完成）
        public int in_repertory([FromBody] in_repertory ir)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (ir.pro_production_id.ToString() == null)
            {
                return 0;
            }
            if (ir.operator_per.ToString() == null ||
              ir.in_time == null)
            {
                return 1;
            }
            //新增数据
            db.in_repertory.Add(ir);
            //保存数据
            db.SaveChanges();
            return 2;
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
        [HttpPost]
        //产品入库单
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
            var list1 = from p in db.in_repertory
                        select p into q
                        where (q.pro_production_id.ToString().Contains(info))
                        select q;
            //查询数据表总共有多少条记录
            int rows1 = db.in_repertory.ToList().Count;
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
        //产品出库入库统计
    }
}
