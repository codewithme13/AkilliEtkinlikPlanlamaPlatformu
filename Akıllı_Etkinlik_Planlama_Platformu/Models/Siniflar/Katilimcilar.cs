using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Katilimcilar
    {
        [Key, Column(Order = 0)] // İlk anahtar olarak KullaniciID
        public int KullaniciID { get; set; }
        public Kullanicilar Kullanici { get; set; }

        [Key, Column(Order = 1)] // İkinci anahtar olarak EtkinlikID
        public int EtkinlikID { get; set; }
        public Etkinlikler Etkinlik { get; set; }
    }
}