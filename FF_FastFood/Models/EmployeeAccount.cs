using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class EmployeeAccount
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }

        [Required]
        [RegularExpression("^(chef|staff)$", ErrorMessage = "Role must be 'chef' or 'staff'.")]
        public string role { get; set; }

        public bool isActive { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string position { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must be numeric.")]
        public string phone { get; set; }
        [Required]
        public string email { get; set; }
        public Nullable<System.DateTime> hire_date { get; set; }
    }
}