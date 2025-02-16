using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Etkinlikler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(50)]
        public string EtkinlikAdi { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }
        public TimeSpan EtkinlikSuresi { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string Konum { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        public string Kategori { get; set; }
        public ICollection<Katilimcilar> Katilimcilar { get; set; }
    }
}