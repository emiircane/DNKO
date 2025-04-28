
namespace DNKO
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        bool islem = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!islem)
            {
                this.Opacity += 0.009;
                if (this.Opacity >= 1.0)
                {
                    islem = true;
                }
            }
            else
            {
                this.Opacity -= 0.009;
                if (this.Opacity <= 0)
                {
                    this.Opacity = 0;
                    timer1.Stop();

                    Kayýt_Giriþ getir = new Kayýt_Giriþ();
                    getir.Show();
                    getir.FormClosed += (s, args) => Application.Exit();

                    this.Hide();
                }
            }
        }
    }
}
