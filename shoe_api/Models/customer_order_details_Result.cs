//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace shoe_api.Models
{
    using System;
    
    public partial class customer_order_details_Result
    {
        public int order_details_id { get; set; }
        public int product_id { get; set; }
        public string order_id { get; set; }
        public decimal order_details_money { get; set; }
        public int quantity { get; set; }
        public string orderr_id { get; set; }
        public int customer_id { get; set; }
        public System.DateTime order_starttime { get; set; }
        public Nullable<System.DateTime> order_endtime { get; set; }
        public string person_handling { get; set; }
        public string operator_per { get; set; }
        public decimal order_paid { get; set; }
        public decimal order_unpaid { get; set; }
        public string order_status { get; set; }
        public string order_delivery_way { get; set; }
    }
}
