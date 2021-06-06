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
        public string adm()
        {
            var info = db.admin.ToList();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return json;
        }
    }
}
