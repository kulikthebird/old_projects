using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Edytor
{

    /// <summary>
    /// Klasa głównego okna programu.
    /// </summary>

    public partial class Edytor : Form
    {

        public Edytor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ustawia obsługę podwójnego buforowania oraz wyświetla powitanie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Edytor_Load(object sender, EventArgs e)
        {
            Referencja.edytor = this;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                        ControlStyles.DoubleBuffer, true);

            Powitanie okno = new Powitanie();
            okno.Show();
        }


        ///////
        /////// Obsługa zdarzeń myszy dla picturebox'a.
        ///////
        ///////
        
        public void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(Referencja.narzedzie.aktywny)  Referencja.narzedzie.Zdarzenie(2, e);
        }
        
        public void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Referencja.narzedzie.aktywny) Referencja.narzedzie.Zdarzenie(1, e);
        }

        public void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Referencja.narzedzie.aktywny) Referencja.narzedzie.Zdarzenie(3, e);
        }


        ///////
        /////// Obsługa zmiany szerokości głównego panelu.
        ///////
        ///////

        public void panel1_Resize(object sender, EventArgs e)
        {
            if (warstwa.RozmarY < panel1.Height && warstwa.RozmarX < panel1.Width)
            {
                pictureBox1.Dock = DockStyle.Fill;
            }
            else
            {
                pictureBox1.Dock = DockStyle.None;
            }
        }
        

        /// <summary>
        /// Wywoływana, gdy zachodzi potrzeba odmalowania picturebox'a.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Referencja.narzedzie.aktywny) Referencja.narzedzie.Paint(e);
        }

    }
}