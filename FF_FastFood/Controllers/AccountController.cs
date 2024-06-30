using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using FF_Fastfood.Models;
using FF_Fastfood.Services;
using FF_Fastfood.Attributes;
using System.Web;

namespace FF_Fastfood.Controllers
{
    public class AccountController : Controller
    {
        private Services.EmailService emailService = new Services.EmailService();
        FF_FastFoodEntities db = new FF_FastFoodEntities();

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AuthenticateUser(model.Username, model.Password);
                if (user != null)
                {
                    var name = "";
                    var cus = db.Customers.FirstOrDefault(a=> a.account_id == user.account_id);
                    // Đăng nhập thành công
                    if(user.role == "customer")
                    {
                        name = cus.name;
                        HttpCookie userCookie = new HttpCookie("UserCookie");
                        userCookie.Values["UserName"] = name;
                        userCookie.Values["UserId"] = user.account_id.ToString();
                        userCookie.Expires = DateTime.Now.AddHours(1); // Cookie hết hạn sau 1 giờ
                        Response.Cookies.Add(userCookie);
                        return RedirectToAction("Index", "Food");
                    }
                    else
                    {
                        name = user.username;
                        HttpCookie userCookie = new HttpCookie("UserCookie");
                        userCookie.Values["UserName"] = name;
                        userCookie.Values["UserId"] = user.account_id.ToString();
                        userCookie.Expires = DateTime.Now.AddHours(1); // Cookie hết hạn sau 1 giờ
                        Response.Cookies.Add(userCookie);
                        return RedirectToAction("PendingOrders", "Chef");
                    }
                    // Thiết lập session hoặc cookie để lưu trữ thông tin đăng nhập
                    
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }


        // GET: Account/SignUp
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: Account/SignUp
        [HttpPost]
        public ActionResult SignUp(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu username đã tồn tại
                if (db.Accounts.Any(a => a.username == model.UserName))
                {
                    ModelState.AddModelError("", "Số điện thoại đã tồn tại.");
                    return View(model);
                }
                var hashedPassword = Crypto.HashPassword(model.Password);
                // Tạo tài khoản mới
                var newAccount = new Account
                {
                    username = model.UserName,
                    password = hashedPassword, // Chú ý: Cần mã hóa mật khẩu trước khi lưu
                    role = "customer",
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                db.Accounts.Add(newAccount);
                db.SaveChanges();

                // Tạo thông tin khách hàng mới
                var newCustomer = new Customer
                {
                    account_id = newAccount.account_id,
                    name = model.Name,
                    phone = model.UserName,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                db.Customers.Add(newCustomer);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        private Account AuthenticateUser(string username, string password)
        {
            // Thực hiện xác thực người dùng, ví dụ: truy vấn từ cơ sở dữ liệu
            var user = db.Accounts.FirstOrDefault(u => u.username == username);
            if (user != null && Crypto.VerifyHashedPassword(user.password, password))
            {
                return user;
            }
            return null;
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            // Xóa session hoặc cookie
            if (Request.Cookies["UserCookie"] != null)
            {
                HttpCookie userCookie = new HttpCookie("UserCookie");
                userCookie.Expires = DateTime.Now.AddDays(-1); // Thiết lập ngày hết hạn trong quá khứ
                Response.Cookies.Add(userCookie);
            }
            return RedirectToAction("Login");
        }

        // GET: Account/ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Customers.SingleOrDefault(u => u.email == model.Email);
                var account = db.Accounts.SingleOrDefault(a=>a.account_id == user.account_id);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email không tồn tại.");
                    return View(model);
                }

                // Tạo mã xác nhận
                var resetCode = new Random().Next(100000, 999999).ToString();
                account.PasswordResetCode = resetCode;
                account.ResetCodeExpiration = DateTime.Now.AddMinutes(2); // mã có hiệu lực trong 10 phút
                db.SaveChanges();

                // Gửi mã xác nhận qua SMS
                emailService.SendResetCodeEmailSMTP(user.email, resetCode);

                // Chuyển hướng đến trang nhập mã xác nhận
                return RedirectToAction("VerifyResetCode", new { email = user.email });
            }

            return View(model);
        }

        // GET: Account/ForgotPasswordConfirmation
        [HttpGet]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: Account/ForgotPasswordConfirmation
        [HttpGet]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyResetCode(string email)
        {
            var model = new VerifyResetCodeViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyResetCode(VerifyResetCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Customers.SingleOrDefault(u => u.email == model.Email);
                var account = db.Accounts.SingleOrDefault(a => a.account_id == user.account_id && a.PasswordResetCode == model.ResetCode && a.ResetCodeExpiration > DateTime.Now);
                if (user == null)
                {
                    ModelState.AddModelError("", "Mã xác nhận không hợp lệ hoặc đã hết hạn.");
                    return View(model);
                }

                // Chuyển hướng đến trang đặt lại mật khẩu
                return RedirectToAction("ResetPassword", new { email = user.email });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Customers.SingleOrDefault(u => u.email == model.Email);
                var account = db.Accounts.SingleOrDefault(a => a.account_id == user.account_id);
                if (user == null)
                {
                    return HttpNotFound("Người dùng không tồn tại.");
                }

                // Mã hóa mật khẩu mới
                account.password = Crypto.HashPassword(model.NewPassword);
                account.PasswordResetCode = null;
                account.ResetCodeExpiration = null;
                db.SaveChanges();

                return RedirectToAction("ResetPasswordConfirmation");
            }

            return View(model);
        }
    }
}
