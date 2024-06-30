using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
namespace FF_Fastfood.Models
{
    public class FindMaxID
    {
        
        FF_FastFoodEntities1 db = new FF_FastFoodEntities1();
        
        public int MaxId()
        {
            
            return db.Foods.Max(f => f.food_id);
        }

        public int AccountMaxID()
        {
            return db.Accounts.Max(f => f.account_id);
        }

        public int EmployeeMaxID()
        {
            return db.Employees.Max(f => f.employee_id);
        }
        public int ShipperMaxID()
        {
            return db.Deliverers.Max(f => f.deliverer_id);
        }


    }
}