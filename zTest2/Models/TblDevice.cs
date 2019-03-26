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
    
    public partial class TblDevice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblDevice()
        {
            this.TblCartItems = new HashSet<TblCartItem>();
            this.TblReceipts = new HashSet<TblReceipt>();
        }
    
        public int DeviceId { get; set; }
        [Display(Name="Device")]
        public string DeviceName { get; set; }
        [Display(Name = "Description")]
        public string DescriptionDevice { get; set; }
        public string Components { get; set; }
        public Nullable<int> Shop { get; set; }
        [Display(Name = "Made In")]
        public string MadeInCountry { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Price { get; set; }
        public string Picture { get; set; }
        public Nullable<int> Category { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual TblCategory TblCategory { get; set; }
        public virtual TblShop TblShop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblCartItem> TblCartItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblReceipt> TblReceipts { get; set; }
    }
}
