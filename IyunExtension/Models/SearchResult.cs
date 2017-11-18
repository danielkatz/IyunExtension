using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IyunExtension.Models
{
    public class SearchResult
    {
        public SefariaLink Link { get; set; }
        public UniqueLink Rank { get; set; }
        public int Level { get; set; }
    }
}