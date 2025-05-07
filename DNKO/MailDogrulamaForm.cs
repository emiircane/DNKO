using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNKO
{
    public partial class MailDogrulamaForm: Form
    {
        public string GirilenKod { get; private set; }
        private string beklenenKod;
        private string kullaniciMail;
        private string kullaniciAdi;
        
        public MailDogrulamaForm()
        {
            InitializeComponent();
        }
        
        // Beklenen doğrulama kodu ve kullanıcı bilgileri için constructor
        public MailDogrulamaForm(string dogrulamaKodu, string mail, string oyuncuAdi)
        {
            InitializeComponent();
            this.beklenenKod = dogrulamaKodu;
            this.kullaniciMail = mail;
            this.kullaniciAdi = oyuncuAdi;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GirilenKod = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
