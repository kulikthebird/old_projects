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
    public partial class Form4 : Form
    {
        public Bitmap bmp1, bmp2;
        public int ostatnia_jasnosc = 0;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany)
            {
                bmp1 = new Bitmap(Referencja.warstwy[warstwa.aktualna].obrazek, new Size(120, 120));
                bmp2 = new Bitmap(bmp1);
                pictureBox1.Image = bmp2;
                textBox1.Text = "0";
                timer1.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < trackBar1.Value; i++)
                Referencja.Wyostrzanie(Referencja.warstwy[warstwa.aktualna].obrazek);
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

                for (int i = 0; i < trackBar1.Value; i++)
                    Referencja.Wyostrzanie(bmp2);

                pictureBox1.Image = bmp2;
                ostatnia_jasnosc = trackBar1.Value;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0) { textBox1.Text = trackBar1.Value.ToString(); return; }
            int wartosc = Convert.ToInt32(textBox1.Text);
            if ((wartosc < 0 || (wartosc > 10))) { textBox1.Text = trackBar1.Value.ToString(); return; }
            trackBar1.Value = wartosc;
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
