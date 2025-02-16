using Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akıllı_Etkinlik_Planlama_Platformu.Controllers
{
    public class KullaniciPanelController : Controller
    {
       
        Context c = new Context();
        // GET: KullaniciPanel yenı gırdım burada yazıyorum
        [Authorize]
        public ActionResult gerekli()
        {
            var mail = (string)Session["KullaniciAdi"];
            var degerler = c.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == mail);
            ViewBag.m = mail;
            return View(degerler);
        }


        public ActionResult Etkinliklerim()
        {
            var mail = (string)Session["KullaniciAdi"];
            var kullaniciId = c.Kullanicilars.Where(x => x.KullaniciAdi == mail).Select(y => y.ID).FirstOrDefault();
            var etkinlikIdleri = c.Katilimcilars
                                 .Where(k => k.KullaniciID == kullaniciId)
                                 .Select(k => k.EtkinlikID)
                                 .ToList();

            var etkinlikler = c.Etkinliklers
                               .Where(e => etkinlikIdleri.Contains(e.ID))
                               .ToList();

            return View(etkinlikler);
        }
        [HttpPost]
        public ActionResult KullaniciProfilGuncelle(Kullanicilar k)
        {
            var mail = (string)Session["KullaniciAdi"];
            var kullanici = c.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == mail);

            if (kullanici != null)
            {
                // Veritabanındaki kullanıcı bilgilerini güncelle
                kullanici.KullaniciAdi = k.KullaniciAdi;
                kullanici.Sifre = k.Sifre;
                kullanici.Eposta = k.Eposta;
                kullanici.Ad = k.Ad;
                kullanici.Soyad = k.Soyad;
                kullanici.TelefonNumarasi = k.TelefonNumarasi;
                kullanici.IlgiAlanlari = k.IlgiAlanlari;
                kullanici.DogumTarihi = k.DogumTarihi;

                c.SaveChanges();
                TempData["SuccessMessage"] = "Kullanıcı bilgileri başarıyla güncellendi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            }

            return RedirectToAction("gerekli");
        }

        public ActionResult KullaniciGetir(int id)
        {
            var etk = c.Kullanicilars.Find(id);
            return View("KullaniciGetir", etk);

        }
        public ActionResult KullaniciGuncelle(Kullanicilar k)
        {

            var etk = c.Kullanicilars.Find(k.ID);

            // Veritabanındaki kullanıcıyı güncelle
            etk.KullaniciAdi = k.KullaniciAdi;
            etk.Sifre = k.Sifre;
            etk.Eposta = k.Eposta;
            etk.Ad = k.Ad;
            etk.Soyad = k.Soyad;
            etk.TelefonNumarasi = k.TelefonNumarasi;
            etk.IlgiAlanlari = k.IlgiAlanlari;
            etk.Konum = k.Konum;
            etk.DogumTarihi = k.DogumTarihi;
            etk.Cinsiyet = k.Cinsiyet;
            etk.IsAdmin = k.IsAdmin;
            c.SaveChanges();
            return View("KullaniciGetir", etk);
        }

    }
}