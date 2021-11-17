using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAppAspNetMvcExportExcel.Models
{
    public class ClientType
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Тип клиента", Order = 10)]
        public string Name { get; set; }

        /// <summary>
        /// Ключ для создания/изменения записи
        /// </summary>    
        [Required]
        [Display(Name = "Ключ для создания/изменения записи", Order = 20)]
        [UIHint("Password")]
        [NotMapped]
        public string Key { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Client> Clients { get; set; }

    }
}