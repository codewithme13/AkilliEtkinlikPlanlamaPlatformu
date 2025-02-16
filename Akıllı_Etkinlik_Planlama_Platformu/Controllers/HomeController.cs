using System.Web.Mvc;
using Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Akıllı_Etkinlik_Planlama_Platformu.Controllers
{
    public class HomeController : Controller
    {
        Context c = new Context();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Baslangic()
        {
            var kullaniciId = Session["ID"];

            // 2. Kullanıcıyı ID'ye göre veritabanından alıyoruz
            var kullanici = c.Kullanicilars
                             .Where(k => k.ID == (int)kullaniciId)
                             .FirstOrDefault();

            if (kullanici == null)
            {
                // Eğer kullanıcı bulunamazsa, hata mesajı döndürüyoruz
                return HttpNotFound("Kullanıcı bulunamadı");
            }

            // 3. Kullanıcının ilgi alanlarını alıyoruz
            var kullaniciIlgiAlanlari = kullanici.IlgiAlanlari;

            // 4. Etkinlikler tablosundaki ilgi alanlarına uygun etkinlikleri alıyoruz
            var uygunEtkinlikler = c.Etkinliklers
                                     .Where(e => e.Tarih >= DateTime.Now) // Yalnızca gelecekteki etkinlikleri alıyoruz
                                     .Where(e => kullaniciIlgiAlanlari.Contains(e.Kategori)) // Kategori karşılaştırması
                                     .OrderBy(e => e.Tarih) // Etkinlikleri tarihe göre sıralıyoruz
                                     .Take(3) // İlk 3 etkinliği alıyoruz
                                     .ToList();

            // 5. Tüm etkinlikleri alıyoruz
            var tumEtkinlikler = c.Etkinliklers
                                   .Where(e => e.Tarih >= DateTime.Now) // Yalnızca gelecekteki etkinlikleri alıyoruz
                                   .OrderBy(e => e.Tarih) // Etkinlikleri tarihe göre sıralıyoruz
                                   .ToList();

            // 6. Uygun etkinlikleri ve tüm etkinlikleri view'a gönderiyoruz
            ViewBag.UygunEtkinlikler = uygunEtkinlikler;
            return View(tumEtkinlikler);

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}