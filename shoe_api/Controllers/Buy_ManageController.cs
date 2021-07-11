﻿using shoe_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace shoe_api.Controllers
{

    [RoutePrefix("Buy_Manages")]//路由
    public class Buy_ManageController : ApiController
    {
        //模型类
        ShoeEntities db = new ShoeEntities();

        //接口一、查询采购计划
        [Route("PostAllData_jihua")]
        [HttpPost]
        public BaseDataTables Select_jihua([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询条件
                string info = "";
                if (obj.search.value != null)
                {
                    info = obj.search.value;
                }
                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageJihua(obj.start, obj.length, info).ToList();

                //查询数据表总共有多少条记录             

                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageJihua(-1, -1, "").ToList().Count();


                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;
                if (obj.search.value != null)
                {
                    rows2 = db.xp_SelectPageJihua(-1, -1, info).ToList().Count();
                }

                ///返回参数
                /// <summary>
                /// 即没有过滤的记录数（数据库里总共记录数）
                /// </summary>
                Pagedata.recordsTotal = rows1;

                /// <summary>
                /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
                /// </summary>
                Pagedata.recordsFiltered = rows2;

                Pagedata.data = list1;
            }
            catch (Exception mes)
            {
                Pagedata.error = mes.ToString();
            }

            return Pagedata;
        }

        [HttpPost]
        public BaseDataTables Select_jihua_detail([FromBody] GetDataTablesMessage obj)
        {
            //新建返回实例对象
            BaseDataTables Pagedata = new BaseDataTables();
            try
            {

                //防止序列化恶性循环===========================
                db.Configuration.ProxyCreationEnabled = false;

                //记录请求次数
                Pagedata.draw = obj.draw;

                //查询数据,数据库分页
                //根据对应页码和条数进行查询               

                //调用存储过程，参数：1、从第几条开始，2、每页记录数，3、查询条件,
                var list1 = db.xp_SelectPageJihua_detail(obj.start, obj.length, obj.detail_id.ToString()).ToList();

                //查询数据表总共有多少条记录
                //调用存储过程，参数：1、从第几条开始(-1为查全部)，2、每页记录数(-1为查全部)，3、查询条件(为''查询全部)
                int rows1 = db.xp_SelectPageJihua_detail(-1, -1, obj.detail_id.ToString()).ToList().Count();

                //记录过滤后的条数(没有则默认为总共记录数)
                int rows2 = rows1;

                ///返回参数
                /// <summary>
                /// 即没有过滤的记录数（数据库里总共记录数）
                /// </summary>
                Pagedata.recordsTotal = rows1;

                /// <summary>
                /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
                /// </summary>
                Pagedata.recordsFiltered = rows2;

                Pagedata.data = list1;
            }
            catch (Exception mes)
            {
                Pagedata.error = mes.ToString();
            }

            return Pagedata;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Shan(int id)
        {
            //先查然后删除
            var matp = db.materials_plan.FirstOrDefault(m => m.materialr_plan_id == id);
            if (matp!=null)
            {
                db.materials_plan.Remove(matp);
            }
            var del = db.SaveChanges();
            return del;
        }
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Shans([FromUri] dynamic ids)
        {
            string str = Convert.ToString(ids);
            //先查然后删除
            List<string> userIdList = str.Split(',').ToList();
            //循环修改id涉及到用户的状态
            foreach (string item in userIdList)
            {
                materials_plan mapl = new materials_plan() { materialr_plan_id = Convert.ToInt32(item) };
                db.Entry(mapl).State = System.Data.Entity.EntityState.Deleted;
            }
            var del = db.SaveChanges();
            return del;
        }


        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public materials_plan Huo(int id)
        {
            return db.materials_plan.Find(id);
        }

        ////接口二、查询采购订单
        //[Route("PostAllData_caigou")]
        //[HttpPost]
        //public BaseDataTables Select_caigou([FromBody] GetDataTablesMessage obj)
        //{
        //    //新建返回实例对象
        //    BaseDataTables Pagedata = new BaseDataTables();
        //    try
        //    {
        //        //防止序列化恶性循环===========================
        //        db.Configuration.ProxyCreationEnabled = false;

        //        //记录请求次数
        //        Pagedata.draw = obj.draw;

        //        //查询条件
        //        string info = "";
        //        if (obj.search.value != null)
        //        {
        //            info = obj.search.value;
        //        }
        //        //根据对应页码和条数进行查询
        //        var list1 = from mp in db.materials_plan
        //                    join m in db.materialr
        //         on mp.materialr_details_id equals m.materialr_details_id 
        //         join im in db.in_materialr on mp.materialr_plan_id equals im.materialr_plan_id
        //                    select new
        //                    {
        //                        materialr_plan_id = mp.materialr_plan_id,
        //                        materialr_details_id = m.materialr_details_id,
        //                        materialr_details_name = m.materialr_details_name,
        //                        materialr_details_price = m.materialr_details_price,
        //                        materialr_num = mp.materialr_num,
        //                        status = mp.status,
        //                    } into q
        //                    where (q.materialr_plan_id.ToString().Contains(info)
        //                           || q.materialr_details_id.ToString().Contains(info)
        //                           || q.materialr_details_name.ToString().Contains(info)
        //                           || q.materialr_details_price.ToString().Contains(info)
        //                           || q.materialr_num.ToString().Contains(info)
        //                           || q.status.ToString().Contains(info)
        //                           )
        //                    select q;

        //        //查询数据表总共有多少条记录
        //        int rows1 = (from mp in db.materials_plan
        //                     join m in db.materialr
        //          on mp.materialr_details_id equals m.materialr_details_id
        //                     select new
        //                     {
        //                         materialr_plan_id = mp.materialr_plan_id,
        //                         materialr_details_id = m.materialr_details_id,
        //                         materialr_details_name = m.materialr_details_name,
        //                         materialr_details_price = m.materialr_details_price,
        //                         materialr_num = mp.materialr_num,
        //                         status = mp.status,
        //                     } into q
        //                     select q).Count();

        //        //记录过滤后的条数(没有则默认为总共记录数)
        //        int rows2 = rows1;
        //        if (obj.search.value != null)
        //        {
        //            rows2 = list1.Count();
        //        }

        //        ///返回参数
        //        /// <summary>
        //        /// 即没有过滤的记录数（数据库里总共记录数）
        //        /// </summary>
        //        Pagedata.recordsTotal = rows1;

        //        /// <summary>
        //        /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
        //        /// </summary>
        //        Pagedata.recordsFiltered = rows2;

        //        Pagedata.data = list1;
        //    }
        //    catch (Exception mes)
        //    {
        //        Pagedata.error = mes.ToString();
        //    }

        //    return Pagedata;
        //}


        //接口三、查询采购里面的详情按钮（包含：当前订单对应所购买的原材料）


    }
}
