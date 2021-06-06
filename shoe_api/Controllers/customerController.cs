using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class customerController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        [HttpGet]
        //客户信息查询
        public string cut()
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            //==================================================
            var info = db.customer.ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }

        public string odr() 
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;
            //==================================================
            var info1 = db.order.ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info1);
            return json;
        }
    }
}
