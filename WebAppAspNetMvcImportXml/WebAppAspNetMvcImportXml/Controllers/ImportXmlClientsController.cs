using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using WebAppAspNetMvcImportXml.Models;

namespace WebAppAspNetMvcImportXml.Controllers
{
    public class ImportXmlClientsController : Controller
    {
        //private readonly string _key = "123456Qq";
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ImportXmlClientViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Import(ImportXmlClientViewModel model)
        {
            if (model.Key != GetKey())
               ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
                return View("Index", model);
            
            var file = new byte[model.FileToImport.InputStream.Length];
            model.FileToImport.InputStream.Read(file, 0, (int)model.FileToImport.InputStream.Length);

            XmlSerializer xml = new XmlSerializer(typeof(List<XmlClient>));
            var clients = (List<XmlClient>)xml.Deserialize(new MemoryStream(file));
            var db = new GosuslugiContext();

            foreach (var client in clients)
            {
                db.Clients.Add(new Client()
                {
                    Reviews = client.Reviews,
                    Name = client.Name,
                    Surname = client.Surname,
                    IsArchive = client.IsArchive,
                    Age = client.Age,
                    Birthday = client.Birthday,
                    Gender = client.Gender,
                    AvailableDocumentIds = client.AvailableDocuments.Select(s => s.Id).ToList(),
                    OrderIds = client.Orders.Select(s => s.Id).ToList(),
                    CitizenshipId = client.Citizenships.Select(s => s.Id).ToList(),
                    Documents = client.Document == null ? null : new Document()
                    {
                        ContentType = client.Document.ContentType,
                        Data = Convert.FromBase64String(client.Document.Data),
                        DateChanged = DateTime.Now,
                        FileName = client.Document.FileName
                    },
                    Key = GetKey()
                }) ;

                db.SaveChanges();
            }

            return RedirectPermanent("/Clients/Index");
        }

        public ActionResult GetExample()
        {
            return File("~/Content/Files/ImportXmlClientsExample.xml", "application/xml", "ImportXmlClientsExample.xml");
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