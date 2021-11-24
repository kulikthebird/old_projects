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
    public partial class Rozmiary : Form
    {

        private int Wys = 0, Szer = 0;


        public Rozmiary()
        {
            InitializeComponent();
            Wys = warstwa.RozmarY;
            Szer = warstwa.RozmarX;
            textBox1.Text = Wys.ToString();
            textBox2.Text = Szer.ToString();
        }

        

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
            warstwa.ZmienRozmiar(Szer, Wys);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
