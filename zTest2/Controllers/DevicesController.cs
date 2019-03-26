using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using zTest2.Models;

namespace zTest2.Controllers
{
    public class DevicesController : Controller
    {
        private zTest2DBEntities db = new zTest2DBEntities();

        private static string SearchQuery = "";
        private static int SearchCriteria = 0;

        public ActionResult Index()
        {
            if (Session["msg"] != null)
            {
                ViewBag.Message = Session["msg"];
                Session["msg"] = null;
            }

            var tblDevices = db.TblDevices.Include(t => t.TblCategory).Include(t => t.TblShop).Where(t => t.Active == true);

            if (!SearchQuery.Equals(""))
            {
                List<TblDevice> listDevices = null;

                switch (SearchCriteria)
                {
                    case 1:
                        listDevices = tblDevices.Where(t => t.DeviceName.StartsWith(SearchQuery)).ToList();
                        break;
                    case 2:
                        listDevices = tblDevices.Where(t => t.DescriptionDevice.Contains(SearchQuery)).ToList();
                        break;
                    case 3:
                        listDevices = tblDevices.Where(t => t.Components.Contains(SearchQuery)).ToList();
                        break;
                    case 4:
                        listDevices = tblDevices.Where(t => t.MadeInCountry.StartsWith(SearchQuery)).ToList();
                        break;
                    case 5:
                        listDevices = tblDevices.Where(t => t.TblCategory.CategoryName.StartsWith(SearchQuery)).ToList();
                        break;
                    case 6:
                        listDevices = tblDevices.Where(t => t.TblShop.ShopName.StartsWith(SearchQuery)).ToList();
                        break;
                }

                SearchQuery = "";
                SearchCriteria = 0;

                return View(listDevices);
            }

            return View(tblDevices.ToList());
        }

        [HttpPost]
        public ActionResult Search() 
        {
            try
            {
                SearchCriteria = int.Parse(Request["searchCriteria"]);
                SearchQuery = Request["searchQuery"];
            }
            catch {
                SearchQuery = "";
                SearchCriteria = 0;
            }

            return RedirectToAction("Index", "Devices");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDevice tblDevice = db.TblDevices.Find(id);
            if (tblDevice == null)
            {
                return HttpNotFound();
            }
            return View(tblDevice);
        }

        public ActionResult Create()
        {
            if (Session["user"] != null)
            {
                if ((Session["user"] as zTest2.Models.TblUser).Role)
                {
                    ViewBag.Category = new SelectList(db.TblCategories, "CategoryId", "CategoryName");
                    ViewBag.Shop = new SelectList(db.TblShops, "ShopId", "ShopName");
                    return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeviceId,DeviceName,DescriptionDevice,Components,Shop,MadeInCountry,Quantity,Price,file,Category")] TblDevice tblDevice, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                bool fileUploaded = false;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/DeviceImages/"), fileName);
                    file.SaveAs(path);

                    fileUploaded = true;
                }

                if (fileUploaded)
                {
                    tblDevice.Picture = file.FileName;

                    db.TblDevices.Add(tblDevice);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.Category = new SelectList(db.TblCategories, "CategoryId", "CategoryName", tblDevice.Category);
            ViewBag.Shop = new SelectList(db.TblShops, "ShopId", "ShopName", tblDevice.Shop);
            return View(tblDevice);
        }

        public ActionResult Edit(int? id)
        {
            if (Session["user"] != null)
            {
                if ((Session["user"] as zTest2.Models.TblUser).Role)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    TblDevice tblDevice = db.TblDevices.Find(id.Value);

                    if (tblDevice == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.Category = new SelectList(db.TblCategories, "CategoryId", "CategoryName", tblDevice.Category);
                    ViewBag.Shop = new SelectList(db.TblShops, "ShopId", "ShopName", tblDevice.Shop);
                    return View(tblDevice);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeviceId,DeviceName,DescriptionDevice,Components,Shop,MadeInCountry,Quantity,Price,file,Category")] TblDevice tblDevice, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                tblDevice.Active = true;

                bool fileUploaded = false;

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/DeviceImages/"), fileName);
                    file.SaveAs(path);

                    fileUploaded = true;
                }

                if (fileUploaded){
                    tblDevice.Picture = file.FileName;
                    db.Entry(tblDevice).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else {
                    if (tblDevice.Picture != null)
                    {
                        db.Entry(tblDevice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        tblDevice.Picture = Request["altfile"];
                        db.Entry(tblDevice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Category = new SelectList(db.TblCategories, "CategoryId", "CategoryName", tblDevice.Category);
            ViewBag.Shop = new SelectList(db.TblShops, "ShopId", "ShopName", tblDevice.Shop);
            return View(tblDevice);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TblDevice tblDevice = db.TblDevices.Find(id);
            if (tblDevice == null)
            {
                return HttpNotFound();
            }
            return View(tblDevice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TblDevice tblDevice = db.TblDevices.Find(id);
            tblDevice.Active = false;
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
