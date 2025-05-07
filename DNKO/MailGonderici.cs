using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DNKO
{
    class MailGonderici
    {
        private string gonderenMail = "kediogluneco@gmail.com";
        private string sifre = "cedr gcab ukyv nuct"; // Uygulama şifresi
        
        // SQL bağlantı stringi
        private SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");

        // Mail bilgilerini veritabanına kaydeden metot
        private void MailBilgisiKaydet(string aliciMail, string mailKonusu, string mailMetni)
        {
            try 
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "INSERT INTO mail_logs (alici_mail, mail_konusu, mail_metni) VALUES (@aliciMail, @mailKonusu, @mailMetni)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@aliciMail", aliciMail);
                    cmd.Parameters.AddWithValue("@mailKonusu", mailKonusu);
                    cmd.Parameters.AddWithValue("@mailMetni", mailMetni);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail kaydı oluşturulamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void DogrulamaKoduGonder(string aliciMail, string dogrulamaKodu)
        {
            if (string.IsNullOrWhiteSpace(aliciMail))
            {
                MessageBox.Show("Lütfen geçerli bir e-posta adresi girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mailMessage.From = new MailAddress(gonderenMail);
                mailMessage.To.Add(aliciMail);
                mailMessage.Subject = "Giriş Doğrulama Kodu";
                mailMessage.Body = $"Merhaba,\n\nGiriş yapabilmek için doğrulama kodunuz: {dogrulamaKodu}\n\nİyi günler dileriz.";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential(gonderenMail, sifre);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mailMessage);
                
                // Mail bilgilerini veritabanına kaydet
                MailBilgisiKaydet(aliciMail, mailMessage.Subject, mailMessage.Body);
                
                MessageBox.Show("Doğrulama kodu e-posta adresinize gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void SifreDegistirildiMailiGonder(string aliciMail)
        {
            if (string.IsNullOrWhiteSpace(aliciMail))
            {
                MessageBox.Show("Geçerli bir e-posta adresi bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mailMessage.From = new MailAddress(gonderenMail);
                mailMessage.To.Add(aliciMail);
                mailMessage.Subject = "Şifre Değişikliği Bildirimi";
                mailMessage.Body = "Merhaba,\n\nŞifreniz başarılı bir şekilde değiştirilmiştir. Sisteme yeni şifreniz ile giriş yapabilirsiniz.\n\nİyi günler dileriz.";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential(gonderenMail, sifre);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mailMessage);
                
                // Mail bilgilerini veritabanına kaydet
                MailBilgisiKaydet(aliciMail, mailMessage.Subject, mailMessage.Body);
                
                MessageBox.Show("Şifre değişikliği bildirimi e-posta adresinize gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bildirim e-postası gönderilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool KayitBasariliMailiGonder(string aliciMail, string kullaniciAdi)
        {
            if (string.IsNullOrWhiteSpace(aliciMail))
            {
                MessageBox.Show("Geçerli bir e-posta adresi bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // E-posta göndermeye başlarken bilgi mesajı
                Console.WriteLine($"Kayıt başarılı e-postası gönderiliyor: {aliciMail}");
                
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mailMessage.From = new MailAddress(gonderenMail);
                mailMessage.To.Add(aliciMail);
                mailMessage.Subject = "Kayıt İşleminiz Tamamlandı";
                mailMessage.Body = $"Merhaba {kullaniciAdi},\n\nKaydınız başarılı bir şekilde tamamlanmıştır. Sisteme giriş yapabilirsiniz.\n\nİyi günler dileriz.";

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential(gonderenMail, sifre);
                smtpServer.EnableSsl = true;

                smtpServer.Send(mailMessage);
                
                // Mail bilgilerini veritabanına kaydet
                MailBilgisiKaydet(aliciMail, mailMessage.Subject, mailMessage.Body);
                
                // Başarılı mesajı göster
                MessageBox.Show("Kayıt başarılı bildirim e-postası adresinize gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt bildirimi e-postası gönderilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
