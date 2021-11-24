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
    public partial class Nowy : Form
    {
        public Nowy()
        {
            InitializeComponent();
        }

        private int Wys = 600, Szer = 800, RozX = 96, RozY = 96;

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 0) { textBox1.Text = Wys.ToString(); return; }
            int wartosc = Convert.ToInt32(textBox1.Text);
            if ((wartosc < 0) || (wartosc > 10000)) { textBox1.Text = Wys.ToString(); return; }
            Wys = wartosc;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength == 0) { textBox2.Text = Szer.ToString(); return; }
            int wartosc = Convert.ToInt32(textBox2.Text);
            if ((wartosc < 0) || (wartosc > 10000)) { textBox2.Text = Szer.ToString(); return; }
            Szer = wartosc;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.TextLength == 0) { textBox3.Text = RozX.ToString(); return; }
            int wartosc = Convert.ToInt32(textBox3.Text);
            if ((wartosc < 0) || (wartosc > 2000)) { textBox3.Text = RozX.ToString(); return; }
            RozX = wartosc;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.TextLength == 0) { textBox4.Text = RozY.ToString(); return; }
            int wartosc = Convert.ToInt32(textBox1.Text);
            if ((wartosc < 0) || (wartosc > 2000)) { textBox4.Text = RozY.ToString(); return; }
            RozY = wartosc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (warstwa.licznik != 0)
                if (MessageBox.Show("Wszystkie nie zapisane zmiany w aktualnym projekcie zostaną utracone. Czy chcesz kontynuować?", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.Warning).Equals(DialogResult.No)) return;


            Referencja.warstwy = new List<warstwa>();
            Bitmap bmp = new Bitmap(Szer, Wys, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp.SetResolution(RozX, RozY);
            Graphics graf = Graphics.FromImage(bmp);
            graf.Clear(Color.White);
            graf.Dispose();
            warstwa.Koniec();
            Referencja.warstwy.Add(new warstwa(bmp));
            warstwa.Odswiez();
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
