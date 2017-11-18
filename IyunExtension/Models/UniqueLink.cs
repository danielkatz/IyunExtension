using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IyunExtension.Models
{
    [Table("refcountrank")]
    public class UniqueLink
    {
        [Key]
        [Column("ref")]
        public string Ref { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("rank")]
        public double Rank { get; set; }
    }
}