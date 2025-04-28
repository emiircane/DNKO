using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;


namespace DNKO
{
    public partial class Kayıt_Giriş : Form
    {
        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");

        public Kayıt_Giriş()
        {
            InitializeComponent();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Kayıt_Giriş_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                string query = "SELECT oyuncuadi, sifre FROM kayit WHERE beni_hatirla = 1";
                SqlCommand komut = new SqlCommand(query, conn);
                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {
                    textBox1.Text = dr["oyuncuadi"].ToString();
                    textBox2.Text = dr["sifre"].ToString();
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Kayıt_Ol kayıtFormu = new Kayıt_Ol();
            kayıtFormu.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string oyuncuadi = textBox1.Text;
            string sifre = textBox2.Text;

            if (string.IsNullOrWhiteSpace(oyuncuadi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection baglanti = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;"))
                {
                    baglanti.Open();

                    string query = "SELECT * FROM kayıt WHERE oyuncuadi = @oyuncuadi AND sifre = @sifre";
                    using (SqlCommand komut = new SqlCommand(query, baglanti))
                    {
                        komut.Parameters.AddWithValue("@oyuncuadi", oyuncuadi);
                        komut.Parameters.AddWithValue("@sifre", sifre);

                        SqlDataReader dr = komut.ExecuteReader();

                        if (dr.Read())
                        {
                            dr.Close();

                            // Beni Hatırla Resetle
                            string resetQuery = "UPDATE kayıt SET beni_hatirla = 0";
                            SqlCommand resetCmd = new SqlCommand(resetQuery, baglanti);
                            resetCmd.ExecuteNonQuery();

                            // CheckBox işaretliyse sadece bu kullanıcı için 1 yap
                            if (checkBox1.Checked)
                            {
                                string updateQuery = "UPDATE kayıt SET beni_hatirla = 1 WHERE oyuncuadi = @oyuncuadi";
                                SqlCommand updateCmd = new SqlCommand(updateQuery, baglanti);
                                updateCmd.Parameters.AddWithValue("@oyuncuadi", oyuncuadi);
                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Giriş Başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Birinci_Bolum girisForm = new Birinci_Bolum();
                            girisForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Admin adminForm = new Admin();
            adminForm.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            hkdso hkdsForm = new hkdso();
            hkdsForm.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Sifremi_Unuttum sifremiUnuttumForm = new Sifremi_Unuttum();
            sifremiUnuttumForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }
    }
}

