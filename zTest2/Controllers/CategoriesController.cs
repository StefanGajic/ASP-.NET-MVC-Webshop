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

    public class CategoriesController : Controller
    {
        private zTest2DBEntities db = new zTest2DBEntities();
       
        public ActionResult Index()
        {
              if (Session["user"] != null)
            {
                if ((Session["user"] as zTest2.Models.TblUser).Role)
                {
            
            return View(db.TblCategories.ToList());
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
            TblCategory tblCategory = db.TblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,Description,Subcategory")] TblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {
                db.TblCategories.Add(tblCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblCategory tblCategory = db.TblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,Description,Subcategory")] TblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblCategory tblCategory = db.TblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TblCategory tblCategory = db.TblCategories.Find(id);
            db.TblCategories.Remove(tblCategory);
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
