using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Edytor
{
    /// <summary>
    /// Klasa kontrolek, które są wyświetlane w panelu Warstwy.
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu(ref Bitmap bmp, string nazwa, int numer)
        {
            InitializeComponent();
            pictureBox1.Image = bmp;
            label1.Text = nazwa;
            Numer = numer;
            checkBox1.Checked = true;
        }



        public void Klik(object sender, EventArgs e)
        {
            warstwa.ZmianaAktualnejWarstwy(Numer);
        }


        private int StanMyszy;
        private Point punkt, temp_Location;
        private int nowyX, nowyY;
        private int temp_Childindex;
        public int Numer;

        private void Menu_MouseDown(object sender, MouseEventArgs e)
        {
            StanMyszy = 1;
            punkt = e.Location;
            temp_Location = this.Location;
            nowyX = Location.X + e.Location.X;
            nowyY = Location.Y + e.Location.Y;
            temp_Childindex = Referencja.edytor.panel3.Controls.GetChildIndex(Referencja.warstwy[Numer].pudelko);
            Referencja.edytor.panel3.Controls.SetChildIndex(Referencja.warstwy[Numer].pudelko, 1);
        }

        private void Menu_MouseMove(object sender, MouseEventArgs e)
        {
            if (StanMyszy == 0) return;
            nowyX = Location.X + e.Location.X - punkt.X;
            nowyY = Location.Y + e.Location.Y - punkt.Y;
            if ((nowyX > -40) && (nowyX < Referencja.edytor.panel3.Width - 110))
                Location = new Point(nowyX, Location.Y);
            if ((nowyY > -20) && (nowyY < Referencja.edytor.panel3.Height - 20))
                Location = new Point(Location.X, nowyY);
        }

        private void Menu_MouseUp(object sender, MouseEventArgs e)
        {
            StanMyszy = 0;
            int index = Numer + (nowyY - temp_Location.Y) / 35;
            if (index > warstwa.licznik - 1) index = warstwa.licznik - 1;
            if (index < 0) index = 0;
            Location = new Point(0, Referencja.warstwy[index].pudelko.Location.Y);
            Referencja.warstwy[index].pudelko.Location = new Point(0, temp_Location.Y);

            Referencja.warstwy[index].pudelko.Numer = Numer;
            Numer = index;
            Referencja.edytor.panel3.Controls.SetChildIndex(Referencja.warstwy[Numer].pudelko, temp_Childindex);
            warstwa.ZmienKolejnosc(Referencja.warstwy[index].pudelko.Numer, index);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Numer < warstwa.licznik)
                Referencja.warstwy[Numer].ZmienStan(checkBox1.CheckState == CheckState.Checked ? true : false);
        }

    }
}
