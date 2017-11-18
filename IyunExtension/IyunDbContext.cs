using IyunExtension.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IyunExtension
{
    public class IyunDbContext : DbContext
    {
        public IyunDbContext()
            : base(@"Data Source=.\sqlexpress;Initial Catalog=sefaria-data;Integrated Security=True") { }

        public DbSet<UniqueLink> UniqueLinks { get; set; }
        public DbSet<SefariaLink> SefariaLinks { get; set; }
    }
}