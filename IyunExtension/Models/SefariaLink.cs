using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IyunExtension.Models
{
    [Table("linkshe")]
    public class SefariaLink
    {
        public int Id { get; set; }

        public string Citation_1 { get; set; }
        public string Citation_2 { get; set; }
        public string He_Citation_1 { get; set; }
        public string He_Citation_2 { get; set; }
        public string Conection_Type { get; set; }
        public string Text_1 { get; set; }
        public string Text_2 { get; set; }
        public string Category_1 { get; set; }
        public string Category_2 { get; set; }
    }
}