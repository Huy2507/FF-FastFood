using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FF_Fastfood
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FoodBySlug",
                url: "thuc-don/{slug}",
                defaults: new { controller = "Food", action = "Index", slug = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Map",
                url: "xem-chi-duong",
                defaults: new { controller = "Map", action = "Index" }
            );

            routes.MapRoute(
                name: "dang-nhap",
                url: "tai-khoan/dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "MyAccount",
                url: "tai-khoan-cua-toi/chinh-sua-thong-tin-tai-khoan",
                defaults: new { controller = "MyAccount", action = "EditProfile" }
            );
            routes.MapRoute(
                name: "MyAccountAddress",
                url: "tai-khoan-cua-toi/dia-chi-cua-toi",
                defaults: new { controller = "MyAccount", action = "YourAddresses" }
            );
            routes.MapRoute(
                name: "MyAccountPwd",
                url: "tai-khoan-cua-toi/thay-doi-mat-khau",
                defaults: new { controller = "MyAccount", action = "ChangePassWord" }
            );
            routes.MapRoute(
                name: "MyAccountOrders",
                url: "tai-khoan-cua-toi/don-hang-cua-toi",
                defaults: new { controller = "MyAccount", action = "Orders" }
            );
            routes.MapRoute(
                name: "cart",
                url: "gio-hang-cua-toi",
                defaults: new { controller = "cart", action = "index" }
            );

            routes.MapRoute(
                name: "placeOrder",
                url: "dat-hang/thong-tin-dat-hang",
                defaults: new { controller = "Order", action = "PlaceOrder", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "OrderSuccess",
                url: "dat-hang/dat-hang-thanh-cong-don-hang-so-{id}",
                defaults: new { controller = "Order", action = "OrderSuccess" , id = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
