using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class saleController : ApiController
    {
        ShoeEntities1 db = new ShoeEntities1();
        //查询客户订单
        [HttpGet]
        public string customer_order()
        {
            var info = from pp in db.order
                       join od in db.customer
            on pp.customer_id equals od.customer_id
                       select new {
                           orderr_id = pp.orderr_id,
                           customer_name = od.customer_name,
                           order_starttime = pp.order_starttime,
                           order_endtime = pp.order_endtime,
                           order_paid = pp.order_paid,
                           order_unpaid = pp.order_unpaid,
                           order_status = pp.order_status,
                       };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
        [HttpPost]
        //新增出库详情
        public int add_out_repertory([FromBody] out_repertory pp)
        {
            //返回0,1,2，用来前端调用接口的时候判断应该给用户数目提示。
            //判断非空
            if (pp.orderr_id.ToString() == null)
            {
                return 0;
            }
            if (pp.operator_per.ToString() == null ||
                pp.out_time.ToString() == null)
            {
                return 1;
            }
            //新增数据
            db.out_repertory.Add(pp);
            //保存数据
            db.SaveChanges();
            return 2;
        }
    }
}
