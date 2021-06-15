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

        ShoeEntities db = new ShoeEntities();
        [HttpGet]
        public string Login()
        {
            if (true)
            {
                return "gdz";
            }
        }

        [HttpPost]
        [Route("api/Login/LoginUp")]
        public object LoginUp(dynamic dy)
        {
            try
            {
                string account = dy.account;
                string pwd = dy.pwd;
                //var ad = DB.admin.ToList();
                var datas = DB.admin.Where(a => a.account == account && a.pwd == pwd).ToList();
                if (datas.Count > 0)
                {
                    return "{" + "\"" + "message" + "\"" + ":" + "\"" + "true" + "\"," + "\"" + "data" + "\"" + ":" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + "}";
                }
                else
                {
                    return "{" + "\"" + "message" + "\"" + ":" + "\"" + "账号或密码错误" + "\"" + "}";
                }
            }
            catch (Exception)
            {
                return "{" + "\"" + "message" + "\"" + ":" + "\"" + "服务器出现错误！请重试！" + "\"" + "}";
            }

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(ad);
            //return flag;

            //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ad));
            //return Json(ad, JsonRequestBehavior.AllowGet);
        }

    }
}
