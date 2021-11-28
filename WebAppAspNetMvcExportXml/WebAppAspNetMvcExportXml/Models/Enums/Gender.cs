using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebAppAspNetMvcExportXml.Models
{
    public enum Gender
    {
        [Display(Name ="Женский")]
        Female = 1,
        
        [Display(Name = "Мужской")]
        Male = 2,
    }
}