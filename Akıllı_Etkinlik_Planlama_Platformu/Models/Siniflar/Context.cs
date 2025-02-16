using System.Data.Entity;

namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Context : DbContext
    {
        public DbSet<Etkinlikler> Etkinliklers { get; set; }
        public DbSet<Katilimcilar> Katilimcilars { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Kompozit anahtar tanımlaması
            modelBuilder.Entity<Katilimcilar>()
                .HasKey(k => new { k.KullaniciID, k.EtkinlikID });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Kullanicilar> Kullanicilars { get; set; }
        public DbSet<Mesajlar> Mesajlars { get; set; }
        public DbSet<Puanlar> Puanlars { get; set; }
    }

}