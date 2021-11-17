using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAspNetMvcExportExcel.Models
{
    public class Order
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        [Required]
        [Display(Name = "Услуга", Order = 10)]
        public string Procedure { get; set; }

        /// <summary>
        /// Описание услуги
        /// </summary>
        
        [Display(Name = "Описание услуги", Order = 20)]
        public string Description { get; set; }

        /// <summary>
        /// Ключ для создания/изменения записи
        /// </summary>    
        [Required]
        [Display(Name = "Ключ для создания/изменения записи", Order = 30)]
        [UIHint("Password")]
        [NotMapped]
        public string Key { get; set; }

        /// <summary>
        /// Связь с таблицей Client
        /// </summary>
        [ScaffoldColumn(false)]
        public virtual ICollection<Client> Clients { get; set; }


    }
}