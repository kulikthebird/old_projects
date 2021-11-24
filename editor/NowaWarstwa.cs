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
    public partial class NowaWarstwa : Form
    {
        public NowaWarstwa()
        {
            InitializeComponent();
        }

        private int Wys = 0, Szer = 0;

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


        private void button1_Click(object sender, EventArgs e)
        {
            if (warstwa.licznik != 0)
                if (MessageBox.Show("Uwaga", "Wszystkie nie zapisane zmiany w poprzednim projekcie zostaną utracone. Czy chcesz kontynuować?", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
                {
                    Referencja.warstwy = new List<warstwa>();
                    Bitmap bmp = new Bitmap(Szer, Wys, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    Graphics graf = Graphics.FromImage(bmp);
                    graf.Clear(Color.White);
                    graf.Dispose();
                    Referencja.warstwy.Add(new warstwa(bmp));
                    warstwa.Odswiez();
                }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
