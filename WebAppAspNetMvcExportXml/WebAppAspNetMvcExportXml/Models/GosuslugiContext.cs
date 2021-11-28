using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebAppAspNetMvcExportXml.Models
{
    public class GosuslugiContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<Citizenship> Citizenships { get; set; }
        public DbSet<AvailableDocument> AvailableDocuments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<LogHistory> LogHistories { get; set; }
        public GosuslugiContext() : base("GosuslugiEntity")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasOptional(x => x.Documents).WithRequired().WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }
    }
}