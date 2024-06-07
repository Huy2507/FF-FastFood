using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FF_Fastfood.Models;

namespace FF_Fastfood.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult ListAccount()
        {
            FF_FastFoodEntities db = new FF_FastFoodEntities();
            List<Account> lst = db.Accounts.ToList();
            return View(lst);
        }
    }
}