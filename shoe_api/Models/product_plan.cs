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
    
    public partial class product_plan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product_plan()
        {
            this.get_materials = new HashSet<get_materials>();
            this.pro_production = new HashSet<pro_production>();
            this.pro_repertory = new HashSet<pro_repertory>();
        }
    
        public int product_plan_id { get; set; }
        public int product_plan_num { get; set; }
        public int order_details_id { get; set; }
        public string operator_per { get; set; }
        public System.DateTime product_time { get; set; }
        public System.DateTime product_end_time { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<get_materials> get_materials { get; set; }
        public virtual order_details order_details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pro_production> pro_production { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pro_repertory> pro_repertory { get; set; }
    }
}
