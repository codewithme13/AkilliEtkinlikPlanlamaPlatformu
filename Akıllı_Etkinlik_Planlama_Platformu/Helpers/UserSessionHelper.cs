using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akıllı_Etkinlik_Planlama_Platformu.Helpers
{
    public static class UserSessionHelper
    {
            private const string KullaniciAdiKey = "KullaniciAdi";
            private const string KullaniciSifreKey = "Sifre";

            // Kullanıcı adı ve şifreyi sakla
            public static void SetUserSession(string kullaniciAdi, string sifre)
            {
                HttpContext.Current.Session[KullaniciAdiKey] = kullaniciAdi;
                HttpContext.Current.Session[KullaniciSifreKey] = sifre;
            }

            // Kullanıcı adını al
            public static string GetKullaniciAdi()
            {
                return HttpContext.Current.Session[KullaniciAdiKey]?.ToString();
            }

            // Kullanıcı şifresini al
            public static string GetSifre()
            {
                return HttpContext.Current.Session[KullaniciSifreKey]?.ToString();
            }

            // Kullanıcı oturumu temizle
            public static void ClearUserSession()
            {
                HttpContext.Current.Session.Remove(KullaniciAdiKey);
                HttpContext.Current.Session.Remove(KullaniciSifreKey);
            }

            // Kullanıcı oturumu kontrol et
            public static bool IsUserLoggedIn()
            {
                return HttpContext.Current.Session[KullaniciAdiKey] != null;
            }
    }
}