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
    using System.Collections.Generic;
    
    public partial class product_plan_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product_plan_details()
        {
            this.pro_production = new HashSet<pro_production>();
        }
    
        public int product_plan_details_id { get; set; }
        public int product_plan_id { get; set; }
        public int product_details_num { get; set; }
        public int product_id { get; set; }
        public string pro_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pro_production> pro_production { get; set; }
        public virtual product product { get; set; }
        public virtual product_plan product_plan { get; set; }
    }
}
