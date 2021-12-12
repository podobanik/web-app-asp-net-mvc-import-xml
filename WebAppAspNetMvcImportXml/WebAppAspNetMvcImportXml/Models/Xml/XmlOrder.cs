using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebAppAspNetMvcImportXml.Models
{
    [XmlRoot("Order")]
    public class XmlOrder
    {
        /// <summary>
        /// Id
        /// </summary> 
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Procedure { get; set; }
    }
}