using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar
{
    public class Mesajlar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int GondericiID { get; set; }
        public Kullanicilar Gonderici { get; set; }

        public int AliciID { get; set; }
        public Kullanicilar Alici { get; set; }
        [Column(TypeName = "Varchar")]
        [StringLength(200)]
        public string MesajMetni { get; set; }
        public DateTime GonderimZamani { get; set; }
    }
}