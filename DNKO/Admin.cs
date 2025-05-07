using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DNKO
{
    public partial class Admin : Form
    {
        private string adminKullaniciAdi = "admin";
        private string adminSifre = "12345";

        SqlConnection conn = new SqlConnection(@"Server=DESKTOP-LC5PILU\SQLEXPRESS;Database=kullanicilar;Integrated Security=True;TrustServerCertificate=True;");

        public Admin()
        {
            InitializeComponent();

            tabControl1.SelectedTab = tabPage1;
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

            // Mail logları butonunu kaldırdık, PictureBox kullanacağız
        }

        // PictureBox için mail logları görüntüleme metodu
        private void pictureBoxMailLogs_Click(object sender, EventArgs e)
        {
            MailLoglariniGoster();
        }

        // Mail logları için yeni form gösterme
        private void MailLoglariniGoster()
        {
            Form mailLogForm = new Form();
            mailLogForm.Text = "Mail Logları";
            mailLogForm.Size = new Size(800, 500);
            mailLogForm.StartPosition = FormStartPosition.CenterScreen;

            DataGridView dgvMailLogs = new DataGridView();
            dgvMailLogs.Dock = DockStyle.Fill;
            dgvMailLogs.AllowUserToAddRows = false;
            dgvMailLogs.AllowUserToDeleteRows = false;
            dgvMailLogs.ReadOnly = true;
            dgvMailLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            mailLogForm.Controls.Add(dgvMailLogs);

            try
            {
                string query = "SELECT id, alici_mail, mail_konusu, LEFT(mail_metni, 100) AS mail_ozeti, gonderim_tarihi FROM mail_logs ORDER BY gonderim_tarihi DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvMailLogs.DataSource = dt;

                // Mail içeriğini görüntülemek için çift tıklama olayı ekle
                dgvMailLogs.CellDoubleClick += (s, args) =>
                {
                    if (args.RowIndex >= 0)
                    {
                        int mailId = Convert.ToInt32(dgvMailLogs.Rows[args.RowIndex].Cells["id"].Value);
                        MailDetayGoster(mailId);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail logları yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mailLogForm.ShowDialog();
        }

        // Mail detaylarını gösterme
        private void MailDetayGoster(int mailId)
        {
            try
            {
                conn.Open();

                string query = "SELECT alici_mail, mail_konusu, mail_metni, gonderim_tarihi FROM mail_logs WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", mailId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string aliciMail = reader["alici_mail"].ToString();
                    string mailKonusu = reader["mail_konusu"].ToString();
                    string mailMetni = reader["mail_metni"].ToString();
                    DateTime gonderimTarihi = Convert.ToDateTime(reader["gonderim_tarihi"]);

                    Form detayForm = new Form();
                    detayForm.Text = "Mail Detayı";
                    detayForm.Size = new Size(500, 400);
                    detayForm.StartPosition = FormStartPosition.CenterScreen;

                    TableLayoutPanel panel = new TableLayoutPanel();
                    panel.Dock = DockStyle.Fill;
                    panel.ColumnCount = 2;
                    panel.RowCount = 4;
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));

                    // Alıcı
                    panel.Controls.Add(new Label() { Text = "Alıcı:", Dock = DockStyle.Fill }, 0, 0);
                    panel.Controls.Add(new TextBox() { Text = aliciMail, Dock = DockStyle.Fill, ReadOnly = true }, 1, 0);

                    // Konu
                    panel.Controls.Add(new Label() { Text = "Konu:", Dock = DockStyle.Fill }, 0, 1);
                    panel.Controls.Add(new TextBox() { Text = mailKonusu, Dock = DockStyle.Fill, ReadOnly = true }, 1, 1);

                    // Tarih
                    panel.Controls.Add(new Label() { Text = "Tarih:", Dock = DockStyle.Fill }, 0, 2);
                    panel.Controls.Add(new TextBox() { Text = gonderimTarihi.ToString(), Dock = DockStyle.Fill, ReadOnly = true }, 1, 2);

                    // İçerik
                    panel.Controls.Add(new Label() { Text = "İçerik:", Dock = DockStyle.Fill }, 0, 3);
                    TextBox txtIcerik = new TextBox()
                    {
                        Text = mailMetni,
                        Dock = DockStyle.Fill,
                        ReadOnly = true,
                        Multiline = true,
                        ScrollBars = ScrollBars.Vertical
                    };
                    panel.Controls.Add(txtIcerik, 1, 3);

                    detayForm.Controls.Add(panel);
                    detayForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Mail bulunamadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail detayları görüntülenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage2 && tabControl1.SelectedTab != tabPage2)
            {
                e.Cancel = true;
            }
        }

        private void VerileriGuncelle()
        {
            string query = "SELECT ad, soyad, oyuncuadi, sifre, mail FROM kayıt";

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox3.Text = row.Cells["ad"].Value.ToString();
                textBox4.Text = row.Cells["soyad"].Value.ToString();
                textBox5.Text = row.Cells["oyuncuadi"].Value.ToString();
                textBox6.Text = row.Cells["sifre"].Value.ToString();
                textBox7.Text = row.Cells["mail"].Value.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text == adminKullaniciAdi && txtSifre.Text == adminSifre)
            {
                MessageBox.Show("Giriş başarılı!");
                tabControl1.SelectedTab = tabPage2;
                VerileriGuncelle();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new Kayıt_Giriş().Show();
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            VerileriGuncelle();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!");
                return;
            }

            string eskiOyuncuAdi = dataGridView1.SelectedRows[0].Cells["oyuncuadi"].Value.ToString();
            string yeniAd = textBox3.Text;
            string yeniSoyad = textBox4.Text;
            string yeniOyuncuAdi = textBox5.Text;
            string yeniSifre = textBox6.Text;
            string yeniMail = textBox7.Text;

            string query = "UPDATE kayıt SET ad = @ad, soyad = @soyad, oyuncuadi = @oyuncuadi, sifre = @sifre, mail = @mail WHERE oyuncuadi = @eskiOyuncuadi";

            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ad", yeniAd);
                    cmd.Parameters.AddWithValue("@soyad", yeniSoyad);
                    cmd.Parameters.AddWithValue("@oyuncuadi", yeniOyuncuAdi);
                    cmd.Parameters.AddWithValue("@sifre", yeniSifre);
                    cmd.Parameters.AddWithValue("@mail", yeniMail);
                    cmd.Parameters.AddWithValue("@eskiOyuncuadi", eskiOyuncuAdi);

                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show(result > 0 ? "Güncelleme başarılı!" : "Güncelleme başarısız!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
            finally
            {
                conn.Close();
                VerileriGuncelle();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!");
                return;
            }

            string oyuncuadi = dataGridView1.SelectedRows[0].Cells["oyuncuadi"].Value.ToString();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM kayıt WHERE oyuncuadi = @oyuncuadi", conn);
                cmd.Parameters.AddWithValue("@oyuncuadi", oyuncuadi);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Kayıt silindi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası: " + ex.Message);
            }
            finally
            {
                conn.Close();
                VerileriGuncelle();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            new Kayıt_Giriş().Show();
            this.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text Files (*.txt)|*.txt";
            save.Title = "Verileri Dışa Aktar";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(save.FileName))
                    {
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            sw.Write(dataGridView1.Columns[i].HeaderText);
                            if (i < dataGridView1.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue;
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                sw.Write(row.Cells[i].Value?.ToString());
                                if (i < dataGridView1.Columns.Count - 1)
                                    sw.Write("\t");
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show("Veriler başarıyla dışa aktarıldı!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dışa aktarma hatası: " + ex.Message);
                }
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
