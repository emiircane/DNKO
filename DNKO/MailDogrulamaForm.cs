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
        public MailDogrulamaForm()
        {
            InitializeComponent();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            GirilenKod = textBox1.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
