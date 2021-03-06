﻿using System;
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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Validation;


namespace zTest2.Controllers
{

    public class AccountController : Controller
    {

        static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        static SHA256 encryptor = SHA256.Create();
        readonly static int SaltLength = 32;


        public static byte[] MakeSalt(int maxLength)
        {
            var Salt = new byte[maxLength];
            random.GetNonZeroBytes(Salt);
            return Salt;
        }

        public static byte[] ComputeHash(string inputString, byte[] salt)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
            var salted = new byte[salt.Length + inputBytes.Length];

            inputBytes.CopyTo(salted, 0);
            salt.CopyTo(salted, inputBytes.Length);

            var hashed = encryptor.ComputeHash(salted);

            return hashed;
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


                newUser.UserName = model.UserName;
                newUser.Email = model.Email;
                newUser.FirstName = model.Name;
                newUser.LastName = model.LastName;
                newUser.Phone = model.Phone;
                var salt = MakeSalt(SaltLength);
                newUser.Salt = salt;
                newUser.HashedPass = ComputeHash(model.Password, salt);
               

                db.TblUsers.Add(newUser);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {

                    Console.WriteLine(e);
                }
               

                Session["user"] = (db.TblUsers.Select(x => x).OrderByDescending(x => x.UserId).Take(1)).ToList()[0];

                Session["welcome msg"] = "Hello " + newUser.FirstName + "!";

                return RedirectToAction("Index", "Manage");

            }

            return View(model);
        }

  
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

                var salt = user.Salt;
                if (salt == null) continue;
                var hash = ComputeHash(password, salt);

                string hashString = System.Text.Encoding.UTF8.GetString(hash);
                string passHash = System.Text.Encoding.UTF8.GetString(user.HashedPass);

                if (string.Equals(hashString, passHash) && user.UserName.Equals(username))
                {
                   
                    Session["user"] = user;

                    Session.Timeout = 60;
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid username or password!");

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