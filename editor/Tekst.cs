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
    public partial class Tekst : Form
    {
        public Tekst()
        {
            InitializeComponent();
            textBox1.Focus();
            fontDialog1 = new FontDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics graf = CreateGraphics();
            SizeF stringSize = new SizeF();
            stringSize = graf.MeasureString(textBox1.Text, fontDialog1.Font);
            graf.Dispose();
            Bitmap temp = new Bitmap((int)stringSize.Width+1, (int)stringSize.Height+1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            graf = Graphics.FromImage(temp);
            graf.DrawString(textBox1.Text, fontDialog1.Font, new SolidBrush(Kolor.KolorLewy), 0, 0);
            graf.Dispose();
            Referencja.warstwy.Add(new warstwa(temp));
            Referencja.narzedzie = new Przenoszenie(null);
            warstwa.Odswiez();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
        }


    }
}
