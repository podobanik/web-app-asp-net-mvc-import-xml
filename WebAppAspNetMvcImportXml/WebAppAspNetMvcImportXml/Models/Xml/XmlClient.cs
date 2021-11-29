using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebAppAspNetMvcImportXml.Models
{
    [XmlRoot("Client")]
    public class XmlClient
    {
        /// <summary>
        /// Имя
        /// </summary>    
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>  
        [XmlElement("Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Год издания книги
        /// </summary>  
        [XmlElement("Age")]
        public int Age { get; set; }

        /// <summary>
        /// Год издания книги
        /// </summary>  
        [XmlElement("Birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>  
        [XmlElement("Gender")]
        public Gender Gender { get; set; }

        /// <summary>
        /// Тип валюты
        /// </summary> 
        [XmlArray("ClientTypes")]
        [XmlArrayItem(typeof(XmlClientType), ElementName = "ClientType")]
        public virtual List<XmlClientType> ClientTypes { get; set; }

     


        ///// <summary>
        ///// Авторы
        ///// </summary> 
        [XmlArray("Orders")]
        [XmlArrayItem(typeof(XmlOrder), ElementName = "Order")]
        public virtual List<XmlOrder> Orders { get; set; }

        ///// <summary>
        ///// Языки
        ///// </summary> 
        [XmlArray("Citizenships")]
        [XmlArrayItem(typeof(XmlCitizenship), ElementName = "Citizenship")]
        public virtual List<XmlCitizenship> Citizenships { get; set; }

        ///// <summary>
        ///// Языки
        ///// </summary> 
        [XmlArray("AvailableDocuments")]
        [XmlArrayItem(typeof(XmlAvailableDocument), ElementName = "AvailableDocument")]
        public virtual List<XmlAvailableDocument> AvailableDocuments { get; set; }

        /// <summary>
        /// Архивная запись
        /// </summary>  
        [XmlElement("IsArchive")]
        public bool IsArchive { get; set; }

        /// <summary>
        /// Описание
        /// </summary>    
        [XmlElement("Annotation")]
        public string Reviews { get; set; }

        /// <summary>
        /// Обложка книги
        /// </summary>    
        [XmlElement("Document")]
        public XmlDocument Document { get; set; }
    }
}