using System.ComponentModel.DataAnnotations;

namespace WebAppAspNetMvcExportExcel.Models
{
    public enum ImportOrderRowLogType
    {
        [Display(Name = "Успешно")]
        Success = 1,

        [Display(Name = "Ошибка при парсинге строки")]
        ErrorParsed = 2,
    }
}