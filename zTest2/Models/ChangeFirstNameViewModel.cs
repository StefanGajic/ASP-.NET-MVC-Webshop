using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zTest2.Models
{
    public class ChangeFirstNameViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Current first name")]
        public string OldFirstName { get; set; }

        
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "New first name")]
        public string NewFirstName { get; set; }
    }
}