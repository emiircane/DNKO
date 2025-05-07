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
using Microsoft.Data.SqlClient;

namespace DNKO
{
    public partial class hkdso : Form
    {
        // SQL bağlantısı
        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");
        
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
                
                // Veritabanına kaydedelim
                KayitEkle(kullaniciMail, mesaj);

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
        
        // Veritabanına kayıt ekleme metodu
        private void KayitEkle(string email, string mesaj)
        {
            try
            {
                // Eğer mail_logs tablosu yoksa oluşturalım
                // (Bu kodu çalıştırdıktan sonra bu fonksiyonu bir kere çalıştırınız, sonra bu kod bloğunu silebilirsiniz)
                /*
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                    
                string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'mail_logs')
                BEGIN
                    CREATE TABLE mail_logs (
                        id INT IDENTITY(1,1) PRIMARY KEY, 
                        alici_mail NVARCHAR(100), 
                        mail_metni NVARCHAR(MAX), 
                        mail_konusu NVARCHAR(100), 
                        gonderim_tarihi DATETIME DEFAULT GETDATE()
                    )
                END";
                
                SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                createCmd.ExecuteNonQuery();
                
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                */
                
                // Veritabanına ekleme işlemi
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "INSERT INTO mail_logs (alici_mail, mail_konusu, mail_metni) VALUES (@aliciMail, @mailKonusu, @mailMetni)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@aliciMail", email);
                    cmd.Parameters.AddWithValue("@mailKonusu", "Dilek / Şikayet / Öneri");
                    cmd.Parameters.AddWithValue("@mailMetni", mesaj);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına kayıt sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
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
