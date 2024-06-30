using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FF_Fastfood.Models;
using FF_Fastfood.ClassForAccount;
using FF_Fastfood.Models.ChartClass;
using System.Web.Helpers;
using PagedList;
using ClosedXML.Excel;
using System.IO;
using System.Web.ModelBinding;
using FF_Fastfood.Models.Filter;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using FF_Fastfood.ClassForOrders;

namespace FF_Fastfood.Controllers
{
    public class AdminController : Controller
    {
        FF_FastFoodEntities1 db = new FF_FastFoodEntities1();

        [HttpGet]
        public ActionResult Index(string search, int? type, int page = 1)
        {

            /*Phiên đầu tiên, tất cả tham số đều null, phiên thứ hai khi có 1 tham số khác 
             null sẽ tạo Session, nếu tham số thay đổi ở những phiên sau, cập nhập lại phiên.       
            */
            FilterType filter = new FilterType(type, search);
            if (Session["filterFood"] != null)
            {

                filter = Session["filterFood"] as FilterType;
                if (type != null || search != null)
                {
                    filter = new FilterType(type, search);
                    Session["filterFood"] = filter;
                }
            }
            else if (filter.type != null || filter.search != null)
            {
                Session["filterFood"] = filter;
            }
            page = page < 1 ? 1 : page;
            List<Food> foodd = db.Foods.ToList();
            int pagesize = 5;
            if (filter.type != null)
            {
                foodd = filter.filterListFoodType(foodd);
            }
            if (filter.search != null)
            {
                foodd = filter.filterListFoodSearch(foodd);
            }
            ViewBag.search = filter.search;
            if (filter.type != null && filter.type != 0)
            {
                ViewBag.type = filter.type;
                FF_Fastfood.Models.Category category = db.Categories.FirstOrDefault(x => x.category_id == filter.type);
                ViewBag.typeName = category.category_name;
            }
            else if (filter.type == 0)
            {
                ViewBag.type = 0;
                ViewBag.typeName = "--All type--";
            }

            return View(foodd.ToPagedList(page, pagesize));
        }
        public String addSelecFood(int? id)
        {
            List<FF_Fastfood.Models.Category> category = db.Categories.ToList();
            String html = "";
            if (category == null)
            {
                html = "<option value='0' selected>--All type--</option>";
            }
            else
            {
                foreach (FF_Fastfood.Models.Category item in category)
                {
                    if(item.category_id==id)
                    {
                        html += "<option value='" + item.category_id + "' selected>" + item.category_name + "</option>";
                    }
                    else
                    {
                        html += "<option value='" + item.category_id + "'>" + item.category_name + "</option>";
                    }
                    
                }
            }
            return html;
        }
        public ActionResult MenuDetails(string id)
        {

            FindMaxID a = new FindMaxID();
            ViewBag.Id = a.MaxId();
            ViewBag.TimeNow = DateTime.Now;
            if (id != null)
            {
                int Dish_ID = int.Parse(id);
                Food foodDetails = db.Foods.Where(row => row.food_id == Dish_ID).FirstOrDefault();
                return View(foodDetails);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddFood(Food food)
        {
            db.Foods.Add(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateFood(Food food)
        {
            Food a = db.Foods.Where(row => row.food_id == food.food_id).FirstOrDefault();

            a.name = food.name;
            a.description = food.description;
            a.price = food.price;
            a.category_id = food.category_id;
            if(food.image_url.Contains(@"\images\Food\"))
            {
                a.image_url = food.image_url;
            }
            else
            {
                a.image_url = @"\images\Food\" + food.image_url;
            }          
            a.seo_title = food.seo_title;
            a.description = food.seo_description;
            a.seo_keywords = food.seo_keywords;
            a.slug = food.slug;
            a.updated_at = food.updated_at;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteFood(int id)
        {
            var a = db.Foods.FirstOrDefault(c => c.food_id == id);

            db.Foods.Remove(a);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //BÊN DƯỚI LÀ ACCOUNT***********************************************

        static List<Account_TK> lst = new List<Account_TK>();
        [HttpGet]
        public ActionResult Account(FilterAccount filter, int page = 1)
        {

            lst = (from sp in db.Accounts
                   where sp.isActive != true
                   select (new Account_TK
                   {
                       account_id = sp.account_id,
                       username = sp.username,
                       password = sp.password,
                       role = sp.role,
                       created_at = sp.created_at,
                       updated_at = sp.updated_at,
                       isActive = sp.isActive
                   })).ToList();
            List<Customer> custom = db.Customers.ToList();
            for (int i = 0; i < custom.Count; i++)
            {
                if (lst.Where(x => x.account_id == custom[i].account_id).FirstOrDefault() != null)
                {
                    lst.Where(x => x.account_id == custom[i].account_id).FirstOrDefault().name = custom[i].name;
                }
            }
            List<Employee> employ = db.Employees.ToList();
            for (int i = 0; i < employ.Count; i++)
            {
                if (lst.Where(x => x.account_id == employ[i].account_id).FirstOrDefault() != null)
                {
                    lst.Where(x => x.account_id == employ[i].account_id).FirstOrDefault().name = employ[i].name;
                }
            }
            List<Deliverer> deliverers = db.Deliverers.ToList();
            for (int i = 0; i < deliverers.Count; i++)
            {
                if (lst.Where(x => x.account_id == deliverers[i].account_id).FirstOrDefault() != null)
                {
                    lst.Where(x => x.account_id == deliverers[i].account_id).FirstOrDefault().name = deliverers[i].name;
                }
            }
            if (filter != null)
            {
                if (filter.account_id != null && filter.account_id != 0)
                {
                    lst = lst.Where(x => x.account_id == filter.account_id).ToList();
                }
            }
            if (filter != null)
            {
                if (filter.isActive == true)
                {
                    lst = lst.Where(x => x.isActive == filter.isActive).ToList();
                }
            }
            if (filter != null)
            {
                if (filter.role != null)
                {
                    if (filter.role == "none")
                    {
                        lst = lst.ToList();

                    }
                    else if (filter.role.Equals("customer") || filter.role.Equals("shipper"))
                    {
                        lst = lst.Where(x => x.role == filter.role).ToList();
                    }
                    else
                    {
                        lst = lst.Where(x => x.role != "customer" && x.role != "shipper").ToList();
                    }

                }
            }


            int pagesize = 10;
            var content = lst.ToPagedList(page, pagesize);
            Session["filter"] = filter;
            return View(content);
        }


        public String addSelecRole(String acc)
        {

            String html = "";
            if (acc == null)
            {
                html = "<option value='none'slected>--All type--</option>";
                html += "<option value='employee'>--employee--</option>";
                html += "<option value='shipper'>--delivery--</option>";
                html += "<option value='customer'>--customer--</option>";
            }
            else
            {

                html = "<option value='none'>--All type--</option>";
                if (acc.Equals("customer"))
                {
                    html += "<option value='customer' selected>--customer--</option>";
                    html += "<option value='employee'>--employee--</option>";
                    html += "<option value='shipper'>--delivery--</option>";
                }
                else if (acc.Equals("deliverer"))
                {
                    html += "<option value='customer' >--customer--</option>";
                    html += "<option value='employee'>--employee--</option>";
                    html += "<option value='shipper'selected>--delivery--</option>";
                }
                else if (acc.Equals("none"))
                {
                    html = "<option value='none'slected>--All type--</option>";
                    html += "<option value='employee'>--employee--</option>";
                    html += "<option value='shipper'>--delivery--</option>";
                    html += "<option value='customer'>--customer--</option>";
                }
                else
                {
                    html += "<option value='customer' >--customer--</option>";
                    html += "<option value='employee'selected>--employee--</option>";
                    html += "<option value='shipper'>--delivery--</option>";
                }
            }
            return html;
        }
        public String addSelecStatus(bool? status)
        {

            String html = "";
            if (status == null)
            {
                html = "<option value='false' selected>--Active--</option>";
            }
            else
            {
                if (status == false)
                {
                    html = "<option value='false' selected>--Active--</option>";
                    html += "<option value='true'>--InActive--</option>";
                }
                else
                {

                    html = "<option value='false' >--Active--</option>";
                    html += "<option value='true'selected>--InActive--</option>";
                }
            }
            return html;
        }
        public String checkValue(bool val)
        {
            if (val == true)
            {
                return "Banned";
            }
            return "Active";
        }
        public ActionResult AccountDetail(string id, string role)
        {
            int userID = int.Parse(id);
            StaffViewModel staffView = new StaffViewModel();
            Account accounts = db.Accounts.FirstOrDefault(x => x.account_id == userID);
            staffView.acc = accounts;
            switch (role)
            {
                case "staff":
                    Employee employstaff = db.Employees.FirstOrDefault(x => x.account_id == userID);
                    staffView.employe = employstaff;
                    return View(staffView);
                case "chef":
                    Employee employchef = db.Employees.FirstOrDefault(x => x.account_id == userID);
                    staffView.employe = employchef;
                    return View(staffView);
                case "customer":
                    Customer cusstomer = db.Customers.FirstOrDefault(x => x.account_id == userID);
                    staffView.custommer = cusstomer;
                    return View(staffView);
                case "shipper":
                    Deliverer deliveries = db.Deliverers.FirstOrDefault(x => x.account_id == userID);
                    staffView.shipper = deliveries;
                    return View(staffView);
                default:
                    return RedirectToAction("Account");
            }


        }


        public ActionResult CreateAccShipper()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CreateAccShipper(ShipperAccount ac)
        {
            FindMaxID findMax = new FindMaxID();
            Account account = new Account();
            account.username = ac.username;
            account.password = ac.password;
            account.role = "shipper";
            account.isActive = ac.isActive;
            account.updated_at = DateTime.Now;
            account.created_at = DateTime.Now;
            Deliverer shipper = new Deliverer();
            shipper.account_id = findMax.AccountMaxID() + 1;
            shipper.name = ac.name;
            shipper.phone = ac.phone;
            shipper.email = ac.email;
            shipper.vehicle_info = ac.vehicle_info;
            shipper.updated_at = DateTime.Now;
            shipper.created_at = DateTime.Now;
            db.Accounts.Add(account);
            db.Deliverers.Add(shipper);
            db.SaveChanges();
            return RedirectToAction("Account");
        }


        public ActionResult CreateAccEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccEmployee(EmployeeAccount ac)
        {
            FindMaxID findMax = new FindMaxID();


            Account account = new Account();

            account.username = ac.username;
            account.password = ac.password;
            account.role = ac.role;
            account.isActive = ac.isActive;
            account.updated_at = DateTime.Now;
            account.created_at = DateTime.Now;
            Employee employee = new Employee();
            employee.account_id = findMax.AccountMaxID() + 1;
            employee.name = ac.name;
            employee.position = ac.position;
            employee.email = ac.email;
            employee.phone = ac.phone;
            employee.created_at = DateTime.Now;
            employee.updated_at = DateTime.Now;
            employee.hire_date = DateTime.Now;
            db.Accounts.Add(account);
            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Account");
        }


        public ActionResult AddAccount(int? id)
        {
            if (id == 1)
            {

                return RedirectToAction("CreateAccEmployee");
            }
            else if (id == 2)
            {
                return RedirectToAction("CreateAccShipper");
            }
            else
            {
                return View("Account");
            }

        }
        public ActionResult ShipperAccountDelete(int? id)
        {
            Account account = db.Accounts.FirstOrDefault(x => x.account_id == id);
            Deliverer deliverer = db.Deliverers.FirstOrDefault(x => x.account_id == id);

            db.Deliverers.Remove(deliverer);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Account");
        }
        public ActionResult EmployeeAccountDelete(int? id)
        {
            Account account = db.Accounts.FirstOrDefault(x => x.account_id == id);
            Employee employee = db.Employees.FirstOrDefault(x => x.account_id == id);
            db.Employees.Remove(employee);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Account");
        }




        public ActionResult AccountUpdateEmployee(Employee employ)
        {
            Employee empl = db.Employees.FirstOrDefault(x => x.employee_id == employ.employee_id);
            if (empl != null)
            {
                empl.name = employ.name;
                empl.position = employ.position;
                empl.phone = employ.phone;
                empl.email = employ.email;
                empl.updated_at = employ.updated_at;
                db.SaveChanges();
                return RedirectToAction("Accounts");
            }
            else
            {
                db.Employees.Add(employ);
                db.SaveChanges();
                return RedirectToAction("Accounts");
            }

        }
        public ActionResult AccountUpdateCustomer(Customer custom)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.customer_id == custom.customer_id);
            customer.name = custom.name;
            customer.phone = custom.phone;
            customer.Addresses = custom.Addresses;
            customer.email = custom.email;
            customer.updated_at = custom.updated_at;
            db.SaveChanges();
            return RedirectToAction("Accounts");
        }
        public ActionResult AccountUpdateShipper()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountUpdateShipper(Deliverer shipper)
        {

            Deliverer shiper = db.Deliverers.FirstOrDefault(x => x.deliverer_id == shipper.deliverer_id);
            if (shiper != null)
            {
                shiper.name = shipper.name;
                shiper.phone = shipper.phone;
                shiper.email = shipper.email;
                shiper.vehicle_info = shipper.vehicle_info;
                shiper.status = shipper.status;
                shiper.updated_at = shipper.updated_at;
                db.SaveChanges();
                return RedirectToAction("Accounts");
            }
            else
            {
                db.Deliverers.Add(shipper);
                db.SaveChanges();
                return RedirectToAction("Accounts");
            }
        }

        /*Bên dưới là phần Orders********************/
        static List<FindUserByCustomId> ordercustom = new List<FindUserByCustomId>();
        public ActionResult Orders(FilterOrder filter, int page = 1)
        {
            int pagesize = 10;
            List<FindUserByCustomId> ordercustom = new List<FindUserByCustomId>();
            List<Order> oder = db.Orders.ToList();
            List<Customer> customer = db.Customers.ToList();
            for (int i = 0; i < oder.Count; i++)
            {
                if (customer.Where(x => x.customer_id == oder[i].customer_id) != null)
                {
                    var item = customer.Where(x => x.customer_id == oder[i].customer_id).FirstOrDefault();

                    if (item != null)
                    {
                        var orderCustomItem = new FindUserByCustomId
                        {
                            name = item.name,
                            order_id = oder[i].order_id,
                            order_date = oder[i].order_date,
                            total_amount = oder[i].total_amount,
                            status = oder[i].status
                        };

                        ordercustom.Add(orderCustomItem);
                    }
                }
            }
            if(Session["filterOrders"]!=null && filter.order_date== DateTime.MinValue && filter.status==null && filter.customerName==null)
            {
                filter = Session["filterOrders"] as FilterOrder;
            }
            if (filter != null)
            {
                if (filter.customerName != null)
                {
                    ordercustom = ordercustom.Where(x => x.name.Contains(filter.customerName)).ToList();
                }
                if (filter.order_date != null && filter.order_date != DateTime.MinValue)
                {
                    ordercustom = ordercustom.Where(x => x.order_date.Value.ToString("MM/dd/yyyy") == filter.order_date.ToString("MM/dd/yyyy")).ToList();
                }
                if (filter.status != null)
                {
                    if (filter.status == "All")
                    {
                        ordercustom = ordercustom.ToList();
                    }
                    else
                    {
                        ordercustom = ordercustom.Where(x => x.status == filter.status).ToList();
                    }

                }
            }
            var content = ordercustom.ToPagedList(page, pagesize);
            Session["filterOrders"] = filter;
            return View(content);
        }
        public IHtmlString SelectOrderStatus(string sttus)
        {
            string html = "";
            if(sttus==null)
            {
                html = "<option value='All'  selected>--All--</option>";
                html += "<option value='Pending'>--Pending--</option>";
                html += "<option value='Waiting'>--Waiting--</option>";
                html += "<option value='Done'>--Done--</option>";
            }
            else { 
            if (sttus == "Pending")
            {
                html = "<option value='All'>--All--</option>";
                html += "<option value='Pending' selected>--Pending--</option>";
                html += "<option value='Waiting'>--Waiting--</option>";
                html += "<option value='Done'>--Done--</option>";
            }
            else if (sttus == "Waiting")
            {
                html = "<option value='All'>--All--</option>";
                html += "<option value='Pending'>--Pending--</option>";
                html += "<option value='Waiting' selected>--Waiting--</option>";
                html += "<option value='Done'>--Done--</option>";
            }
            else if (sttus == "Done")
            {
                html = "<option value='All'>--All--</option>";
                html += "<option value='Pending'>--Pending--</option>";
                html += "<option value='Waiting'>--Waiting--</option>";
                html += "<option value='Done' selected>--Done--</option>";
            }
            else
            {
                html = "<option value='All' selected>--All--</option>";
                html += "<option value='Pending'>--Pending--</option>";
                html += "<option value='Waiting'>--Waiting--</option>";
                html += "<option value='Done'>--Done--</option>";
            }
            }
            return new MvcHtmlString(html);
        }

        public ActionResult OrdersDetail(int? id)
        {
            ViewBag.orderID = id;
            ContentForOrder contentForOrder = new ContentForOrder();
            return View(contentForOrder.findFoodByIDs(id));
        }

        [HttpGet]
        public ActionResult ExportExcel(int? id)
        {
            ContentForOrder contentForOrder = new ContentForOrder();
            List<FindFoodByID> foodByID = contentForOrder.findFoodByIDs(id);
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Order Detail");


            // First Names
            ws.Cell("A1").Value = "Order Detail ID";
            ws.Cell("B1").Value = "Order ID";
            ws.Cell("C1").Value = "Food Name";
            ws.Cell("D1").Value = "Quanlity";
            ws.Cell("E1").Value = "Price";
            int row = 2;
            // Last Names
            for (int i = 0; i < foodByID.Count; i++)
            {
                ws.Cell("A" + row).Value = foodByID[i].order_item_id;
                ws.Cell("B" + row).Value = foodByID[i].order_id;
                ws.Cell("C" + row).Value = foodByID[i].name;
                ws.Cell("D" + row).Value = foodByID[i].quantity;
                ws.Cell("E" + row).Value = foodByID[i].price;
                row++;
            }
            /*String nameFile = "Export_" + DateTime.Now.Ticks + ".xlsx";
            String pathExcel = Server.MapPath("~/Resource/ExportExcel/"+ nameFile);
            wb.SaveAs(pathExcel);
            return Json(nameFile, JsonRequestBehavior.AllowGet);*/
            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                // Return the file
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedFile.xlsx");
            }
        }
        //*******************CHART CONTROL
        public ActionResult Chartcontrol(FillChartData filter)
        {
            FillChartData fillChartData = new FillChartData(filter);
            var labelss = fillChartData.FillChart().Select(item => item.order_date).ToArray();

            // Tạo mảng data từ thuộc tính total_amount của danh sách

            var datas = fillChartData.FillChart().Select(item => item.total_amount.ToString()).ToArray();

            
            if(filter!=null)
            {
                
            }
            else
            {
                 labelss = fillChartData.FillChart().Select(item => item.order_date).ToArray();
                // Tạo mảng data từ thuộc tính total_amount của danh sách
                 datas = fillChartData.FillChart().Select(item => item.total_amount.ToString()).ToArray();
            }

            var chartData = new
            {
                labels = labelss,
                datasets = new[]
                    {
                        new
                        {
                            label = "Total Amount",
                            data = datas,
                            borderWidth = 1
                        }
                    }
            };
            Session["filterChart"] = filter;
            return View(chartData);
        }
       
        public String SelectChartType(int? type)
        {
            String html = "";
            if (type == null || type==0)
            {
                html = "<option value='0' selected>Today</option>";
                html += "<option value='1' >Yesterday</option>";
                html += "<option value='2'>Seven days ago</option>";
                html += "<option value='3'>This month</option>";
                html += "<option value='4'>Self select</option>";
            }
            else if (type == 1)
            {
                html = "<option value='0' >Today</option>";
                html += "<option value='1' selected>Yesterday</option>";
                html += "<option value='2'>Seven days ago</option>";
                html += "<option value='3'>This month</option>";
                html += "<option value='4'>Self select</option>";
            }
            else if (type == 2)
            {
                html = "<option value='0' >Today</option>";
                html += "<option value='1' >Yesterday</option>";
                html += "<option value='2' selected>Seven days ago</option>";
                html += "<option value='3'>This month</option>";
                html += "<option value='4'>Self select</option>";
            }
            else if(type==3)
            {
                html = "<option value='0' selected>Today</option>";
                html += "<option value='1' >Yesterday</option>";
                html += "<option value='2'>Seven days ago</option>";
                html += "<option value='3' selected >This month</option>";
                html += "<option value='4'>Self select</option>";
            }
            else if(type == 4)
            {
                html = "<option value='0' selected>Today</option>";
                html += "<option value='1' >Yesterday</option>";
                html += "<option value='2'>Seven days ago</option>";
                html += "<option value='3' >This month</option>";
                html += "<option value='4' selected>Self select</option>";
            }
            return html;
        }

        public ActionResult Delivery()
        {
            return View();
        }
        public ActionResult Delivery_Order_Detail()
        {
            return View();
        }

    }
}