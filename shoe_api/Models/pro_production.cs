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
    
    public partial class pro_production
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public pro_production()
        {
            this.in_repertory = new HashSet<in_repertory>();
            this.product_quality_testing = new HashSet<product_quality_testing>();
        }
    
        public int pro_production_id { get; set; }
        public int product_plan_id { get; set; }
        public string pro_production_dep { get; set; }
        public string operator_per { get; set; }
        public System.DateTime product_time { get; set; }
        public string status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<in_repertory> in_repertory { get; set; }
        public virtual product_plan product_plan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product_quality_testing> product_quality_testing { get; set; }
    }
}
