using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Puanlar
    {
        [Key]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KullaniciID { get; set; }
        public Kullanicilar Kullanici { get; set; }

        public int PuanDeger { get; set; }
        public DateTime KazanilanTarih { get; set; }
    }
}