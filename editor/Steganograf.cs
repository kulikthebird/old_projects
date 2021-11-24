using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Edytor
{

    /// <summary>
    /// Klasa Steganograf zawiera niezbędne do ukrywania i odczytywania z bitmapy funkcje.
    /// </summary>
    class Steganograf
    {

        public static string sciezka1, sciezka2; // Ścieżki do plików.

        /// <summary>
        /// Funkcja hashująca łańcuch znaków.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Odczytuje ukryte w bitmapie dane i zapisuje do ustalonego przez użytkownika pliku.
        /// </summary>
        /// 
        public static void Odczyt()
        {
            Bitmap bmp = Referencja.warstwy[warstwa.aktualna].obrazek;
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            Stream plik = null;

            unsafe
            {
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width;
                int rozmiar = 0;
                int licznik = 0;
                int bajt = 0;


                if ((32 + 4) > warstwa.Tlo.Height * warstwa.Tlo.Width * 3)
                {
                    MessageBox.Show("Plik ten nie zawiera ukrytych tym programem informacji.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bmp.UnlockBits(dane);
                    return;
                }

                string przed_hashowaniem = "";



                for (int i = 0; i < 32; i++)
                {
                    przed_hashowaniem += *wsk;
                    rozmiar += (int)(*wsk & 1) << i; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                }

                string hash = MD5(przed_hashowaniem);
                byte[] hash_zobrazka = new byte[32];
                
                for (int i = 0; i < 256; i++)
                {
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;

                    hash_zobrazka[i/8] = (byte)bajt; bajt = 0;
                }

                if (System.Text.Encoding.Default.GetString(hash_zobrazka) != hash)
                {
                    MessageBox.Show("Plik ten nie zawiera ukrytych tym programem informacji.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bmp.UnlockBits(dane);
                    return;
                }

                SaveFileDialog dial = new SaveFileDialog();
                if (DialogResult.Equals(dial.ShowDialog(), DialogResult.OK))
                {
                    sciezka1 = dial.FileName;
                }
                else return;
                try
                {
                    plik = File.Open(sciezka1, FileMode.Create);
                }
                catch
                {
                    bmp.UnlockBits(dane);
                    MessageBox.Show("Plik nie mógł zostać utworzony.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                for( int i=0; i<rozmiar; i++)
                {
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); bajt <<= 1; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    bajt += (*wsk & 1); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;

                    plik.WriteByte((byte)bajt); bajt = 0;
                }
                

            }
            bmp.UnlockBits(dane);
            if(plik != null) plik.Close();
        }

        /// <summary>
        /// Wyświetla okno dialogowe, w którym użytkownik wybiera plik do ukrycia w bitmapie.
        /// </summary>

        public static void PobierzDane()
        {
            Stream plik;
            OpenFileDialog dial = new OpenFileDialog();

            if (DialogResult.Equals(dial.ShowDialog(), DialogResult.OK))
            {
                sciezka2 = dial.FileName;
            }

            try
            {
                plik = File.Open(sciezka2, FileMode.Open);
            }

            catch
            {
                MessageBox.Show("Nie można uzyskać dostępu do podanego pliku.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((plik.Length + 32 + 4) > warstwa.Tlo.Height * warstwa.Tlo.Width * 3)
            {
                MessageBox.Show("Plik, który chcesz ukryć ma za duży rozmiar. Zwiększ rozmiary płótna.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sciezka2 = null;
            }
            plik.Close();
        }

        /// <summary>
        /// Ukrywa wybrany wcześniej przez użytkownika plik w bitmapie.
        /// </summary>

        public static void Zapis()
        {
            Stream plik;
            Bitmap bmp = warstwa.Tlo;
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                plik = File.Open(sciezka2, FileMode.Open);
            }

            catch
            {
                MessageBox.Show("Nie można uzyskać dostępu do podanego pliku.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            unsafe
            {
                
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width;
                int wczytany, licznik=0;

                int rozmiar = (int)plik.Length;

                if ((rozmiar + 32 + 4) > bmp.Height * bmp.Width * 3)
                {
                    bmp.UnlockBits(dane);
                    plik.Close();
                    MessageBox.Show("Plik, który chcesz ukryć ma za duży rozmiar. Zwiększ rozmiary płótna.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string przed_hashowaniem = "";

                for(int i=0;i<32;i++)
                {
                    *wsk = (byte)((*wsk & 254) + (rozmiar & 1)); przed_hashowaniem += *wsk; if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    rozmiar >>= 1;
                }

                string hash = MD5(przed_hashowaniem);

                for (int i = 0; i < 256; i++)
                {
                    wczytany = hash[i/8];
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 7) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 6) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 5) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 4) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 3) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 2) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 1) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                }

                while (true)
                {
                    wczytany = plik.ReadByte();
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 7) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 6) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 5) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 4) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 3) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 2) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany >> 1) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    *wsk = (byte)((*wsk & 254) + ((wczytany) & 1)); if (licznik % 3 == 2) wsk += 2; else wsk++; licznik++;
                    if (plik.Position == plik.Length) break;
                }
            }
            bmp.UnlockBits(dane);
            plik.Close();
        }

    }
}