using Akıllı_Etkinlik_Planlama_Platformu.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using System.IO;
using Akıllı_Etkinlik_Planlama_Platformu.Helpers;
using System.Diagnostics;

namespace Akıllı_Etkinlik_Planlama_Platformu.Controllers
{
    public class LoginController : Controller
    {
        private readonly Random _random = new Random();
        Context c = new Context();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kayıt()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Kayıt(Kullanicilar ku, HttpPostedFileBase ProfileImage)
        {
            // Kullanıcı zaten var mı kontrol et
            var mevcutKullanici = c.Kullanicilars.FirstOrDefault(x => x.Eposta == ku.Eposta || x.KullaniciAdi == ku.KullaniciAdi);
            if (mevcutKullanici != null)
            {
                ViewBag.Hata = "Bu e-posta adresi ya da kullanıcı adı zaten kullanımda.";
                return View();  // Kullanıcıyı bilgilendir
            }

            // Profil fotoğrafı yüklendiyse, dosya yolunu belirleyip kaydet
            string profilFotoDosyaYolu = string.Empty;
            if (ProfileImage != null && ProfileImage.ContentLength > 0)
            {
                // Dosyanın adı ve yolu
                string dosyaAdi = Path.GetFileName(ProfileImage.FileName);
                string dosyaYolu = Path.Combine(Server.MapPath("~/Pictures"), dosyaAdi); // Pictures klasörüne kaydedilecek yol
                ProfileImage.SaveAs(dosyaYolu);  // Dosyayı kaydet

                profilFotoDosyaYolu = "/Pictures/" + dosyaAdi;  // Dosyanın web yolu
            }

            // Yeni kullanıcı oluştur
            ku.ProfilFotoDosyaYolu = profilFotoDosyaYolu;  // Fotoğrafın yolunu kaydet
            c.Kullanicilars.Add(ku);
            c.SaveChanges();

            // Başarı mesajı
            ViewBag.Basari = "Kayıt işleminiz başarılı!";

            return RedirectToAction("Index", "Login");  // Kullanıcıyı giriş sayfasına yönlendir
        }






        [HttpGet]
        public ActionResult KullaniciLogin1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KullaniciLogin1(string KullaniciAdi, string Sifre)
        {
            var bilgiler = c.Kullanicilars.FirstOrDefault(k => k.KullaniciAdi == KullaniciAdi && k.Sifre == Sifre);            
            if (bilgiler != null)
            {
                //UserSessionHelper.SetUserSession(bilgiler.KullaniciAdi, bilgiler.Sifre);
               
                FormsAuthentication.SetAuthCookie(bilgiler.KullaniciAdi, false);
                if (Session["Konum"] == null)
                {
                    Session["Konum"] = "Kocaeli Üniversitesi Umuttepe Kampüsü";
                }
                Session["ID"] = bilgiler.ID;


                Session["KullaniciAdi"] = bilgiler.KullaniciAdi.ToString();

                Session["IsAdmin"] = bilgiler.IsAdmin;
                var kullaniciPuan = c.Puanlars.FirstOrDefault(p => p.KullaniciID == bilgiler.ID);
                if (kullaniciPuan != null)
                {
                    Session["Puan"] = kullaniciPuan.PuanDeger; // Puanı session'a ekliyoruz
                }
                else
                {
                    Session["Puan"] = 0; // Eğer kullanıcıya ait puan yoksa, 0 olarak ayarlıyoruz
                }

                if (bilgiler.IsAdmin)
                {
                    return RedirectToAction("Baslangic", "Home");
                }
                else
                {
                    return RedirectToAction("Baslangic", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin( Kullanicilar k)
        {
            var bilgiler = c.Kullanicilars.FirstOrDefault(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);
            if(bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KullaniciAdi, false);
                if (Session["Konum"] == null)
                {
                    Session["Konum"] = "Kocaeli Üniversitesi Umuttepe Kampüsü";
                }
                Session["ID"] = bilgiler.ID;
                Session["KullaniciAdi"] = bilgiler.KullaniciAdi.ToString();
                Session["IsAdmin"] = bilgiler.IsAdmin;
                var kullaniciPuan = c.Puanlars.FirstOrDefault(p => p.KullaniciID == bilgiler.ID);
                if (kullaniciPuan != null)
                {
                    Session["Puan"] = kullaniciPuan.PuanDeger; // Puanı session'a ekliyoruz
                }
                else
                {
                    Session["Puan"] = 0; // Eğer kullanıcıya ait puan yoksa, 0 olarak ayarlıyoruz
                }
                if (bilgiler.IsAdmin)
                {
                    return View("Baslangic");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }



        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string Eposta)
        {
            if (string.IsNullOrEmpty(Eposta))
            {
                ViewBag.AlertMessage = "Lütfen bir e-posta adresi girin.";
                return View();
            }

            bool exists = ResetKullanici.CheckEmailExists(Eposta);
            if (exists)
            {
                string newPassword = ResetKullanici.GenerateRandomPassword();
                ResetKullanici.UpdatePassword(Eposta, newPassword);

                // E-posta gönderimi
                try
                {
                    ResetKullanici.SendResetMail(Eposta, newPassword);
                    ViewBag.AlertMessage = "Yeni şifreniz e-posta adresinize gönderildi.";
                }
                catch (Exception ex)
                {
                    ViewBag.AlertMessage = $"E-posta gönderilirken bir hata oluştu: {ex.Message}";
                }
            }
            else
            {
                ViewBag.AlertMessage = "E-posta adresi sistemde bulunamadı.";
            }

            return View();
        }

        public ActionResult Logout()
        {
            UserSessionHelper.ClearUserSession();
            Session["Konum"] = null;
            Session["Puan"] = null;
            Session["ID"] = null;
            Session["KullaniciAdi"] = null;  
            Session["IsAdmin"] = null; 
            FormsAuthentication.SignOut();   
            return RedirectToAction("Index", "Login"); 
        }

        public ActionResult Kullanicilarim()
        {
            var kullanicilarListesi = c.Kullanicilars.ToList();
            return View(kullanicilarListesi);
        }

        public ActionResult KullaniciSil(int id)
        {

            var kullanici = c.Kullanicilars.FirstOrDefault(k => k.ID == id);

            if (kullanici != null)
            {
                c.Kullanicilars.Remove(kullanici);
                c.SaveChanges();
            }
            return RedirectToAction("Kullanicilarim");

        }
       
    }
}