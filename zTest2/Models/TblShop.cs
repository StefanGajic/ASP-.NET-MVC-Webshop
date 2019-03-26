//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zTest2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class TblShop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblShop()
        {
            this.TblDevices = new HashSet<TblDevice>();
        }
    
        public int ShopId { get; set; }
        [Display(Name = "Shop")]
        public string ShopName { get; set; }
        public string Address { get; set; }
        [Display(Name = "Employees")]
        public Nullable<int> NoOfEmployees { get; set; }
        [Display(Name = "Tax Number")]
        public Nullable<int> TaxIdentificationNo { get; set; }
        [Display(Name = "Founding Date")]
        public Nullable<System.DateTime> DateOfFounding { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblDevice> TblDevices { get; set; }
    }
}