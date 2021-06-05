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
                           product_plan_id = pp.product_plan_id ,
                           order_id = od.order_id,
                           product_plan_num =pp.product_plan_num,
                           operator_per=pp.operator_per,
                           product_time=pp.product_time,
                           product_end_time=pp.product_end_time,
                       };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
    }
}
