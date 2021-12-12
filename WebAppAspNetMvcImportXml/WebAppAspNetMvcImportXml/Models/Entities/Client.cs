using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Common.Attributes;
using Common.Extentions;


namespace WebAppAspNetMvcImportXml.Models
{
    public class Client
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Имя клиента
        /// </summary>
        [Required]
        [Display(Name = "Имя клиента", Order = 5)]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия клиента
        /// </summary>
        [Required]
        [Display(Name = "Фамилия клиента", Order = 10)]
        public string Surname { get; set; }

        /// <summary>
        /// Возраст клиента
        /// </summary>
        [Required]
        [Display(Name = "Возраст клиента", Order = 20)]
        public int Age { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        
        [Display(Name = "Дата рождения", Order = 30)]
        public DateTime? Birthday { get; set; }

        [ScaffoldColumn(false)]
        public Gender Gender { get; set; }


        [Display(Name = "Пол", Order = 40)]
        [UIHint("DropDownList")]
        [TargetProperty("Gender")]
        [NotMapped]
        public IEnumerable<SelectListItem> GenderDictionary
        {
            get
            {
                var dictionary = new List<SelectListItem>();

                foreach (Gender type in Enum.GetValues(typeof(Gender)))
                {
                    dictionary.Add(new SelectListItem
                    {
                        Value = ((int)type).ToString(),
                        Text = type.GetDisplayValue(),
                        Selected = type == Gender
                    });
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Пол
        /// </summary>
        /// [Required]
        [ScaffoldColumn(false)]
        public List<int> ClientTypeIds { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<ClientType> ClientTypes { get; set; }

        [Display(Name = "Тип клиента", Order = 50)]
        [UIHint("RadioList")]
        [TargetProperty("ClientTypeIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> ClientTypeDictionary
        {
            get
            {
                var db = new GosuslugiContext();
                var query = db.ClientTypes;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Clients.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.Name}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }




        /// <summary>
        /// Связь с таблицей Order
        /// </summary>
        [ScaffoldColumn(false)]
        public virtual ICollection<Order> Orders { get; set; }
        
        [ScaffoldColumn(false)]
        public virtual Document Documents { get; set; }


        [Display(Name = "Фото клиента", Order = 60)]
        [NotMapped]
        public HttpPostedFileBase DocumentFile { get; set; }




        [ScaffoldColumn(false)]
        public List<int> OrderIds { get; set; }

        [Display(Name = "Услуги", Order = 70)]
        [UIHint("MultipleDropDownList")]
        [TargetProperty("OrderIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> OrderDictionary
        {
            get
            {
                var db = new GosuslugiContext();
                var query = db.Orders;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Clients.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.Procedure}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }

         
        

        
        /// <summary>
        /// Гражданство
        /// </summary> 
        
        [ScaffoldColumn(false)]
        public virtual ICollection<Citizenship> Citizenships { get; set; }

        [ScaffoldColumn(false)]
        public List<int> CitizenshipId { get; set; }
        

        [Display(Name = "Гражданство", Order = 80)]
        [UIHint("MultipleSelect")]
        [TargetProperty("CitizenshipId")]
        [NotMapped]
        public IEnumerable<SelectListItem> CitizenshipDictionary
        {
            get
            {
                var db = new GosuslugiContext();
                var query = db.Citizenships;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Clients.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.Name}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }
        /// <summary>
        /// Список документов
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<AvailableDocument> AvailableDocuments { get; set; }

        [ScaffoldColumn(false)]
        public List<int> AvailableDocumentIds { get; set; }

        [Display(Name = "Имеющиеся документы", Order = 90)]
        [UIHint("MultipleSelect")]
        [TargetProperty("AvailableDocumentIds")]
        [NotMapped]
        public IEnumerable<SelectListItem> AvailableDocumentDictionary
        {
            get
            {
                var db = new GosuslugiContext();
                var query = db.AvailableDocuments;

                if (query != null)
                {
                    var Ids = query.Where(s => s.Clients.Any(ss => ss.Id == Id)).Select(s => s.Id).ToList();
                    var dictionary = new List<SelectListItem>();
                    dictionary.AddRange(query.ToSelectList(c => c.Id, c => $"{c.Name}", c => Ids.Contains(c.Id)));
                    return dictionary;
                }

                return new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            }
        }
        
        /// <summary>
        /// Отзывы
        /// </summary>   
        
        [Display(Name = "Отзыв", Order = 110)]
        [UIHint("TextArea")]
        public string Reviews { get; set; }

        /// <summary>
        /// Ключ для создания/изменения записи
        /// </summary>    
        [Required]
        [Display(Name = "Ключ для создания/изменения записи", Order = 120)]
        [UIHint("Password")]
        [NotMapped]
        public string Key { get; set; }

        /// <summary>
        /// Архивная запись
        /// </summary>  
        [Display(Name = "Архивная запись", Order = 100)]
        public bool IsArchive { get; set; }
    }
}