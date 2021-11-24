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
    public partial class Form5 : Form
    {
        public Bitmap bmp1, bmp2;
        public int Red = 0, Green = 0, Blue = 0;

        public Form5()
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
                textBox2.Text = "0";
                textBox3.Text = "0";
                timer1.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Referencja.Nasycenie_kolorow(Red, Green, Blue, Referencja.warstwy[warstwa.aktualna].obrazek);
            warstwa.Odswiez();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((Red != trackBar1.Value) || (Green != trackBar2.Value) || (Blue != trackBar3.Value))
            {
                bmp2 = new Bitmap(bmp1);

                Red = trackBar1.Value - 255;
                Green = trackBar2.Value - 255;
                Blue = trackBar3.Value - 255;
                
                Referencja.Nasycenie_kolorow(Red, Green, Blue, bmp2);
                pictureBox1.Image = bmp2;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = (trackBar1.Value - 255).ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = (trackBar2.Value - 255).ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = (trackBar3.Value - 255).ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0) { textBox1.Text = (trackBar1.Value - 255).ToString(); return; }
            int wartosc = Convert.ToInt32(textBox1.Text);
            if ((wartosc < 255) || (wartosc > 255)) return;
            trackBar1.Value = wartosc + 255;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength == 0) { textBox2.Text = (trackBar2.Value - 255).ToString(); return; }
            int wartosc = Convert.ToInt32(textBox2.Text);
            if ((wartosc < 255) || (wartosc > 255)) return;
            trackBar2.Value = wartosc + 255;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0) { textBox3.Text = (trackBar3.Value - 255).ToString(); return; }
            int wartosc = Convert.ToInt32(textBox3.Text);
            if ((wartosc < -255) || (wartosc > 255)) { return; }
            trackBar3.Value = wartosc + 255;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

    }
}
