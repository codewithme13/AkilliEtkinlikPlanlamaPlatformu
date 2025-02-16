using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Kullanicilar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        public string KullaniciAdi { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        public string Sifre { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(40)]
        public string Eposta { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string Konum { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string IlgiAlanlari { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        public string Ad { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(30)]
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(10)]
        public string Cinsiyet { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(15)]
        public string TelefonNumarasi { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string ProfilFotoDosyaYolu { get; set; }
        public bool IsAdmin { get; set; }


        public ICollection<Katilimcilar> KatildigiEtkinlikler { get; set; }
        public ICollection<Mesajlar> GonderilenMesajlar { get; set; }
        public ICollection<Mesajlar> AlinanMesajlar { get; set; }
        public ICollection<Puanlar> Puanlar { get; set; }
    }
}