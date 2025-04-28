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
using Microsoft.Data.SqlClient;

namespace DNKO
{
    public partial class SifreYenilemeEkrani : Form
    {
        private string mail;
        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");

        public SifreYenilemeEkrani(string oyuncuAdi, string mail)
        {
            InitializeComponent();
            this.mail = mail;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string yeniSifre1 = textBox1.Text.Trim();
            string yeniSifre2 = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(yeniSifre1) || string.IsNullOrEmpty(yeniSifre2))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (yeniSifre1 != yeniSifre2)
            {
                MessageBox.Show("Lütfen şifreleri aynı girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "UPDATE kayıt SET sifre = @sifre WHERE mail = @mail";
                using (SqlCommand komut = new SqlCommand(query, conn))
                {
                    komut.Parameters.AddWithValue("@sifre", yeniSifre1);
                    komut.Parameters.AddWithValue("@mail", mail);

                    int result = komut.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Şifreniz başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new Kayıt_Giriş().Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Şifre güncellenemedi. Lütfen tekrar deneyin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new Kayıt_Giriş().Show();
            this.Close();
        }
    }
}
