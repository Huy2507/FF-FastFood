using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
namespace FF_Fastfood.Models
{
    public class CheckFunction
    {
        FF_FastFoodEntities1 db = new FF_FastFoodEntities1();

        public List<Food> menuType(string search)
        {
            if (search == null)
            {
                return db.Foods.ToList();
            } else
            {
                return db.Foods.Where(row => row.name.Contains(search)).ToList();
            }

        }

        public List<Food> checkType(int? type,List<Food> typeSelect)
        {
            if (type == 0 || type == null)
            {
                return typeSelect.ToList();
            }
            else if(type != 0 && type != null)
            {
                List<Food> ckeckType = typeSelect.Where(row => row.category_id == type).ToList();
                return ckeckType;
            }
            else
            {
                return typeSelect.ToList();
            }
        }
        public List<Food> checkPrice(int? cost, List<Food> priceSelect)
        {
            if (cost == 0)
            {
                return priceSelect.ToList();
            }
            else if (cost == 1)
            {
                return priceSelect.Where(row => row.price< 30000 ).ToList();
            }
            else if (cost == 2)
            {
                return priceSelect.Where(row => row.price > 30000 && row.price < 70000).ToList();
            }
            else if (cost == 3)
            {
                return priceSelect.Where(row =>  row.price > 70000).ToList();
            }
            else
            {
                return priceSelect.ToList();
            }
        }


        public StaffViewModel SearchUser(string search)
        {
           
            if (search != null)
            {
                StaffViewModel staffView = new StaffViewModel();
                List<Employee> employees = db.Employees.Where(x=>x.name.Equals(search)).ToList();
                List<Customer> customers = db.Customers.Where(x => x.name.Equals(search)).ToList();
                List<Deliverer> deliveries = db.Deliverers.Where(x => x.name.Equals(search)).ToList();
                List<Account> accounts = db.Accounts.ToList();

                staffView.LSTaccounts = accounts;
                staffView.LSTcustomers = customers;
                staffView.LSTemployees = employees;
                staffView.LSTshipper = deliveries;
                return staffView;
            }
            else
            {
                StaffViewModel staffView = new StaffViewModel();
                List<Employee> employees = db.Employees.ToList();
                List<Customer> customers = db.Customers.ToList();
                List<Deliverer> deliveries = db.Deliverers.ToList();
                List<Account> accounts = db.Accounts.ToList();

                staffView.LSTaccounts = accounts;
                staffView.LSTcustomers = customers;
                staffView.LSTemployees = employees;
                staffView.LSTshipper = deliveries;
                return staffView;
            }
        }
    }
}