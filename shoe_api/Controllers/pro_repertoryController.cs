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
        [HttpGet]
        //库存查询
        public string select_product()
        {
            var info = from p in db.product
                       join pr in db.pro_repertory on
            p.product_id equals pr.product_id
                       select new
                       {
                           repertory_id = pr.repertory_id,
                           product_name = p.product_name,
                           product_type = p.product_type,
                           product_price = p.product_price,
                           pro_repertory_num = pr.pro_repertory_num
                       };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
        [HttpPost]
        //产品入库单（质检完成）
        //public int in_repertory([FromBody] in_repertory ir)
        //{
        //    //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
        //    //判断非空
        //    if (ir.pro_production_id.ToString() == null)
        //    {
        //        return 0;
        //    }
        //    if (ir.operator_per.ToString() == null ||
        //      ir.in_time == null)
        //    {
        //        return 1;
        //    }
        //    //新增数据
        //    //db.in_repertory.Add(ir);
        //    //保存数据
        //    db.SaveChanges();
        //    return 2;
        //}
        //产品出库单
        [HttpGet]
        //库存查询
        public string select_out_product()
        {
            //防止序列化恶性循环================================
            db.Configuration.ProxyCreationEnabled = false;
            //==================================================
            var info = from p in db.out_repertory select p;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
        //产品出库入库统计
    }
}
