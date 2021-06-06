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
        [HttpGet]
        public string cut()
        {
            var info = db.customer.ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json.ToString();
        }
    }
}
