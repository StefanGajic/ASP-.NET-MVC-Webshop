using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using zTest2.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace zTest2.Controllers
{

    public class AccountController : Controller
    {
  
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            string username = model.UserName;
            string password = model.Password;

            zTest2DBEntities db = new zTest2DBEntities();

            List<TblUser> allUsers = db.TblUsers.ToList();

            foreach (var user in allUsers)
            {
                if (user.UserName.Equals(username) && user.Password.Equals(password))
                {
                  
                    Session["user"] = user; 

                    Session.Timeout = 60;

                    return RedirectToAction("Index", "Home"); 
                }
            }

            return View(model); 

           
        }


        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                zTest2DBEntities db = new zTest2DBEntities();

                TblUser newUser = new TblUser();

                newUser.Email = model.Email;
                newUser.FirstName = model.Name;
                newUser.LastName = model.LastName;
                newUser.Phone = model.Phone;
                newUser.Password = model.Password;
                newUser.UserName = model.UserName;

                db.TblUsers.Add(newUser);
                db.SaveChanges();

                Session["user"] = (db.TblUsers.Select(x => x).OrderByDescending(x => x.UserId).Take(1)).ToList()[0];

                Session["welcome msg"] = "Hello " + newUser.FirstName + "!";

                return RedirectToAction("Index", "Manage");
         
            }

            return View(model);
        }
  
        public ActionResult AdminRights()
        {
            List<TblUser> model = new List<TblUser>();

            zTest2DBEntities db = new zTest2DBEntities();

            model = db.TblUsers.ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeAdminRights()
        {
            int userId = int.Parse(Request["userId"]);

            zTest2DBEntities db = new zTest2DBEntities();

            db.TblUsers.Find(userId).Role = db.TblUsers.Find(userId).Role ? false : true;

            db.SaveChanges();

            return RedirectToAction("AdminRights", "Account");
        }

        
        public ActionResult LogOff()
        {
            Session["user"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            Session["user"] = null;
            Session.Abandon();
            
            return RedirectToAction("Index", "Home");
        }

    }
}