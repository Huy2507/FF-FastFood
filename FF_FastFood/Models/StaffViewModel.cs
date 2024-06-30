using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
namespace FF_Fastfood.Models
{
    public class StaffViewModel
    {
        public List<Employee> LSTemployees { get; set; }
        public List<Customer> LSTcustomers { get; set; }
        public List<Deliverer> LSTshipper { get; set; }
        public List<Account> LSTaccounts { get; set; }

        public Employee employe { get; set; }
        public Customer custommer{ get; set; }
        public Deliverer shipper { get; set; }
        public Account acc { get; set; }

    }



    /*public class AddAccountDeliveryViewModel
    {
        public int Account_id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string vehicle_info { get; set; }
        public string status { get; set; }
    }*/
}