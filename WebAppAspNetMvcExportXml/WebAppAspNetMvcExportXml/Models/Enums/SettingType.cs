using System.ComponentModel.DataAnnotations;

namespace WebAppAspNetMvcExportXml.Models
{
    public enum SettingType
    {
        [Display(Name = "Пароль")]
        Password = 1,
    }
}