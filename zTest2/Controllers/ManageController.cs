using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using zTest2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace zTest2.Controllers
{
    public class ManageController : Controller
    {

        public async Task<ActionResult> Index()
        {

            if (Session["welcome msg"] != null)
            {
                ViewBag.Message = Session["welcome msg"];
                Session["welcome msg"] = null;
            }

            return View(Session["user"]);
        }



        public ActionResult ChangePhone()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePhone(ChangePhoneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["user"] as TblUser;

            if (model.OldPhone.Equals(user.Phone))
            {
                user.Phone = model.NewPhone;

                zTest2DBEntities db = new zTest2DBEntities();

                db.TblUsers.Find(user.UserId).Phone = model.NewPhone;

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Manage");
        }



        public ActionResult ChangeLastName()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeLastName(ChangeLastNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["user"] as TblUser;

            if (model.OldLastName.Equals(user.LastName))
            {
                user.LastName = model.NewLastName;

                zTest2DBEntities db = new zTest2DBEntities();

                db.TblUsers.Find(user.UserId).LastName = model.NewLastName;

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Manage");
        }

        public ActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please enter correct information";
                return View(model);
            }

            var user = Session["user"] as TblUser;

            if (model.OldEmail.Equals(user.Email))
            {
                user.Email = model.NewEmail;

                zTest2DBEntities db = new zTest2DBEntities();

                db.TblUsers.Find(user.UserId).Email = model.NewEmail;

                db.SaveChanges();

                Session["welcome msg"] = "You successfully changed your email address!";

                return RedirectToAction("Index", "Manage");
            }
            else
            {
                ViewBag.Message = "Please enter correct information";
                return View(model);
            }
        }


        public ActionResult ChangeFirstName()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangeFirstName(ChangeFirstNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Session["user"] as TblUser;

            if (model.OldFirstName.Equals(user.FirstName))
            {
                user.FirstName = model.NewFirstName;

                zTest2DBEntities db = new zTest2DBEntities();

                db.TblUsers.Find(user.UserId).FirstName = model.NewFirstName;

                db.SaveChanges();
            }

            return RedirectToAction("Index", "Manage");
        }


        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = Session["user"] as TblUser;

            if (model.OldPassword.Equals(user.Password) && model.NewPassword.Equals(model.ConfirmPassword))
            {
                user.Password = model.NewPassword;

                zTest2DBEntities db = new zTest2DBEntities();

                db.TblUsers.Find(user.UserId).Password = model.NewPassword;

                db.SaveChanges();
            }

            return View(model);
        }

        public ActionResult Purchases()
        {
            string datefrom = Request["datefrom"];
            string dateto = Request["dateto"];

            if (datefrom == null || dateto == null)
            {
                zTest2DBEntities db = new zTest2DBEntities();

                var grouped = db.TblReceipts.Select(x => x).GroupBy(x => x.DateAndTime).ToList();

                return View(grouped);
            }
            else
            {
                int fromy = int.Parse(datefrom.Split('-')[0]);
                int fromm = int.Parse(datefrom.Split('-')[1]);
                int fromd = int.Parse(datefrom.Split('-')[2]);

                int toy = int.Parse(dateto.Split('-')[0]);
                int tom = int.Parse(dateto.Split('-')[1]);
                int tod = int.Parse(dateto.Split('-')[2]);

                zTest2DBEntities db = new zTest2DBEntities();

                var grouped = db.TblReceipts.Select(x => x).GroupBy(x => x.DateAndTime).ToList();

                var grouped_new = new List<System.Linq.IGrouping<System.Nullable<System.DateTime>, zTest2.Models.TblReceipt>>();

                foreach (var item in grouped)
                {
                    if (item.Key.Value >= new DateTime(fromy, fromm, fromd) && item.Key.Value <= new DateTime(toy, tom, tod, 23, 59, 59))
                    {
                        grouped_new.Add(item);
                    }
                }

                return View(grouped_new);
            }
        } 



    }
}