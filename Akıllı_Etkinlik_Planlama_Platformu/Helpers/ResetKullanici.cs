using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Akıllı_Etkinlik_Planlama_Platformu.Helpers
{
    public static class ResetKullanici
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=your_database_name;Username=your_username;Password=your_password;";

        // E-posta adresinin var olup olmadığını kontrol eden metod
        public static bool CheckEmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM dbo.\"Kullanicilars\"  WHERE \"Eposta\"  = @Email"; // Eposta kontrolü
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Şifreyi güncelleyen metod
        public static void UpdatePassword(string email, string newPassword)
        {
            string query = "UPDATE dbo.\"Kullanicilars\" SET \"Sifre\"  = @NewPassword WHERE \"Eposta\" = @Email"; // Sifre güncelleme
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword); // Yeni şifreyi al
                cmd.Parameters.AddWithValue("@Email", email); // Eposta ile sorgulama yap
                cmd.ExecuteNonQuery(); // Veritabanını güncelle
            }
        }

        // Şifre sıfırlama e-postası gönderen metod
        public static void SendResetMail(string email, string newPassword)
        {
            using (MailMessage mail = new MailMessage())
            {
                string smtpUser = "EMAİL ADRESINIZ"; // Gmail hesabınız
                string smtpPass = "EMAİL UYGULAMA ŞİFRENİZ"; // Gmail uygulama şifreniz

                mail.From = new MailAddress(smtpUser, "Şifre Sıfırlama");
                mail.To.Add(email);
                mail.Subject = "Şifreniz Sıfırlandı";
                mail.Body = $"Merhaba,\n\nYeni şifreniz: {newPassword}\n\nSisteme giriş yapabilirsiniz.";
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) // Gmail SMTP server
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;
                    smtp.Send(mail); // Mail gönderimi
                }
            }
        }

        // Rastgele yeni şifre üreten metod
        public static string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"; // Geçerli karakterler
            Random random = new Random();
            return new string(Enumerable.Repeat(validChars, 10) // 10 karakterli rastgele şifre
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray());
        }
    }
}