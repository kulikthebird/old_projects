using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Edytor
{
    /// <summary>
    /// Klasa Referencja zawiera zmienne potrzebne do działania programu oraz referencje do
    /// obiektu głównego okna programu.
    /// </summary>
    public partial class Referencja
    {
        public static List<warstwa> warstwy = new List<warstwa>();    // Warstwy
        public static PixelFormat formatObrazka;                      // Format obrazka
        public static bool Zaladowany;                                // Czy jest zaladowany jakis plik
        public static Przybornik narzedzie = new Przybornik();        // Przybornik
        public static warstwa Schowek;                                // Dodatkowa wartstwa zawierająca obrazek w schowku
        public static Edytor edytor;
    }

    /// <summary>
    /// Klasa warstwa, której obiekty są warstwami zawierającymi bitmapy.
    /// </summary>

    public class warstwa
    {

        /// <summary>
        /// Tworzy nową warstwę.
        /// </summary>
        /// <param name="mapka"></param>
        public warstwa(Bitmap mapka)
        {
            obrazek = new Bitmap(mapka.Width, mapka.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Referencja.edytor.CreateGraphics())
            {
                mapka.SetResolution(g.DpiX, g.DpiY);
            }
            Graphics lol = Graphics.FromImage(obrazek);
            lol.DrawImage(mapka, 0, 0);
            lol.Dispose();
            Width = obrazek.Width;
            Height = obrazek.Height;
            mapka.Dispose();
            if (licznik == 0)
            {
                Tlo = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Referencja.edytor.pictureBox1.Image = warstwa.Tlo;
                Referencja.edytor.pictureBox1.Visible = true;
                RozmarX = Width;
                RozmarY = Height;
                Referencja.edytor.panel1_Resize(null, null);

            }
            Pudelka.Add(pudelko = new Menu(ref obrazek, licznik==0 ? "Tło" : "warstwa"+(licznik+1), licznik));
            
            pudelko.Location = new Point(0, licznik * 35);

            for (int i = 0; i < warstwa.licznik; i++)
                Referencja.warstwy[i].pudelko.BackColor = Color.Silver;
            pudelko.BackColor = Color.Gray;

            Referencja.edytor.panel3.Controls.Add(pudelko);
            Referencja.edytor.panel3.Controls.SetChildIndex(pudelko, licznik + 2);

            Referencja.Zaladowany = true;

            aktualna = licznik++;
            Widoczny = true;
            Location = new Point(0, 0);

            Referencja.edytor.pictureBox1.Location = new Point(0, 0);
            Referencja.edytor.pictureBox1.Refresh();
        }

        /// <summary>
        /// Zmienia kolejność warstw.
        /// </summary>
        /// <param name="jeden"></param>
        /// <param name="dwa"></param>
        public static void ZmienKolejnosc(int jeden, int dwa)
        {
            if (jeden == dwa) return;
            warstwa temp = Referencja.warstwy[jeden];
            Referencja.warstwy[jeden] = Referencja.warstwy[dwa];
            Referencja.warstwy[dwa] = temp;
            Odswiez();
        }

        /// <summary>
        /// Zmienia rozmiar warstwy.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void ZmienRozmiar(int x, int y)
        {
            Bitmap temp = new Bitmap(Tlo);
            warstwa.Tlo = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics graf = Graphics.FromImage(Tlo);
            graf.DrawImage(temp, 0, 0);
            graf.Dispose();
            RozmarX = x;
            RozmarY = y;

            Referencja.edytor.pictureBox1.Image = Tlo;
            Odswiez();

        }

        /// <summary>
        /// Usuwa warstwę.
        /// </summary>
        public static void UsunWarstwe()
        {
            if(Referencja.Schowek != null)
            if (Referencja.warstwy[warstwa.aktualna].pudelko.Numer == Referencja.Schowek.pudelko.Numer)
                Referencja.Schowek = null;
            if (licznik == 0) return;

            Referencja.warstwy[warstwa.aktualna].pudelko.Dispose();
            Referencja.warstwy.RemoveAt(warstwa.aktualna);
            warstwa.Pudelka.RemoveAt(warstwa.aktualna);
            warstwa.licznik--;

            if (licznik == 0) Referencja.Zaladowany = false;

            for (int i = warstwa.aktualna; i < warstwa.licznik; i++)
            {
                Referencja.warstwy[i].pudelko.Location = new Point(0, Referencja.warstwy[i].pudelko.Location.Y - 35);
                Referencja.warstwy[i].pudelko.Numer--;
            }
            ZmianaAktualnejWarstwy(--warstwa.aktualna);
            warstwa.Odswiez();
        }

        /// <summary>
        /// Odmalowuje obraz.
        /// </summary>
        public static void Odswiez()
        {
            Graphics grafika = Graphics.FromImage(warstwa.Tlo);
            grafika.Clear(Color.Transparent);
            for (int i = 0; i < warstwa.licznik; i++)
            {
                Referencja.warstwy[i].pudelko.pictureBox1.Refresh();
                if(Referencja.warstwy[i].Widoczny == true)
                grafika.DrawImage(Referencja.warstwy[i].obrazek, Referencja.warstwy[i].Location);
            }
            grafika.Dispose();
            Referencja.edytor.pictureBox1.Refresh();
        }

        /// <summary>
        /// Zmienia numer aktualnie wybranej warstwy.
        /// </summary>
        /// <param name="index"></param>
        public static void ZmianaAktualnejWarstwy(int index)
        {
            aktualna = index;
            Referencja.narzedzie.ZmianaAktualnejWarstwy();
            for (int i = 0; i < warstwa.licznik; i++)
                Referencja.warstwy[i].pudelko.BackColor = Color.Silver;
            if(warstwa.licznik != 0)
                Referencja.warstwy[index].pudelko.BackColor = Color.Gray;
        }


        /// <summary>
        /// Zmienia stan widoczności warstwy (widoczna lub niewidoczna warstwa).
        /// </summary>
        /// <param name="value"></param>
        public void ZmienStan(bool value)
        {
            if (Widoczny == value) return;
            Widoczny = (bool)value;
            Odswiez();
        }

        public static void Koniec()
        {
            licznik = 0;
            aktualna = 0;
            Pudelka = new List<Menu>();
        }


        
        public static int licznik = 0;                          //Licznik warstw
        public static int aktualna = 0;                         //Numer aktualnej warstwy.
        public static List<Menu> Pudelka = new List<Menu>();    //Lista kontrolek w panelu Warstwy
        public static Bitmap Tlo;                               //Bitmapa zawierająca główny obraz (płótno)
        public static int RozmarX, RozmarY;                     //Rozmiary płótna
        public bool Widoczny;                                   //Stan widoczności
        public Point Location;                                  //Położenie wartswy na płótnie
        public Bitmap obrazek;                                  //Bitmapa warstwy
        public Menu pudelko;                                    //Kontrolka w panelu Warstwy
        public int Width, Height;                               //Szerokość i wysokość wartstwy
    }
}

