using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{
    [RoutePrefix("Materials_plan")]
    public class Materials_planController : ApiController
    {

        ShoeEntities db = new ShoeEntities();

        [Route("POSTAllData")]
        [HttpPost]
        public BaseDataTables FenYe([FromBody] GetDataTablesMessage obj)
        {
            //防止序列化恶性循环===========================
            db.Configuration.ProxyCreationEnabled = false;

            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();

            Pagedata.draw = obj.draw;

            //根据对应页码和条数进行查询
            var list1 = db.materials_plan.ToList();

            //查询数据表总共有多少条记录
            int rows1 = db.materials_plan.ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //if (obj.search.value != null)
            //{
            //    rows2 = db.materials_plan.Where(a => a.name == obj.search.value).ToList().Count;
            //    list1 = db.materials_plan.ToList();
            //}

            /// <summary>
            /// 即没有过滤的记录数（数据库里总共记录数）
            /// </summary>
            Pagedata.recordsTotal = rows1;

            /// <summary>
            /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
            /// </summary>
            Pagedata.recordsFiltered = rows2;

            Pagedata.data = list1;

            return Pagedata;
        }
    }
}
