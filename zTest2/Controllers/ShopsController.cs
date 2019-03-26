using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using zTest2.Models;

namespace zTest2.Controllers
{
   
    public class ShopsController : Controller
    {
        private zTest2DBEntities db = new zTest2DBEntities();

        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                if ((Session["user"] as zTest2.Models.TblUser).Role)
                {

                    return View(db.TblShops.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Devices");
                }
            }
            else
            {
                return RedirectToAction("Index", "Devices");
            }
        }
        

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblShop tblShop = db.TblShops.Find(id);
            if (tblShop == null)
            {
                return HttpNotFound();
            }
            return View(tblShop);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShopId,ShopName,Address,NoOfEmployees,TaxIdentificationNo,DateOfFounding")] TblShop tblShop)
        {
            if (ModelState.IsValid)
            {
                db.TblShops.Add(tblShop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblShop);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblShop tblShop = db.TblShops.Find(id);
            if (tblShop == null)
            {
                return HttpNotFound();
            }
            return View(tblShop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShopId,ShopName,Address,NoOfEmployees,TaxIdentificationNo,DateOfFounding")] TblShop tblShop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblShop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblShop);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblShop tblShop = db.TblShops.Find(id);
            if (tblShop == null)
            {
                return HttpNotFound();
            }
            return View(tblShop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TblShop tblShop = db.TblShops.Find(id);
            db.TblShops.Remove(tblShop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
