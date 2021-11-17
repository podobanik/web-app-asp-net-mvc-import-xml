using WebAppAspNetMvcExportExcel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Rotativa;
using Common.Extentions;

namespace WebAppAspNetMvcExportExcel.Controllers
{
    public class ClientsController : Controller
    {
        
        [HttpGet]
        public ActionResult Index()
        {
            GosuslugiContext db = new GosuslugiContext();
            var client = db.Clients.ToList();
            return View(client);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var client = new Client();
            return View(client);
        }

        [HttpPost]
        public ActionResult Create(Client model)
        {
            var db = new GosuslugiContext();
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
            {
                var clients = db.Clients.ToList();
                ViewBag.Create = model;
                return View("Index", clients);
            }

            
            if (model.DocumentFile != null)
            {
                var data = new byte[model.DocumentFile.ContentLength];
                model.DocumentFile.InputStream.Read(data, 0, model.DocumentFile.ContentLength);

                model.Documents = new Document()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = model.DocumentFile.ContentType,
                    FileName = model.DocumentFile.FileName
                };
            }


            if (model.OrderIds != null && model.OrderIds.Any())
            {
                var orders = db.Orders.Where(s => model.OrderIds.Contains(s.Id)).ToList();
                model.Orders = orders;
            }
            if (model.AvailableDocumentIds != null && model.AvailableDocumentIds.Any())
            {
                var availableDocuments = db.AvailableDocuments.Where(s => model.AvailableDocumentIds.Contains(s.Id)).ToList();
                model.AvailableDocuments = availableDocuments;
            }
            if (model.ClientTypeIds != null && model.ClientTypeIds.Any())
            {
                var clientTypes = db.ClientTypes.Where(s => model.ClientTypeIds.Contains(s.Id)).ToList();
                model.ClientTypes = clientTypes;
            }
            if (model.CitizenshipId != null && model.CitizenshipId.Any())
            {
                var citizenships = db.Citizenships.Where(s => model.CitizenshipId.Contains(s.Id)).ToList();
                model.Citizenships = citizenships;
            }
            db.Clients.Add(model);
            db.SaveChanges();
            return RedirectPermanent("/Clients/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == id);

            if (client == null)
                return RedirectPermanent("/Clients/Index");

            db.Clients.Remove(client);
            db.SaveChanges();

            return RedirectPermanent("/Clients/Index");

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == id);

            if (client == null)
                return RedirectPermanent("/Clients/Index");


            return View(client);

        }

        [HttpPost]
        public ActionResult Edit(Client model)
        {


            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == model.Id);

            if (client == null)
                ModelState.AddModelError("Id", "Запись не найдена");
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");

            if (!ModelState.IsValid)
            {
                var clients = db.Clients.ToList();
                ViewBag.Edit = model;
                return View("Index", clients);
            }

            MappingOrder(model, client, db);

            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectPermanent("/Clients/Index");
        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            var db = new GosuslugiContext();
            var image = db.Documents.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                FileStream fs = System.IO.File.OpenRead(Server.MapPath(@"~/Content/Images/not-foto.png"));
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                fs.Close();

                return File(new MemoryStream(fileData), "image/jpeg");
            }

            return File(new MemoryStream(image.Data), image.ContentType);
        }

        private void MappingOrder(Client source, Client destination, GosuslugiContext db)
        {
            destination.Name = source.Name;
            destination.Surname = source.Surname;
            destination.Age = source.Age;
            destination.Birthday = source.Birthday;
            destination.Gender = source.Gender;
            destination.IsArchive = source.IsArchive;
            destination.Reviews = source.Reviews;
            destination.Key = source.Key;


            if (destination.Orders != null)
                destination.Orders.Clear();

            if (source.OrderIds != null && source.OrderIds.Any())
                destination.Orders = db.Orders.Where(s => source.OrderIds.Contains(s.Id)).ToList();

            if (destination.ClientTypes != null)
                destination.ClientTypes.Clear();

            if (source.ClientTypeIds != null && source.ClientTypeIds.Any())
                destination.ClientTypes = db.ClientTypes.Where(s => source.ClientTypeIds.Contains(s.Id)).ToList();

            if (destination.Citizenships != null)
                destination.Citizenships.Clear();

            if (source.CitizenshipId != null && source.CitizenshipId.Any())
                destination.Citizenships = db.Citizenships.Where(s => source.CitizenshipId.Contains(s.Id)).ToList();

            if (destination.AvailableDocuments != null)
                destination.AvailableDocuments.Clear();

            if (source.AvailableDocumentIds != null && source.AvailableDocumentIds.Any())
                destination.AvailableDocuments = db.AvailableDocuments.Where(s => source.AvailableDocumentIds.Contains(s.Id)).ToList();

            if (source.DocumentFile != null)
            {
                var image = db.Documents.FirstOrDefault(x => x.Id == source.Id);


                var data = new byte[source.DocumentFile.ContentLength];
                source.DocumentFile.InputStream.Read(data, 0, source.DocumentFile.ContentLength);

                destination.Documents = new Document()
                {
                    Guid = Guid.NewGuid(),
                    DateChanged = DateTime.Now,
                    Data = data,
                    ContentType = source.DocumentFile.ContentType,
                    FileName = source.DocumentFile.FileName
                };
            }


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
        public ActionResult Detail(int id)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Clients/Index");

            return View(client);

        }

        [HttpGet]
        public ActionResult Pdf(int id)
        {
            var db = new GosuslugiContext();
            var client = db.Clients.FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Books/Index");

            var pdf = new ViewAsPdf("Pdf", client);
            var data = pdf.BuildFile(this.ControllerContext);


            return File(new MemoryStream(data), "application/pdf", "detail.pdf");
        }

        [HttpGet]
        public ActionResult GetXlsx()
        {
            var db = new GosuslugiContext();
            var xlsx = db.Clients.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clients.xlsx");
        }
    }
}