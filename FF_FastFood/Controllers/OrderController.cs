using FF_Fastfood.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FF_Fastfood.Controllers
{
    public class OrderController : Controller
    {
        private FF_FastFoodEntities dbContext = new FF_FastFoodEntities();
        public ActionResult PlaceOrder()
        {
            var cartId = GetCartIdForCurrentUser();

            // Fetch the cart items
            var cartItems = dbContext.Cart_Items
                .Where(c => c.cart_id == cartId)
                .Include(c => c.Food) // Assuming you have a navigation property to Food
                .ToList();

            // Create a ViewModel to pass the cart items and customer information to the view
            var viewModel = new PlaceOrderViewModel
            {
                CartItems = cartItems,
                Customer = dbContext.Customers.FirstOrDefault(c => c.customer_id == dbContext.Carts.FirstOrDefault(cart => cart.cart_id == cartId).customer_id)
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult PlaceOrder(PlaceOrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var cartId = GetCartIdForCurrentUser();
                var cartItems = dbContext.Cart_Items.Where(c => c.cart_id == cartId).ToList();
                var totalAmount = cartItems.Sum(item => item.Food.price * item.quantity);

                var order = new Order
                {
                    customer_id = dbContext.Carts.FirstOrDefault(cart => cart.cart_id == cartId).customer_id,
                    order_date = DateTime.Now,
                    total_amount = totalAmount,
                    status = "Pending"
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                foreach (var item in cartItems)
                {
                    var orderItem = new Order_Items
                    {
                        order_id = order.order_id,
                        food_id = item.food_id,
                        quantity = item.quantity,
                        price = item.Food.price
                    };
                    dbContext.Order_Items.Add(orderItem);
                }
                // Create payment record
                var payment = new Payment
                {
                    order_id = order.order_id,
                    payment_method = orderViewModel.PaymentMethod,
                    payment_status = "Pending",
                    amount = order.total_amount,
                    created_at = DateTime.Now
                };
                dbContext.Payments.Add(payment);
                dbContext.SaveChanges();

                // Clear the cart
                dbContext.Cart_Items.RemoveRange(cartItems);
                dbContext.SaveChanges();

                // Kiểm tra phương thức thanh toán và chuyển hướng
                switch (orderViewModel.PaymentMethod)
                {
                    case "Thanh Toán Qua Ví Điện Tử":
                        return RedirectToAction("PayWithMomo", new { orderId = order.order_id, amount = totalAmount });
                    case "VNPay":
                        return RedirectToAction("PayWithVNPay", new { orderId = order.order_id, amount = totalAmount });
                    default:
                        return RedirectToAction("OrderSuccess", new { id = order.order_id });
                }

                //return RedirectToAction("OrderSuccess", new { id = order.order_id });
            }

            // If we got this far, something failed, redisplay form
            return View(orderViewModel);
        }


        public ActionResult OrderSuccess(int id)
        {
            var order = dbContext.Orders.Include(o => o.Order_Items.Select(oi => oi.Food))
                                        .FirstOrDefault(o => o.order_id == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }


        private Cart GetCart()
        {
            int cartId = GetCartIdForCurrentUser();
            return dbContext.Carts.Include("Cart_Items.Food").FirstOrDefault(c => c.cart_id == cartId);
        }

        private int GetCartIdForCurrentUser()
        {
            var userId = Session["UserID"];
            if (userId == null)
            {
                throw new InvalidOperationException("User is not logged in, please login and try again.");
            }

            var customerId = dbContext.Customers
                .Where(c => c.account_id.ToString() == userId.ToString())
                .Select(c => c.customer_id)
                .FirstOrDefault();

            if (customerId == 0)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            var cartId = dbContext.Carts
                .Where(c => c.customer_id == customerId)
                .Select(c => c.cart_id)
                .FirstOrDefault();

            if (cartId == 0)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            return cartId;
        }

        //Thanh Toán MOMO
        [HttpGet]
        public async Task<ActionResult> PayWithMomo(int orderId, decimal amount)
        {
            try
            {
                // Lấy thông tin cấu hình từ web.config
                string endpoint = WebConfigurationManager.AppSettings["MoMo:Endpoint"];
                string partnerCode = WebConfigurationManager.AppSettings["MoMo:PartnerCode"];
                string accessKey = WebConfigurationManager.AppSettings["MoMo:AccessKey"];
                string secretKey = WebConfigurationManager.AppSettings["MoMo:SecretKey"];
                string redirectUrl = WebConfigurationManager.AppSettings["MoMo:RedirectUrl"];
                string ipnUrl = WebConfigurationManager.AppSettings["MoMo:IpnUrl"];
                string orderInfo = "Thanh toán đơn hàng #" + orderId;
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                // Tạo signature cho yêu cầu thanh toán
                string rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType=payWithATM";


                string signature = GenerateSignature(secretKey, rawHash);

                // Dữ liệu gửi đi cho MoMo
                var momoPayment = new
                {
                    partnerCode = partnerCode,
                    partnerName = "MoMo",
                    storeId = "MomoTestStore",
                    requestId = requestId,
                    amount = amount,
                    orderId = orderId,
                    orderInfo = orderInfo,
                    redirectUrl = redirectUrl,
                    ipnUrl = ipnUrl,
                    lang = "vi",
                    extraData = extraData,
                    requestType = "payWithATM",
                    signature = signature
                };

                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(momoPayment);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(endpoint, content);
                    var responseString = await response.Content.ReadAsStringAsync();


                    var momoResponse = JsonConvert.DeserializeObject<MomoResponse>(responseString);

                    if (!string.IsNullOrEmpty(momoResponse.payUrl))
                    {
                        // Redirect đến MoMo để thanh toán qua ATM
                        return Redirect(momoResponse.payUrl);
                    }
                    else
                    {
                        return View("Error", new HandleErrorInfo(new Exception("Không thể tạo yêu cầu thanh toán với MoMo"), "Order", "PayWithMomo"));
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Order", "PayWithMomo"));
            }
        }

        private string GenerateSignature(string secretKey, string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public class MomoResponse
        {
            public string payUrl { get; set; }
            public string errorCode { get; set; }
        }

        //Thanh Toan VNPay

    }
}