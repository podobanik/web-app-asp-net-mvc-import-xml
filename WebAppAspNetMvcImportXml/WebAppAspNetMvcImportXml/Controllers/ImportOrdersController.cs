using ClosedXML.Excel;
using Common.Extentions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebAppAspNetMvcImportXml.Models;


namespace WebAppAspNetMvcImportXml.Controllers
{
    public class ImportOrdersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ImportOrderViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Import(ImportOrderViewModel model, LogHistory loghistories)
        {
            
            if (model.Key != GetKey())
                ModelState.AddModelError("Key", "Ключ для создания/изменения записи указан не верно");
            if (!ModelState.IsValid)
                return View("Index", model);

            var log = ProceedImport(model);
            var db = new GosuslugiContext();

            MappingLog(log, loghistories);
            db.LogHistories.Add(loghistories);
            db.SaveChanges();

            return View("Log", log);
        }

        public ActionResult GetExample()
        {
            return File("~/Content/Files/ImportOrdersExample.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ImportOrdersExample.xlsx");
        }

        private ImportOrderLog ProceedImport(ImportOrderViewModel model)
        {
            var startTime = DateTime.Now;

            var workBook = new XLWorkbook(model.FileToImport.InputStream);
            var workSheet = workBook.Worksheet(1);
            var rows = workSheet.RowsUsed().Skip(1).ToList();

            var logs = new List<ImportOrderRowLog>();
            var data = ParseRows(rows, logs, model);
            ApplyImported(data);

            var successCount = data.Count();
            var failedCount = rows.Count() - successCount;
            var finishTime = DateTime.Now;

            var result = new ImportOrderLog()
            {
                StartImport = startTime,
                EndImport = finishTime,
                SuccessCount = successCount,
                FailedCount = failedCount,
                Logs = logs
            };
            
            return result;
        }

        private List<ImportOrderData> ParseRows(IEnumerable<IXLRow> rows, List<ImportOrderRowLog> logs, ImportOrderViewModel model)
        {
            var result = new List<ImportOrderData>();
            int index = 1;
            foreach (var row in rows)
            {
                try
                {
                    var data = new ImportOrderData()
                    {
                        Procedure = ConvertToString(row.Cell("A").GetValue<string>().Trim()),
                        Description = ConvertToString(row.Cell("B").GetValue<string>().Trim()),
                        Key = model.Key,
                    };

                    result.Add(data);
                    logs.Add(new ImportOrderRowLog()
                    {
                        Id = index,
                        Message = $"ОК",
                        Type = ImportOrderRowLogType.Success
                    }); ;

                }
                catch (Exception ex)
                {
                    logs.Add(new ImportOrderRowLog()
                    {
                        Id = index,
                        Message = $"Error: {ex.GetBaseException().Message}",
                        Type = ImportOrderRowLogType.ErrorParsed
                    }); ;
                }

                index++;
            }


            return result;
        }

        private void ApplyImported(List<ImportOrderData> data)
        {
            var db = new GosuslugiContext();

            foreach (var value in data)
            {
                var model = new Order()
                {
                    Procedure = value.Procedure,
                    Description = value.Description,
                    Key = value.Key,
                };

                db.Orders.Add(model);
                db.SaveChanges();
            }
        }

        private string ConvertToString(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Значение не определено");

            var reuslt = HandleInjection(value);

            return reuslt;
        }
        private string HandleInjection(string value)
        {
            var badSymbols = new Regex(@"^[+=@-].*");
            return Regex.IsMatch(value, badSymbols.ToString()) ? string.Empty : value;
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
        public ActionResult History()
        {
            GosuslugiContext db = new GosuslugiContext();
            var logHistory = db.LogHistories.ToList();
            return View(logHistory);
        }

        [HttpGet]
        public ActionResult DeleteLog(int id)
        {
            var db = new GosuslugiContext();
            var logHistory = db.LogHistories.FirstOrDefault(x => x.Id == id);

            if (logHistory == null)
                return RedirectPermanent("/ImportOrders/History");

            db.LogHistories.Remove(logHistory);
            db.SaveChanges();

            return RedirectPermanent("/ImportOrders/History");

        }
        private void MappingLog(ImportOrderLog source, LogHistory destination)
        {
            destination.StartImport = source.StartImport;
            destination.EndImport = source.EndImport;
            destination.SuccessCount = source.SuccessCount;
            destination.FailedCount = source.FailedCount;
        }


        [HttpGet]
        public ActionResult GetXlsx()
        {
            var db = new GosuslugiContext();
            var xlsx = db.LogHistories.ToXlsx();

            return File(xlsx.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Logs.xlsx");
        }
    }
}