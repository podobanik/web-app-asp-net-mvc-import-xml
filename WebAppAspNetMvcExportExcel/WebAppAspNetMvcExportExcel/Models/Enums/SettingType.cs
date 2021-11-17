using System.ComponentModel.DataAnnotations;

namespace WebAppAspNetMvcExportExcel.Models
{
    public enum SettingType
    {
        [Display(Name = "Пароль")]
        Password = 1,
    }
}