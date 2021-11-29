using Common.Extentions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAspNetMvcImportXml.Models;

namespace WebAppAspNetMvcImportXml.Controllers
{
    public class AvailableDocumentsController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            var db = new GosuslugiContext();
            var availableDocuments = db.AvailableDocuments.ToList();

            return View(availableDocuments);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var availableDocuments = new AvailableDocument();
            return View(availableDocuments);
        }

        [HttpPost]
        public ActionResult Create(AvailableDocument model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
            {
                var availabledocuments = db.AvailableDocuments.ToList();
                ViewBag.Create = model;
                return View("Index", availabledocuments);
            }



            db.AvailableDocuments.Add(model);
            db.SaveChanges();

            return RedirectPermanent("/AvailableDocuments/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var availableDocuments = db.AvailableDocuments.FirstOrDefault(x => x.Id == id);
            if (availableDocuments == null)
                return RedirectPermanent("/AvailableDocuments/Index");

            db.AvailableDocuments.Remove(availableDocuments);
            db.SaveChanges();

            return RedirectPermanent("/AvailableDocuments/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var availableDocuments = db.AvailableDocuments.FirstOrDefault(x => x.Id == id);
            if (availableDocuments == null)
                return RedirectPermanent("/AvailableDocuments/Index");

            return View(availableDocuments);
        }

        [HttpPost]
        public ActionResult Edit(AvailableDocument model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            var availableDocuments = db.AvailableDocuments.FirstOrDefault(x => x.Id == model.Id);
            if (availableDocuments == null)
                ModelState.AddModelError("Id", "Документ не найден");

            if (!ModelState.IsValid)
            {
                var availabledocuments = db.AvailableDocuments.ToList();
                ViewBag.Create = model;
                return View("Index", availabledocuments);
            }

            MappingCitizenships(model, availableDocuments);

            db.Entry(availableDocuments).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/AvailableDocuments/Index");
        }

        private void MappingCitizenships(AvailableDocument sourse, AvailableDocument destination)
        {
            destination.Name = sourse.Name;
            destination.Key = sourse.Key;
        }
        private string GetKey()
        {
            var db = new GosuslugiContext();
            var setting = db.Settings.FirstOrDefault(x => x.Type == SettingType.Password);
            if (setting == null)
                throw new Exception("Setting not found");

            return setting.Value;
        }


        [HttpGet]
        public ActionResult GetXlsx()
        {
            var db = new GosuslugiContext();
            var xlsx = db.AvailableDocuments.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Documents.xlsx");
        }
    }
}