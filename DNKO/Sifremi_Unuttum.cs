using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DNKO
{
    public partial class Sifremi_Unuttum: Form
    {

        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");
        private string dogrulamaKodu;
        private string girilenMail;
        private int kalanSure = 60;
        private System.Windows.Forms.Timer geriSayimTimer = new System.Windows.Forms.Timer();
        public Sifremi_Unuttum()
        {
            InitializeComponent();

            // Başlangıçta kod kutusu ve onay butonu devre dışı
            textBox2.Enabled = false;
            pictureBox2.Enabled = false;

            // Timer ayarı
            geriSayimTimer.Interval = 1000;
            geriSayimTimer.Tick += GeriSayimTimer_Tick;
        }

        private bool MailVarMi(string mail)
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM kayıt WHERE mail = @mail";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@mail", mail);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        private string KodUret()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            girilenMail = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(girilenMail))
            {
                MessageBox.Show("Lütfen e-posta adresinizi girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!MailVarMi(girilenMail))
            {
                MessageBox.Show("Bu e-posta ile kayıtlı kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dogrulamaKodu = KodUret();
            MailGonderici mailGonderici = new MailGonderici();
            mailGonderici.DogrulamaKoduGonder(girilenMail, dogrulamaKodu);

            // Kod alanlarını aktif et
            textBox2.Enabled = true;
            pictureBox2.Enabled = true;

            // Butonu disable et
            pictureBox1.Enabled = false;
            kalanSure = 60;
            geriSayimTimer.Start();

            MessageBox.Show("Doğrulama kodu gönderildi!\nKodun geçerlilik süresi 60 saniyedir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GeriSayimTimer_Tick(object sender, EventArgs e)
        {
            kalanSure--;

            if (kalanSure <= 0)
            {
                geriSayimTimer.Stop();
                pictureBox1.Enabled = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string girilenKod = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(girilenKod))
            {
                MessageBox.Show("Doğrulama kodunu girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (girilenKod == dogrulamaKodu)
            {
                SifreYenilemeEkrani sifreForm = new SifreYenilemeEkrani("", girilenMail);
                sifreForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Doğrulama kodu yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
