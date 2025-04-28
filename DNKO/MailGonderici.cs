using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DNKO
{
    class MailGonderici
    {
        private string gonderenMail = "kediogluneco@gmail.com";
        private string sifre = "cedr gcab ukyv nuct"; // Uygulama şifresi

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
                MessageBox.Show("Doğrulama kodu e-posta adresinize gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
