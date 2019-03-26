using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zTest2.Models;

namespace zTest2.Controllers
{
    public class ShoppingCartController : Controller
    {
        zTest2DBEntities storeDB = new zTest2DBEntities();

        public ActionResult ShowCart()
        {
            TblCart model = null;

            foreach(TblCart cart in storeDB.TblCarts)
            {
                if (cart.UserId == (Session["user"] as TblUser).UserId)
                {
                    model = cart;
                    break;
                }
            }

            return View(model);
        }

        [HttpPost]
        public string Add()
        {
            int id = int.Parse(Request["id"]);
            int amount = int.Parse(Request["amount"]);
            
            TblCart usersCart = null;
            
            foreach (TblCart cart in storeDB.TblCarts)
            {
                if (cart.UserId == (Session["user"] as TblUser).UserId)
                {
                    usersCart = cart;
                    break;
                }
            }

            if (usersCart == null)
            {
                usersCart = new TblCart();
                usersCart.UserId = (Session["user"] as TblUser).UserId;
                storeDB.TblCarts.Add(usersCart);
                storeDB.SaveChanges();
            }

            TblCartItem usersCartItem = null;

            foreach (TblCartItem item in usersCart.TblCartItems)
            {
                if (item.DeviceId == id)
                {
                    usersCartItem = item;
                    break;
                }
            }

            if (usersCartItem == null)
            {
                usersCartItem = new TblCartItem();
                usersCartItem.DeviceId = id;
                usersCartItem.Amount = amount;
                usersCartItem.CartId = usersCart.CartId;

                usersCart.TblCartItems.Add(usersCartItem);

                storeDB.SaveChanges();
            }
            else
            {
                usersCartItem.Amount += amount;
                storeDB.SaveChanges();
            }

            string response_amount = "";

            foreach (TblDevice device in storeDB.TblDevices)
            {
                if (device.DeviceId == id)
                {
                    device.Quantity -= amount;
                    response_amount = "" + device.Quantity;
                    break;
                }
            }

            storeDB.SaveChanges();

            return response_amount;
        }

        [HttpPost]
        public string Remove()
        {
            int cartid = int.Parse(Request["cart_id"]);
            int deviceid = int.Parse(Request["device_id"]);
            int amount = int.Parse(Request["amount"]);

            TblCartItem usersItem = null;

            foreach(TblCartItem item in storeDB.TblCartItems)
            {
                if (item.CartId == cartid && item.DeviceId == deviceid)
                {
                    usersItem = item;
                    break;
                }
            }

            string response_amount = "";

            if (usersItem.Amount > amount)
            {
                usersItem.Amount -= amount;
                response_amount = "" + usersItem.Amount;
                storeDB.SaveChanges();
            }
            else
            {
                storeDB.TblCarts.Find(cartid).TblCartItems.Remove(usersItem);
                storeDB.TblCartItems.Remove(usersItem);
                storeDB.SaveChanges();

                response_amount = "0";
            }

            storeDB.TblDevices.Find(deviceid).Quantity += amount;
            storeDB.SaveChanges();

            return response_amount;
        }

        public ActionResult Purchase()
        {
            TblCart userCart = null;

            foreach(TblCart cart in storeDB.TblCarts)
            {
                if (cart.UserId == (Session["user"] as TblUser).UserId)
                {
                    userCart = cart;
                    break;
                }
            }

            DateTime purchaseDate = DateTime.Now;

            List<TblReceipt> model = new List<TblReceipt>();

            foreach(TblCartItem cartItem in userCart.TblCartItems)
            {
                TblReceipt receiptItem = new TblReceipt();

                receiptItem.Amount = cartItem.Amount;
                receiptItem.DateAndTime = purchaseDate;
                receiptItem.DeviceId = cartItem.DeviceId;
                receiptItem.UserId = (Session["user"] as TblUser).UserId;
                receiptItem.Tax = 20;
                receiptItem.PriceWithTax = (int)(int.Parse(storeDB.TblDevices.Find(cartItem.DeviceId).Price) * cartItem.Amount * 1.20);

                storeDB.TblReceipts.Add(receiptItem);
                model.Add(receiptItem);
            }

            var items = userCart.TblCartItems;

            userCart.TblCartItems.Clear();

            foreach (TblCartItem cartItem in items)
            {
                storeDB.TblCartItems.Remove(cartItem);
            }

            storeDB.SaveChanges();

            return View(model);
        }

    }
}