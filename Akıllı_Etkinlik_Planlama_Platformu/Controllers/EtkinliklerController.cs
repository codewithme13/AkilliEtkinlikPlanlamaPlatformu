using Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System;
using System.Data.Entity;
using System.Web.Security;
using Akıllı_Etkinlik_Planlama_Platformu.Helpers;

namespace Akıllı_Etkinlik_Planlama_Platformu.Controllers
{
    public class EtkinliklerController : Controller
    {
        Context c = new Context();
        public ActionResult Index(int sayfa = 1, string p = null)
        {
            var etkinlikler = from x in c.Etkinliklers select x;

            if (!string.IsNullOrEmpty(p))
            {
                etkinlikler = etkinlikler.Where(y => y.EtkinlikAdi.Contains(p));
            }

            var degerler = etkinlikler.ToList().ToPagedList(sayfa, 5);

            return View(degerler);
        }
        public ActionResult TumEtkinlikler()
        {

            var etkinlikler = c.Etkinliklers
               .OrderByDescending(e => e.Tarih)
               .ToList();
            if (!etkinlikler.Any())
            {
                ViewBag.Mesaj = "Etkinlik bulunamadı.";
            }
            return View(etkinlikler);

        }


        public ActionResult EnYakinEtkinlikler()
        {

            var enYakinEtkinlikler = c.Etkinliklers
                .Where(e => e.Tarih >= DateTime.Now)  // Şu anki tarih ve saatten büyük etkinlikleri alıyoruz
                .OrderBy(e => e.Tarih)  // Tarihe göre sıralıyoruz
                .Take(3)  // İlk 3 etkinliği alıyoruz
                .ToList();  

            return View(enYakinEtkinlikler);
        }

        [HttpGet]
        public ActionResult EtkinlikEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EtkinlikEkle(Etkinlikler k)
        {
            c.Etkinliklers.Add(k);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EtkinlikSil(int id)
        {
            var etk = c.Etkinliklers.Find(id);
            c.Etkinliklers.Remove(etk);
            c.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult EtkinlikGetir(int id)
        {
            var etk = c.Etkinliklers.Find(id);
            return View("EtkinlikGetir", etk);

        }
        public ActionResult EtkinlikGuncelle(Etkinlikler k)
        {
            try
            {
                var etk = c.Etkinliklers.Find(k.ID);
                if (etk != null)
                {
                    // Güncellenen alanları atıyoruz
                    etk.EtkinlikAdi = k.EtkinlikAdi;
                    etk.Aciklama = k.Aciklama;
                    etk.Tarih = k.Tarih;
                    etk.Saat = k.Saat;
                    etk.EtkinlikSuresi = k.EtkinlikSuresi;
                    etk.Konum = k.Konum;
                    etk.Kategori = k.Kategori;

                    c.SaveChanges(); // Veritabanına kaydediyoruz
                    TempData["SuccessMessage"] = "Etkinlik başarıyla güncellendi."; // Başarı mesajı
                }
                else
                {
                    TempData["ErrorMessage"] = "Güncellenecek etkinlik bulunamadı."; // Hata mesajı
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Etkinlik güncellenirken bir hata oluştu: " + ex.Message; // Hata mesajı
            }
            return RedirectToAction("Index");
        }
        


        [HttpGet]
        public ActionResult EtkinlikKategori(string kategori)
        {

            var etkinlikler = c.Etkinliklers
                                .Where(e => e.Kategori == kategori)
                                .ToList();

            // Etkinlikleri ViewBag ile view'a gönderiyoruz
            ViewBag.Kategori = kategori;
            return View(etkinlikler);
        }


        [HttpPost]
        public JsonResult Katıl(int etkinlikID)
        {
            try
            {
                // Session'dan kullanıcı adını al
                var kullaniciAdi = Session["KullaniciAdi"]?.ToString();

                if (string.IsNullOrEmpty(kullaniciAdi))
                {
                    return Json(new { success = false, message = "Kullanıcı adı bulunamadı." });
                }

                // Kullanıcı adı ile kullanıcıyı 'Kullanicilars' tablosundan bul
                var kullanici = c.Kullanicilars.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi);

                if (kullanici == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                // Kullanıcı ID'sini al
                int kullaniciID = kullanici.ID;

                // Katılım kaydını oluştur
                var katilimci = new Katilimcilar
                {
                    KullaniciID = kullaniciID,
                    EtkinlikID = etkinlikID
                };

                c.Katilimcilars.Add(katilimci);
                c.SaveChanges();

                return Json(new { success = true, message = "Etkinliğe katılım başarılı!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bu etkinliğe kaydınız zaten mevcut! Lütfen başka etkinliklere kayıt yaptırmayı deneyiniz" });
            }
        }


        public ActionResult EtkinlikOlusturma()
        {
            var model = new Etkinlikler();
            return View(model);
        }

        [HttpPost]
        public ActionResult EtkinlikOlusturma(Etkinlikler model)
        {
            if (ModelState.IsValid)
            {
               
                    c.Etkinliklers.Add(model);
                    c.SaveChanges();

                    int kullaniciID = (int)Session["ID"]; 

                    var mevcutPuan = c.Puanlars
                                            .Where(p => p.KullaniciID == kullaniciID)
                                            .OrderByDescending(p => p.KazanilanTarih) // En son eklenen puanı al
                                            .FirstOrDefault();

                    if (mevcutPuan != null)
                    {
                        mevcutPuan.PuanDeger += 15;

                        c.SaveChanges();
                    }
                    else
                    {
                        var yeniPuanKaydi = new Puanlar
                        {
                            KullaniciID = kullaniciID,
                            PuanDeger = 15,
                            KazanilanTarih = DateTime.Now
                        };
                        c.Puanlars.Add(yeniPuanKaydi);
                    }

                TempData["SuccessMessage"] = "Etkinlik başarıyla oluşturuldu ve puanınız güncellendi!";
                return RedirectToAction("Baslangic", "Home");
            }

            return View(model);
        }
    


    [HttpPost]
        public JsonResult GonderMesaj(int etkinlikID, string mesaj)
        {
            var gondericiID = (int)Session["ID"];

            if (string.IsNullOrEmpty(mesaj))
            {
                return Json(new { success = false, message = "Mesaj boş olamaz." });
            }

            try
            {
                using (var db = new Context())
                {
                    var yeniMesaj = new Mesajlar
                    {
                        GondericiID = gondericiID,
                        MesajMetni = mesaj,
                        GonderimZamani = DateTime.Now
                    };
                    db.Mesajlars.Add(yeniMesaj);
                    db.SaveChanges();

                    var aliciIDler = db.Katilimcilars
                        .Where(k => k.EtkinlikID == etkinlikID && k.KullaniciID != gondericiID)
                        .Select(k => k.KullaniciID)
                        .ToList();

                    foreach (var aliciID in aliciIDler)
                    {
                        var mesajAlici = new Mesajlar
                        {
                            GondericiID = gondericiID,
                            AliciID = aliciID, 
                            MesajMetni = mesaj,
                            GonderimZamani = DateTime.Now
                        };
                        db.Mesajlars.Add(mesajAlici); 
                    }

                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Message sent successfully!" }); // İngilizce mesaj
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while sending the message: " + ex.Message }); // İngilizce hata mesajı
            }
        }

    }
}