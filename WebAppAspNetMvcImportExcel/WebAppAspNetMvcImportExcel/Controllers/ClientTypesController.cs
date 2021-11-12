using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAspNetMvcImportExcel.Models;

namespace WebAppAspNetMvcImportExcel.Controllers
{
    public class ClientTypesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var db = new GosuslugiContext();
            var clienttypes = db.ClientTypes.ToList();

            return View(clienttypes);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var clienttypes = new ClientType();
            return View(clienttypes);
        }

        [HttpPost]
        public ActionResult Create(ClientType model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
            {
                var clientTypes = db.ClientTypes.ToList();
                ViewBag.Create = model;
                return View("Index", clientTypes);
            }
            


            db.ClientTypes.Add(model);
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var clienttype = db.ClientTypes.FirstOrDefault(x => x.Id == id);
            if (clienttype == null)
                return RedirectPermanent("/ClientTypes/Index");

            db.ClientTypes.Remove(clienttype);
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var clienttype = db.ClientTypes.FirstOrDefault(x => x.Id == id);
            if (clienttype == null)
                return RedirectPermanent("/ClientTypes/Index");

            return View(clienttype);
        }

        [HttpPost]
        public ActionResult Edit(ClientType model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            var clienttype = db.ClientTypes.FirstOrDefault(x => x.Id == model.Id);
            if (clienttype == null)
                ModelState.AddModelError("Id", "Тип не найден");

            if (!ModelState.IsValid)
            {
                var clientTypes = db.ClientTypes.ToList();
                ViewBag.Create = model;
                return View("Index", clientTypes);
            }

            MappingGenre(model, clienttype);

            db.Entry(clienttype).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectPermanent("/ClientTypes/Index");
        }

        private void MappingGenre(ClientType sourse, ClientType destination)
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
    }
}