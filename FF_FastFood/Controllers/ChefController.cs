using System;
using System.Linq;
using System.Web.Mvc;
using FF_Fastfood.Models; // Đảm bảo rằng bạn đã thêm namespace cho mô hình của bạn.

namespace FF_Fastfood.Controllers
{
    public class ChefController : Controller
    {
        private FF_FastFoodEntities db = new FF_FastFoodEntities();

        public ActionResult PendingOrders()
        {
            var pendingOrders = db.Orders
                                  .Where(o => o.status == "Pending")
                                  .Select(o => new OrdersViewModel
                                  {
                                      OrderId = o.order_id,
                                      OrderDate = (DateTime)o.order_date,
                                      TotalAmount = o.total_amount,
                                      CustomerName = db.Customers.FirstOrDefault(c=>c.customer_id == o.customer_id).name,
                                      Status = o.status
                                  })
                                  .ToList();

            return View(pendingOrders);
        }

        // Action để hiển thị chi tiết đơn hàng
        public ActionResult OrderDetails(int? id)
        {
            var order = db.Orders.Include("Order_Items").FirstOrDefault(o => o.order_id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var model = new OrderDetailViewModel
            {
                Items = order.Order_Items.Select(i => new OrderItemViewModel
                {
                    ProductName = db.Foods.FirstOrDefault(f => f.food_id == i.food_id).name,
                    Quantity = i.quantity,
                    Price = i.price
                }).ToList()
            };

            return PartialView("_OrderDetails", model);
        }
    }
}
