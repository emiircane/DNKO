using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DNKO
{
    public partial class hkdso : Form
    {
        public hkdso()
        {
            InitializeComponent();
        }

        private void girişEkranıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kayıt_Giriş girisFormu = new Kayıt_Giriş();
            girisFormu.Show();
            this.Close();
        }

        private void uygulamayıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcının girdiği bilgileri al
                string kullaniciMail = textBox1.Text.Trim(); // Kullanıcı e-posta
                string mesaj = richTextBox1.Text.Trim();     // Kullanıcının mesajı

                // Boş alan kontrolü
                if (string.IsNullOrWhiteSpace(kullaniciMail) || string.IsNullOrWhiteSpace(mesaj))
                {
                    MessageBox.Show("Lütfen e-posta ve mesaj alanlarını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yönetici e-posta adresi
                string yoneticiMail = "kediogluneco@gmail.com"; // Yönetici e-postasını gir

                // Mail mesajı oluştur
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(kullaniciMail); // Gönderen olarak kullanıcı e-postası
                mail.To.Add(yoneticiMail); // Yöneticiye gönder
                mail.Subject = "Yeni Dilek / Şikayet Formu";
                mail.Body = $"Gönderen: {kullaniciMail}\n\nMesaj:\n{mesaj}";

                // SMTP istemcisi oluştur
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("kediogluneco@gmail.com", "cedr gcab ukyv nuct"); // Yönetici e-posta bilgileri
                smtp.EnableSsl = true;

                // E-postayı gönder
                smtp.Send(mail);

                MessageBox.Show("Mesajınız başarıyla gönderildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Alanları temizle
                textBox1.Clear();
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesaj gönderilemedi! Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new Kayıt_Giriş().Show();
            this.Close();
        }
    }
}
