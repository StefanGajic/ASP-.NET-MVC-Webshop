using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zTest2.Models
{
    public class ChangePhoneViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Current phone number")]
        public string OldPhone { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long. Please enter a valid phone number. Include country code, for USA its +1 and than number", MinimumLength = 5)]
        [Display(Name = "New phone number")]
        public string NewPhone { get; set; }
    }
}