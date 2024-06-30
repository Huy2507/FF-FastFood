using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class ShipperAccount
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
       
        public bool isActive { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string vehicle_info { get; set; }

        [Required]
        public string status { get; set; }
    }
}