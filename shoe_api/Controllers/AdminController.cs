using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using shoe_api.Models;
using System.Web.Http;

namespace shoe_api.Controllers
{
    [RoutePrefix("UserTypeGet")]
    public class AdminController : ApiController
    {

        /// <summary>
        /// GetDataTablesMessage类是对于前端，接收DataTables回传数据model
        /// </summary>
        /// 
        /// /// <summary>
        /// BaseDataTables类是对于后端，返回DataTables所需要的值
        /// </summary>
        ShoeEntities db = new ShoeEntities();
        [Route("GetAllData")]
        [HttpPost]
        public BaseDataTables FenYe1([FromBody] GetDataTablesMessage obj)
        {

            BaseDataTables Pagedata = new BaseDataTables();

            Pagedata.draw = obj.draw;

            //根据对应页码和条数进行查询
           var list1 = db.xp_adminPage(obj.length,obj.start,"").ToList();

            //查询数据表总共有多少条记录
            int rows1 = db.admin.ToList().Count;

            //记录过滤后的条数
            int rows2 = rows1;
            //var list=new List<admin>();
            if (obj.search.value != null)
            {
                rows2 = db.admin.Where(a => a.name == obj.search.value).ToList().Count;
                list1 = db.xp_adminPage(obj.length, obj.start , obj.search.value).ToList();
            }

            /// <summary>
            /// 即没有过滤的记录数（数据库里总共记录数）
            /// </summary>
            Pagedata.recordsTotal = rows1;

            /// <summary>
            /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
            /// </summary>
            Pagedata.recordsFiltered = rows2;

            //Pagedata.data = list1;

            return Pagedata;
        }
    }
}
