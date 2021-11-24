using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Edytor
{
    partial class Edytor
    {
        public string glowna_sciezka;                   // Zawiera ścieżkę do głównego pliku (warstwa "Tło"), w którym będą zapisywane zmiany.
        public ImageFormat format;                      // Zawiera format głównego pliku.


        /// <summary>
        /// Pobiera bitmapę z pliku oraz tworzy nową wartstwę
        /// </summary>
        /// <param name="sciezka"></param>

         public void PobierzZPliku(string sciezka)
         {
             Bitmap temp;
             try
             {
                 temp = new Bitmap(sciezka);
             }
             catch
             {
                 MessageBox.Show("Błąd", "Nie można otworzyć pliku.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
             }
             format = temp.RawFormat;
             Referencja.warstwy.Add(new warstwa(temp));
             if (warstwa.licznik == 1) glowna_sciezka = sciezka;
             warstwa.Odswiez();
         }

        /// <summary>
        /// Zapisuje zmiany do nowego pliku graficznego.
        /// </summary>

         public void Zapisz_jako()
         {
             if (DialogResult.Equals(savefile.ShowDialog(), DialogResult.OK))
             {

                 ImageFormat format = new ImageFormat(new Guid());
                 switch (savefile.FilterIndex)
                 {
                     case 1: format = ImageFormat.Bmp; break;
                     case 2: format = ImageFormat.Jpeg; break;
                     case 3: format = ImageFormat.Png; break;
                     case 4: format = ImageFormat.Gif; break;
                     case 5: format = ImageFormat.Tiff; break;
                 }
                 if (Steganograf.sciezka2 != null) Steganograf.Zapis();
                 {
                     try
                     {
                         warstwa.Tlo.Save(savefile.FileName, format);
                     }
                     catch
                     {
                         MessageBox.Show("Nieoczekiwany błąd podczas próby zapisu do pliku.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return;
                     }
                 }
             }
             

         }

        /// <summary>
        /// Zapisuje zmiany do głównego pliku graficznego.
        /// </summary>

         public void Zapisz()
         {
             if (glowna_sciezka == null) return;
             if (Steganograf.sciezka2 != null) Steganograf.Zapis();
             try
             {
                 warstwa.Tlo.Save(glowna_sciezka, format);
             }
             catch
             {
                 MessageBox.Show("Nieoczekiwany błąd podczas próby zapisu do pliku.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
             }
         }

        /// <summary>
        /// Wyświetla okno dialogowe, a następnie otwiera plik graficzny.
        /// </summary>

        public void Otworz()
        {
            if (DialogResult.Equals(openfile.ShowDialog(), DialogResult.OK))
            {
                PobierzZPliku(openfile.FileName);
            }
        }

         /////
         ///// Obsługa menu głównego okna
         /////
         /////

        private void jasnośćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Form2 okno = new Form2();
            okno.Show();
        }


        private void rozmazanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Form3 okno = new Form3();
            okno.Show();
        }

        private void wyostrzanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Form4 okno = new Form4();
            okno.Show();
        }

        private void nasycenieBarwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Form5 okno = new Form5();
            okno.Show();
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Otworz();
        }

        private void nowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nowy okno = new Nowy();
            okno.Show();
        }


        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany == false) return;
            Zapisz();
        }

        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany == false) return;
            Zapisz_jako();
        }

        private void szarośćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany == false) return;
            Referencja.Szarosc(Referencja.warstwy[warstwa.aktualna].obrazek);
            warstwa.Odswiez();
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Referencja.Zaladowany == false) return;
            Referencja.Sepia(Referencja.warstwy[warstwa.aktualna].obrazek);
            warstwa.Odswiez();
        }

        private void nowaWarstwaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            NowaWarstwa okno = new NowaWarstwa();
            okno.Show();
        }

        private void nowaWarstwaZZaznToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            if (!(Referencja.narzedzie.aktualne_narzedzie == 1 || Referencja.narzedzie.aktualne_narzedzie == 2 || Referencja.narzedzie.aktualne_narzedzie == 3)) return;
            Referencja.warstwy.Add(new warstwa(Referencja.narzedzie.StworzBitmape()));
            Referencja.warstwy[warstwa.aktualna].Location = Referencja.narzedzie.punkty[0];
            Referencja.narzedzie = new Przenoszenie(null);
        }

        private void usuńWarstwęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            warstwa.UsunWarstwe();
        }


        private void scalWidoczneWarstwyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Referencja.warstwy.Add(new warstwa(new Bitmap(warstwa.Tlo)));
        }


        private void kopiujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            if (Referencja.narzedzie == null) return;
            if (!(Referencja.narzedzie.aktualne_narzedzie == 1 || Referencja.narzedzie.aktualne_narzedzie == 2)) return;
            int temp_aktualna = 0;
            if (Referencja.Schowek != null)
            {
                temp_aktualna = warstwa.aktualna;
                warstwa.aktualna = Referencja.Schowek.pudelko.Numer;
                warstwa.UsunWarstwe();
            }

            Referencja.warstwy.Add(Referencja.Schowek = new warstwa(Referencja.narzedzie.StworzBitmape()));
            Referencja.Schowek.pudelko.label1.Text = "Schowek";
            Referencja.Schowek.Location = Referencja.narzedzie.punkty[0];

            warstwa.ZmianaAktualnejWarstwy(temp_aktualna);
            Referencja.narzedzie = new Przenoszenie(Referencja.Schowek);
            pictureBox1.Refresh();

        }

        private void wklejToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            if (Referencja.Schowek == null) return;

            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            graf.DrawImage(Referencja.Schowek.obrazek, Referencja.Schowek.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, Referencja.Schowek.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y);
            graf.Dispose();
            warstwa.Odswiez();
            Referencja.narzedzie = new Przenoszenie(Referencja.Schowek);
            warstwa.Odswiez();
        }

        private void wytnijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            if (Referencja.narzedzie == null) return;
            if (!(Referencja.narzedzie.aktualne_narzedzie == 1 || Referencja.narzedzie.aktualne_narzedzie == 2)) return;
            int temp_aktualna = 0;
            if (Referencja.Schowek != null)
            {
                temp_aktualna = warstwa.aktualna;
                warstwa.aktualna = Referencja.Schowek.pudelko.Numer;
                warstwa.UsunWarstwe();
            }

            Referencja.warstwy.Add(Referencja.Schowek = new warstwa(Referencja.narzedzie.StworzBitmape()));
            Referencja.Schowek.pudelko.label1.Text = "Schowek";
            Referencja.Schowek.Location = Referencja.narzedzie.punkty[0];

            warstwa.ZmianaAktualnejWarstwy(temp_aktualna);
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            if (Referencja.narzedzie.aktualne_narzedzie == 1)
            {
                graf.FillRectangle(new SolidBrush(Color.White), Referencja.narzedzie.rect);
                graf.Dispose();
            }
            else
            {
                graf.FillPolygon(new SolidBrush(Color.White), ((ZaznaczL)Referencja.narzedzie).punkty_temp.ToArray());
                graf.Dispose();
            }
            Referencja.narzedzie = new Przenoszenie(Referencja.Schowek);
            warstwa.Odswiez();
        }

        private void wyczyśćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            if (Referencja.narzedzie == null) return;
            if (Referencja.narzedzie.aktualne_narzedzie != 1) return;
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            graf.FillRectangle(new SolidBrush(Color.White), Referencja.narzedzie.rect);
            graf.Dispose();
        }

        private void rozmaryPłótnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Rozmiary okno = new Rozmiary();
            okno.Show();
        }

        private void schowajDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            MessageBox.Show("Dane mogą zostać ukryte przy pomocy tego programu tylko w przypadku,\ngdy obraz zostanie zapisany bez kompresji bądź z kompresją bezstratną.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Steganograf.PobierzDane();
        }

        private void odczytajDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            Steganograf.Odczyt();
        }

        private void zmieńNazwęToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Referencja.Zaladowany) return;
            ZmienNazwe okno = new ZmienNazwe();
            okno.Show();
        }


        /////
        ///// Obsługa paneli zmieniających kolory oraz grubości rysowanych kształtów.
        /////
        /////


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Kolor.Grubosc = (int)numericUpDown1.Value;
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            colorDialog1 = new ColorDialog();
            colorDialog1.AllowFullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Kolor.KolorLewy = colorDialog1.Color;
                panel5.BackColor = colorDialog1.Color;
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            colorDialog1 = new ColorDialog();
            colorDialog1.AllowFullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Kolor.KolorPrawy = colorDialog1.Color;
                panel4.BackColor = colorDialog1.Color;
            }
        }



        /////
        ///// Obsługa kliknięć przycisków w przyborniku
        /////
        /////


        private void button1_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new ZaznaczP();
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new ZaznaczL();
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Przenoszenie(null);
            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Wypelnienie();
            pictureBox1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Olowek();
            pictureBox1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Pedzel();
            pictureBox1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Gumka();
            pictureBox1.Refresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new LiniaProsta();
            pictureBox1.Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new LiniaLamana();
            pictureBox1.Refresh();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Bezier();
            pictureBox1.Refresh();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Napis();
            pictureBox1.Refresh();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Prostokat();
            pictureBox1.Refresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Referencja.narzedzie = new Elipsa();
            pictureBox1.Refresh();
        }

    }
}
