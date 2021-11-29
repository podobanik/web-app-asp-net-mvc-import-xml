using System.ComponentModel.DataAnnotations;

namespace WebAppAspNetMvcImportXml.Models
{
    public enum SettingType
    {
        [Display(Name = "Пароль")]
        Password = 1,
    }
}