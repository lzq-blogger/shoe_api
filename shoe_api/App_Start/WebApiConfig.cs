using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace shoe_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //解决跨域问题，添加引用using System.Web.Http.Cors;，安装包microsoft.aspnet.webapi.cors
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Serialize;   //post请求有参数时加的东西
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                (Newtonsoft.Json.ReferenceLoopHandling)Newtonsoft.Json.PreserveReferencesHandling.Objects;
            // Web API 路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
