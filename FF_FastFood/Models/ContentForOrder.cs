using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
using FF_Fastfood.ClassForAccount;
namespace FF_Fastfood.Models
{
    public class ContentForOrder
    {
        FF_FastFoodEntities1 db = new FF_FastFoodEntities1();
        public List<FindFoodByID> findFoodByIDs(int? id)
        {
            List<FindFoodByID> foodByID = new List<FindFoodByID>();
            List<Order_Items> order_Items = db.Order_Items.Where(x => x.order_id == id).ToList();
            List<Food> foodsd = db.Foods.ToList();
            for (int i = 0; i < order_Items.Count; i++)
            {

                var item = foodsd.Where(x => x.food_id == order_Items[i].food_id).FirstOrDefault();
                if (item != null)
                {
                    FindFoodByID findFoodByID = new FindFoodByID();
                    findFoodByID.name = item.name;
                    findFoodByID.order_item_id = order_Items[i].order_item_id;
                    findFoodByID.order_id = order_Items[i].order_id;
                    findFoodByID.price = order_Items[i].price;
                    findFoodByID.quantity = order_Items[i].quantity;
                    foodByID.Add(findFoodByID);
                }

            }
            return foodByID;
        }
    }
}