using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DNKO
{
    public partial class Kayıt_Ol : Form
    {
        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");
        string resimDosyaYolu = "";

        public Kayıt_Ol()
        {
            InitializeComponent();
        }

        // 🔸 Avatar seçme
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bir resim seçin";
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                resimDosyaYolu = ofd.FileName;
                pictureBox1.Image = Image.FromFile(resimDosyaYolu);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            string oyuncuadi = textBox3.Text;
            string sifre = textBox4.Text;
            string mail = textBox5.Text;

            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad) || string.IsNullOrWhiteSpace(oyuncuadi) ||
                string.IsNullOrWhiteSpace(sifre) || string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(resimDosyaYolu))
            {
                MessageBox.Show("Tüm alanları ve resmi doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(sifre, "^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{8,}$"))
            {
                MessageBox.Show("Şifre en az 8 karakter, bir büyük harf, bir küçük harf ve özel karakter içermelidir!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kodAd = oyuncuadi.Length >= 3 ? oyuncuadi.Substring(0, 3) : oyuncuadi;
            string kodSoyad = soyad.Length >= 2 ? soyad.Substring(soyad.Length - 2) : soyad;
            string dogrulamaKodu = $"{kodAd}.{kodSoyad}2025".ToUpper();

            MailGonderici gonderici = new MailGonderici();
            gonderici.DogrulamaKoduGonder(mail, dogrulamaKodu);

            // Yeni yapılandırıcıyı kullanarak MailDogrulamaForm'u başlat
            MailDogrulamaForm dogrulamaFormu = new MailDogrulamaForm(dogrulamaKodu, mail, oyuncuadi);
            if (dogrulamaFormu.ShowDialog() == DialogResult.OK)
            {
                if (dogrulamaFormu.GirilenKod == dogrulamaKodu)
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        string query = "INSERT INTO kayıt (ad, soyad, oyuncuadi, sifre, mail, avatar) VALUES (@ad, @soyad, @oyuncuadi, @sifre, @mail, @avatar)";
                        using (SqlCommand komut = new SqlCommand(query, conn))
                        {
                            komut.Parameters.AddWithValue("@ad", ad);
                            komut.Parameters.AddWithValue("@soyad", soyad);
                            komut.Parameters.AddWithValue("@oyuncuadi", oyuncuadi);
                            komut.Parameters.AddWithValue("@sifre", sifre);
                            komut.Parameters.AddWithValue("@mail", mail);
                            komut.Parameters.AddWithValue("@avatar", resimDosyaYolu);

                            int sonuc = komut.ExecuteNonQuery();

                            if (sonuc > 0)
                            {
                                // Kayıt başarılı olduğunda bilgilendirme e-postası gönder
                                try
                                {
                                    MailGonderici mailGonderici = new MailGonderici();
                                    mailGonderici.KayitBasariliMailiGonder(mail, oyuncuadi);
                                }
                                catch (Exception mailEx)
                                {
                                    // E-posta gönderilemezse bile kayıt işlemi tamamlanacaktır
                                    MessageBox.Show("Kayıt tamamlandı ancak bilgilendirme e-postası gönderilemedi: " + mailEx.Message,
                                                  "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                MessageBox.Show("Kayıt başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                                new Kayıt_Giriş().Show();
                            }
                            else
                            {
                                MessageBox.Show("Kayıt oluşturulamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Doğrulama kodu hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            new Kayıt_Giriş().Show();
            this.Close();
        }
    }
}
