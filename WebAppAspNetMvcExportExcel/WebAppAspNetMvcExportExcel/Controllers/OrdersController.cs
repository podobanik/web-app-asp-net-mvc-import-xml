using WebAppAspNetMvcExportExcel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Extentions;

namespace WebAppAspNetMvcExportExcel.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            GosuslugiContext db = new GosuslugiContext();
            var orders = db.Orders.ToList();
            return View(orders);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var order = new Order();
            return View(order);
        }

        [HttpPost]
        public ActionResult Create(Order model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
            {
                var orders = db.Orders.ToList();
                ViewBag.Create = model;
                return View("Index", orders);
            }


            db.Orders.Add(model);
            db.SaveChanges();
            return RedirectPermanent("/Orders/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var order = db.Orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectPermanent("/Orders/Index");

            db.Orders.Remove(order);
            db.SaveChanges();

            return RedirectPermanent("/Orders/Index");

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var order = db.Orders.FirstOrDefault(x => x.Id == id);

            if (order == null)
                return RedirectPermanent("/Orders/Index");


            return View(order);

        }

        [HttpPost]
        public ActionResult Edit(Order model)
        {


            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            var order = db.Orders.FirstOrDefault(x => x.Id == model.Id);

            if (order == null)
                ModelState.AddModelError("Id", "Запись не найдена");

            if (!ModelState.IsValid)
            {
                var orders = db.Orders.ToList();
                ViewBag.Create = model;
                return View("Index", orders);
            }

            MappingOrder(model, order);

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectPermanent("/Orders/Index");
        }
        private void MappingOrder(Order source, Order destination)
        {
            destination.Procedure = source.Procedure;
            destination.Description = source.Description;
            destination.Key = source.Key;
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
            var xlsx = db.Orders.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
        }
    }
}