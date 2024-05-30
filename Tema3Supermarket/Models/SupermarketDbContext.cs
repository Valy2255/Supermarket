using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext() : base("name=mvpDB") 
            {
                Database.Log = sql => Debug.WriteLine(sql); 
            }
        

        public DbSet<Produs> Produse { get; set; }
        public DbSet<Producator> Producatori { get; set; }
        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Stoc> Stocuri { get; set; }
        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<BonCasa> BonuriCasa { get; set; }
        public DbSet<ProdusBon> ProduseBon { get; set; }
        public DbSet<Oferta> Oferte { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produs>()
                .HasRequired(p => p.Categorie)
                .WithMany(c => c.Produse)
                .HasForeignKey(p => p.CategorieId);

            modelBuilder.Entity<Produs>()
                .HasRequired(p => p.Producator)
                .WithMany(pr => pr.Produse)
                .HasForeignKey(p => p.ProducatorId);

            modelBuilder.Entity<Stoc>()
                .HasRequired(s => s.Produs)
                .WithMany(p => p.Stocuri)
                .HasForeignKey(s => s.ProdusId);

            modelBuilder.Entity<BonCasa>()
                .HasRequired(b => b.Casier)
                .WithMany(u => u.BonuriCasa)
                .HasForeignKey(b => b.CasierId);

            modelBuilder.Entity<ProdusBon>()
                .HasRequired(pb => pb.BonCasa)
                .WithMany(b => b.ProduseBon)
                .HasForeignKey(pb => pb.BonId);

            modelBuilder.Entity<ProdusBon>()
                .HasRequired(pb => pb.Produs)
                .WithMany(p => p.ProduseBon)
                .HasForeignKey(pb => pb.ProdusId);

            modelBuilder.Entity<Oferta>()
                .HasRequired(o => o.Produs)
                .WithMany(p => p.Oferte)
                .HasForeignKey(o => o.ProdusId);
        }
    }
}
