using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAspNetMvcImportExcel.Models
{
    public class AvailableDocument
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>    
        [Required]
        [Display(Name = "Название", Order = 10)]
        public string Name { get; set; }

        /// <summary>
        /// Ключ для создания/изменения записи
        /// </summary>    
        [Required]
        [Display(Name = "Ключ для создания/изменения записи", Order = 20)]
        [UIHint("Password")]
        [NotMapped]
        public string Key { get; set; }

        /// <summary>
        /// Список клиентов
        /// </summary> 
        [ScaffoldColumn(false)]
        public virtual ICollection<Client> Clients { get; set; }
    }
}