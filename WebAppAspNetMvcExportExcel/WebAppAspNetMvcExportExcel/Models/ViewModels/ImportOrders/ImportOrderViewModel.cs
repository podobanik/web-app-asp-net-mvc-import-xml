using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAspNetMvcExportExcel.Models
{
    public class ImportOrderViewModel
    {
        /// <summary>
        /// Id
        /// </summary> 
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Ключ для создания/изменения записи
        /// </summary>    
        [Required]
        [Display(Name = "Ключ для создания/изменения записи", Order = 10)]
        [UIHint("Password")]
        [NotMapped]
        public string Key { get; set; }

        [Display(Name = "Файл импорта", Order = 20)]
        [Required(ErrorMessage = "Укажите файл импорта (.xlsx)")]
        public HttpPostedFileBase FileToImport { get; set; }
    }
}