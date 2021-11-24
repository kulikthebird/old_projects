using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edytor
{
    public partial class Form2 : Form
    {
        public Bitmap bmp1, bmp2;
        public int ostatnia_jasnosc = 256;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany)
            {
                bmp1 = new Bitmap(Referencja.warstwy[warstwa.aktualna].obrazek, new Size(120,120));
                bmp2 = new Bitmap(bmp1);
                pictureBox1.Image = bmp2;
                textBox1.Text = "0";
                timer1.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Referencja.Jasnosc(trackBar1.Value - 256, Referencja.warstwy[warstwa.aktualna].obrazek);
            warstwa.Odswiez();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ostatnia_jasnosc != trackBar1.Value)
            {
                bmp2 = new Bitmap(bmp1);
                Referencja.Jasnosc(trackBar1.Value - 256, bmp2);
                pictureBox1.Image = bmp2;
                ostatnia_jasnosc = trackBar1.Value;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = (trackBar1.Value - 256).ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0) { textBox1.Text = (trackBar1.Value - 256).ToString(); return; }
            int wartosc = Convert.ToInt32(textBox1.Text);
            if ((wartosc < -256) || (wartosc > 256)) { textBox1.Text = (trackBar1.Value - 256).ToString(); return; }
            trackBar1.Value = wartosc+256;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }




    }
}
