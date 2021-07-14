using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class indexController : ApiController
    {
        ShoeEntities db = new ShoeEntities();
        //统计总销售量
        [HttpGet]
        public string ss()
        {
            return "";
        }
    }
}
