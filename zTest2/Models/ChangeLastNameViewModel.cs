using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zTest2.Models
{
    public class ChangeLastNameViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Current last name")]
        public string OldLastName { get; set; }

        
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "New last name")]
        public string NewLastName { get; set; }
    }
}