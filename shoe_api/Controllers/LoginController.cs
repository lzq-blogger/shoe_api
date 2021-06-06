using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    public class LoginController : ApiController
    {

        ShoeEntities DB = new ShoeEntities();
        [HttpGet]
        public string Login()
        {
            if (true)
            {
                return "gdz";
            }
        }

        [HttpPost]
        public string LoginUp(dynamic dy)
        {
            try
            {
                string name = dy.name;
                string pwd = dy.pwd;
                //var ad = DB.admin.ToList();
                var flag = DB.admin.Where(a => a.name == name && a.pwd == pwd).ToList(); 
                if (flag.Count > 0)
                {
                    return "{" + "message" + ":true," + "data" + ":" + Newtonsoft.Json.JsonConvert.SerializeObject(flag) + "}";
                }
                else
                {
                    return "message:" + "false";
                }
            }
            catch (Exception)
            {
                return "message:" + "throw";
            }
           
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(ad);
            //return flag;

            //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ad));
            //return Json(ad, JsonRequestBehavior.AllowGet);
        }

    }
}
