using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Edytor
{

    /// <summary>
    /// Klasa zawierająca informacje o kolorach pierwszo- i drugoplanowym
    /// oraz szerokość rysowanych obiektów
    /// </summary>

    public class Kolor
    {

        public Kolor()
        {
            KolorLewy = Color.Black;
            KolorPrawy = Color.White;
        }

        public static Color KolorLewy = Color.Black, KolorPrawy = Color.White;
        public static int Grubosc = 5;
    }

    /// <summary>
    /// Klasa główna, będąca podstawą dla klas potomnych przybornika.
    /// </summary>

    public class Przybornik : Referencja
    {
        public bool aktywny;
        public Rectangle rect;
        public int aktualne_narzedzie = -1;
        public Point[] punkty = new Point[2];

        public Przybornik()
        {
                if(Referencja.narzedzie != null) Referencja.narzedzie.ZniszczObiekt();
        }

        public virtual void Zdarzenie(int typ, MouseEventArgs e) { }
        public virtual void Paint(PaintEventArgs e) { }
        public virtual void ZmianaAktualnejWarstwy(){ }
        public virtual void ZniszczObiekt() { }
        public virtual Bitmap StworzBitmape() { return null; }
    }

    /// <summary>
    /// Zaznaczanie prostokątnego obszaru
    /// </summary>

    public class ZaznaczP : Przybornik
    {
        public ZaznaczP()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 1;
        }

        ~ZaznaczP()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    punkty[0] = e.Location;
                }
                if (e.Button.Equals(MouseButtons.Right))
                {
                    edytor.contextMenuStrip1.Show(edytor.pictureBox1, e.Location);
                }

            }
            else
                if (typ == 2)
                {
                    if (punkty[0] == null) return;
                    if (e.Button == MouseButtons.Left)
                    {
                        punkty[1] = new Point(e.Location.X, e.Location.Y);
                        Referencja.edytor.pictureBox1.Refresh();
                    }
                    else
                        if (typ == 3)
                        {


                        }
                }
        }

        public override Bitmap StworzBitmape()
        {
            Bitmap temp;
            temp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics graf = Graphics.FromImage(temp);
            graf.DrawImage(Referencja.warstwy[warstwa.aktualna].obrazek, new Rectangle(0, 0, rect.Width, rect.Height), new Rectangle(rect.X - Referencja.warstwy[warstwa.aktualna].Location.X, rect.Y - Referencja.warstwy[warstwa.aktualna].Location.Y, rect.Width, rect.Height), GraphicsUnit.Pixel);
            graf.Dispose();
            return temp;
        }


        public override void Paint(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            Pen pen2 = new Pen(Color.Yellow);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            if (punkty[0].X < punkty[1].X) if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[0].X, punkty[0].Y, punkty[1].X - punkty[0].X, punkty[1].Y - punkty[0].Y);
                else rect = new Rectangle(punkty[0].X, punkty[1].Y, punkty[1].X - punkty[0].X, punkty[0].Y - punkty[1].Y);
            else if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[1].X, punkty[0].Y, punkty[0].X - punkty[1].X, punkty[1].Y - punkty[0].Y);
            else rect = new Rectangle(punkty[1].X, punkty[1].Y, punkty[0].X - punkty[1].X, punkty[0].Y - punkty[1].Y);
            e.Graphics.DrawRectangle(pen2, rect);
            e.Graphics.DrawRectangle(pen2, rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            e.Graphics.DrawRectangle(pen, rect);
        }
    }

    /// <summary>
    /// Zaznaczanie obszaru ograniczonego zbiorem punktów
    /// </summary>

    public class ZaznaczL : Przybornik
    {

        public List<Point> punkty_temp = new List<Point>();
        private bool koniec = false;

        public ZaznaczL()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 2;
        }

        ~ZaznaczL()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    if(punkty_temp.Count >1)
                        if (Math.Sqrt((Math.Pow((double)(e.Location.X - punkty_temp[0].X), 2) + Math.Pow((double)(e.Location.Y - punkty_temp[0].Y), 2))) < 7)
                        {
                            koniec = true;
                        }
                    if (koniec == true) return;
                    punkty_temp.Add(e.Location);
                    Referencja.edytor.pictureBox1.Refresh();
                }
                else if (e.Button.Equals(MouseButtons.Right))
                {
                    edytor.contextMenuStrip1.Show(edytor.pictureBox1, e.Location);
                }
            }
        }

        public override Bitmap StworzBitmape()
        {
            int x = punkty_temp[0].X, y = punkty_temp[0].Y, width, height;
            int maxx = 0, maxy = 0;
            for (int i = 0; i < punkty_temp.Count; i++)
            {
                if (x > punkty_temp[i].X) x = punkty_temp[i].X;
                if (y > punkty_temp[i].Y) y = punkty_temp[i].Y;
                if (maxx < punkty_temp[i].X) maxx = punkty_temp[i].X;
                if (maxy < punkty_temp[i].Y) maxy = punkty_temp[i].Y;
            }


            width = maxx - x + 1;
            height = maxy - y + 1;


            Bitmap mono_temp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Bitmap temp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics graf = Graphics.FromImage(temp);
            graf.DrawImage(Referencja.warstwy[warstwa.aktualna].obrazek, new Rectangle(0, 0, width, height), new Rectangle(x - Referencja.warstwy[warstwa.aktualna].Location.X, y - Referencja.warstwy[warstwa.aktualna].Location.Y, width, height), GraphicsUnit.Pixel);
            graf.Dispose();

            for (int i = 0; i < punkty_temp.Count; i++)
                punkty_temp[i] = new Point(punkty_temp[i].X - x, punkty_temp[i].Y - y);

            graf = Graphics.FromImage(mono_temp);
            graf.Clear(Color.FromArgb(0, 0, 0, 0));
            using (Pen pen = new Pen(new SolidBrush(Color.White)))
                graf.FillPolygon(new SolidBrush(Color.White), punkty_temp.ToArray());
            graf.Dispose();

            for (int i = 0; i < punkty_temp.Count; i++)
                punkty_temp[i] = new Point(punkty_temp[i].X + x, punkty_temp[i].Y + y);

            BitmapData dane_mono = mono_temp.LockBits(new Rectangle(0, 0, width, height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData dane_temp = temp.LockBits(new Rectangle(0, 0, width, height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk_mono = (byte*)dane_mono.Scan0;
                byte* wsk_temp = (byte*)dane_temp.Scan0;
                for (int yy = 0; yy < height; yy++)
                {
                    for (int xx = 0; xx < width * 4; xx += 4)
                    {
                        byte* pixel_mono = &wsk_mono[yy * width * 4 + xx];
                        byte* pixel_temp = &wsk_temp[yy * width * 4 + xx];
                        pixel_temp[0] &= pixel_mono[0];
                        pixel_temp[1] &= pixel_mono[1];
                        pixel_temp[2] &= pixel_mono[2];
                        pixel_temp[3] &= pixel_mono[3];
                    }
                }
            }
            mono_temp.UnlockBits(dane_mono);
            temp.UnlockBits(dane_temp);
            mono_temp.Dispose();
            punkty[0] = new Point(x, y);
            return temp;
        }

        public override void Paint(PaintEventArgs e)
        {
            using (Pen pen1 = new Pen(Brushes.Black))
            {
                using (Pen pen2 = new Pen(Brushes.Yellow))
                {
                    using (Pen pen3 = new Pen(Brushes.Green))
                    {

                        if (punkty_temp.Count > 1)
                            for (int i = 0; i < punkty_temp.Count; i++)
                            {
                                e.Graphics.DrawLines(pen2, punkty_temp.ToArray());
                                e.Graphics.DrawLine(pen2, punkty_temp[0], punkty_temp[punkty_temp.Count - 1]);
                            }


                        for (int i = 0; i < punkty_temp.Count; i++)
                        {
                            if (i == 0)
                            {
                                e.Graphics.DrawRectangle(pen3, new Rectangle(punkty_temp[i].X - 2, punkty_temp[i].Y - 2, 4, 4));
                                e.Graphics.DrawRectangle(pen2, new Rectangle(punkty_temp[i].X - 4, punkty_temp[i].Y - 4, 8, 8));
                                e.Graphics.DrawRectangle(pen3, new Rectangle(punkty_temp[i].X - 6, punkty_temp[i].Y - 6, 12, 12));
                            }
                            else
                            {
                                e.Graphics.DrawRectangle(pen1, new Rectangle(punkty_temp[i].X - 2, punkty_temp[i].Y - 2, 4, 4));
                                e.Graphics.DrawRectangle(pen2, new Rectangle(punkty_temp[i].X - 4, punkty_temp[i].Y - 4, 8, 8));
                            }
                        }




                    }
                }
            }
        }
    }

    /// <summary>
    /// Przemieszczanie warstw po płótnie
    /// </summary>

    public class Przenoszenie : Przybornik
    {
        private bool region = false;
        public warstwa kwadrat;

        public Przenoszenie(warstwa RefDoWarstwy)
            : base()
        {
            aktywny = true;
            punkty[0] = new Point(-1, -1);
            aktualne_narzedzie = 3;
            if (RefDoWarstwy == null) 
                this.kwadrat = Referencja.warstwy[warstwa.aktualna];
            else
                this.kwadrat = RefDoWarstwy;
            rect = new Rectangle(kwadrat.Location.X, kwadrat.Location.Y, kwadrat.Width, kwadrat.Height);

        }

        public override void ZmianaAktualnejWarstwy()
        {
            if (warstwa.licznik == 0)
            {
                Referencja.narzedzie = new Przybornik();
                return;
            }

            kwadrat = Referencja.warstwy[warstwa.aktualna];
            rect = new Rectangle(kwadrat.Location.X, kwadrat.Location.Y, kwadrat.Width, kwadrat.Height);
            Referencja.edytor.pictureBox1.Refresh();
        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    if (e.Location.X > kwadrat.Location.X && e.Location.X < kwadrat.Width + kwadrat.Location.X && e.Location.Y > kwadrat.Location.Y && e.Location.Y < kwadrat.Height + kwadrat.Location.Y)
                    {
                        punkty[0] = e.Location;
                        punkty[1] = kwadrat.Location;
                        region = true;
                        rect = new Rectangle(kwadrat.Location.X, kwadrat.Location.Y, kwadrat.Width, kwadrat.Height);
                    }
                    else region = false;
                }
                if (e.Button.Equals(MouseButtons.Right))
                {
                    edytor.contextMenuStrip1.Show(edytor.pictureBox1, e.Location);
                }

            }
            else
                if (typ == 2)
                {
                    if (!region) return;

                    if (e.Button == MouseButtons.Left)
                    {
                        kwadrat.Location.X = e.Location.X - punkty[0].X + punkty[1].X;
                        kwadrat.Location.Y = e.Location.Y - punkty[0].Y + punkty[1].Y;
                        rect.X = punkty[1].X + e.X - punkty[0].X;
                        rect.Y = punkty[1].Y + e.Y - punkty[0].Y;
                        warstwa.Odswiez();
                    }
                    else
                        if (typ == 3)
                        {


                        }

                }
        }

        public override void Paint(PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            Pen pen2 = new Pen(Color.Yellow);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            e.Graphics.DrawRectangle(pen2, rect);
            e.Graphics.DrawRectangle(pen2, rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            e.Graphics.DrawRectangle(pen, rect);
            pen.Dispose();
            pen2.Dispose();
        }

    }

    /// <summary>
    /// Ołówek
    /// </summary>

    public class Olowek : Przybornik
    {
        public Olowek()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 0;
        }

        ~Olowek()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            Pen olowek;
            if (typ == 1) punkty[0] = e.Location;
            if (e.Button.Equals(MouseButtons.Left))
            {
                olowek = new Pen(new SolidBrush(Kolor.KolorLewy));
                graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                punkty[0] = e.Location;
                olowek.Dispose();
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                olowek = new Pen(new SolidBrush(Kolor.KolorPrawy));
                graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                punkty[0] = e.Location;
                olowek.Dispose();
            }
            warstwa.Odswiez();
            graf.Dispose();
        }
    }

    /// <summary>
    /// Pędzel
    /// </summary>

    public class Pedzel : Przybornik
    {
        public Pedzel()
            : base()
        {
            aktywny = true;
            punkty[0] = new Point(-1, -1);
            aktualne_narzedzie = 0;
        }

        ~Pedzel()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);

            if (typ == 1) punkty[0] = e.Location;
            if (e.Button.Equals(MouseButtons.Left))
            {
                using (SolidBrush brush = new SolidBrush(Kolor.KolorLewy))
                {
                    using (Pen olowek = new Pen(brush, Kolor.Grubosc))
                    {
                        graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                        graf.FillEllipse(brush, punkty[0].X - Kolor.Grubosc / 2, punkty[0].Y - Kolor.Grubosc / 2, Kolor.Grubosc, Kolor.Grubosc);
                        punkty[0] = e.Location;
                        olowek.Dispose();
                    }
                }
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                using (SolidBrush brush = new SolidBrush(Kolor.KolorPrawy))
                {
                    using (Pen olowek = new Pen(brush, Kolor.Grubosc))
                    {
                        graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                        graf.FillEllipse(brush, punkty[0].X - Kolor.Grubosc / 2, punkty[0].Y - Kolor.Grubosc / 2, Kolor.Grubosc, Kolor.Grubosc);
                        punkty[0] = e.Location;
                        olowek.Dispose();
                    }
                }
            }

            warstwa.Odswiez();
            graf.Dispose();
        }
    }

    /// <summary>
    /// Gumka
    /// </summary>

    public class Gumka : Przybornik
    {
        public Gumka()
            : base()
        {
            aktywny = true;
            punkty[0] = new Point(-1, -1);
            aktualne_narzedzie = 1;
        }

        ~Gumka()
        {
            
        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);

            if (typ == 1) punkty[0] = e.Location;
            if (e.Button.Equals(MouseButtons.Left))
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    using (Pen olowek = new Pen(brush, Kolor.Grubosc))
                    {
                        graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                        graf.FillRectangle(brush, punkty[0].X - Kolor.Grubosc / 2, punkty[0].Y - Kolor.Grubosc / 2, Kolor.Grubosc, Kolor.Grubosc);
                        punkty[0] = e.Location;
                        olowek.Dispose();
                    }
                }
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                using (SolidBrush brush = new SolidBrush(Kolor.KolorPrawy))
                {
                    using (Pen olowek = new Pen(brush, Kolor.Grubosc))
                    {
                        graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                        graf.FillRectangle(brush, punkty[0].X - Kolor.Grubosc / 2, punkty[0].Y - Kolor.Grubosc / 2, Kolor.Grubosc, Kolor.Grubosc);
                        punkty[0] = e.Location;
                        olowek.Dispose();
                    }
                }
            }

            warstwa.Odswiez();
            graf.Dispose();
        }
    }

    /// <summary>
    /// Tworzy linie proste
    /// </summary>

    public class LiniaProsta : Przybornik
    {
        private Pen olowek;
        private bool rysuj;

        public LiniaProsta()
            : base()
        {
            aktywny = true;
            punkty[0] = new Point(-1, -1);
            aktualne_narzedzie = 0;
            rysuj = false;
        }

        ~LiniaProsta()
        {
            if (olowek != null) olowek.Dispose();
        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            
            if (typ == 1)
            {
                rysuj = true;
                if (e.Button == MouseButtons.Left)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorLewy), Kolor.Grubosc);
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorPrawy), Kolor.Grubosc);
                } 
                punkty[0] = e.Location;
                Referencja.edytor.pictureBox1.Refresh();
            }
            else if (typ == 2)
            {
                punkty[1] = e.Location;
                Referencja.edytor.pictureBox1.Refresh();
            }
            else if (typ == 3)
            {
                rysuj = false;
                Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);

                graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(punkty[1].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[1].Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                warstwa.Odswiez();
                graf.Dispose();
            }
        }

        public override void Paint(PaintEventArgs e)
        {
            if (rysuj == false) return;
            e.Graphics.DrawLine(olowek, punkty[0], punkty[1]);
        }
    }


    /// <summary>
    /// Tworzy linie łamane
    /// </summary>

    public class LiniaLamana : Przybornik
    {
        private Pen olowek;
        private bool rysuj;

        public LiniaLamana()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 0;
            rysuj = false;
        }

        ~LiniaLamana()
        {
            if (olowek != null) olowek.Dispose();
        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorLewy), Kolor.Grubosc);
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorPrawy), Kolor.Grubosc);
                }

                if (rysuj == false) { punkty[0] = e.Location; punkty[1] = e.Location; }
                rysuj = true;
                Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
                graf.FillEllipse(olowek.Brush, punkty[0].X - Kolor.Grubosc / 2, punkty[0].Y - Kolor.Grubosc / 2, Kolor.Grubosc, Kolor.Grubosc);
                graf.DrawLine(olowek, new Point(punkty[0].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[0].Y - Referencja.warstwy[warstwa.aktualna].Location.Y), new Point(punkty[1].X - Referencja.warstwy[warstwa.aktualna].Location.X, punkty[1].Y - Referencja.warstwy[warstwa.aktualna].Location.Y));
                graf.Dispose();
                punkty[0] = punkty[1];
                punkty[1] = e.Location;
                warstwa.Odswiez();
            }
            if (typ == 2)
            {
                punkty[1] = e.Location;
                Referencja.edytor.pictureBox1.Refresh();
            }
        }

        public override void Paint(PaintEventArgs e)
        {
            if (rysuj == false) return;
            e.Graphics.DrawLine(olowek, punkty[0], punkty[1]);

        }
    }



    /// <summary>
    /// Wypełnia obszar
    /// </summary>

    public class Wypelnienie : Przybornik
    {
        private int width, height;
        uint kolorpobrany, kolor;
        unsafe private uint* wsk;
        Stack<int> q = new Stack<int>();

        public Wypelnienie()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 0;
        }



        unsafe int ZbadajSegment(int x1, int y1)
        {
            int x2 = x1;
            int len;
            while (true)
            {
                x1--;
                if ((x1 < 0) || wsk[y1 * width + x1] != kolorpobrany)
                {
                    x1++; break;
                }
            }
            while (true)
            {
                x2++;
                if ((x2 > width) || wsk[y1 * width + x2] != kolorpobrany) break;
            }
            len = x2 - x1;
            q.Push(x1); q.Push(y1); q.Push(len);
            return x2 + 1;
        }



        unsafe void SzukajSegmentu(int x1, int y1, int len)
        {
            int x2 = x1 + len - 1;

            while (x1 < x2)
            {
                if (wsk[y1 * width + x1] == kolor) x1 += 2;
                else if (wsk[y1 * width + x1] != kolorpobrany) x1++;
                else x1 = ZbadajSegment(x1, y1);
            }
        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    kolor = (uint)Kolor.KolorLewy.ToArgb();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    kolor = (uint)Kolor.KolorPrawy.ToArgb();
                }

                width = Referencja.warstwy[warstwa.aktualna].obrazek.Width;
                height = Referencja.warstwy[warstwa.aktualna].obrazek.Height;
                int x = e.Location.X - Referencja.warstwy[warstwa.aktualna].Location.X, y = e.Location.Y - Referencja.warstwy[warstwa.aktualna].Location.Y, len;
                if (x < 0 || x > width || y < 0 || y > height - 1) return;


                BitmapData dane = Referencja.warstwy[warstwa.aktualna].obrazek.LockBits(new Rectangle(0, 0, Referencja.warstwy[warstwa.aktualna].obrazek.Width, 
                    Referencja.warstwy[warstwa.aktualna].obrazek.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                wsk = (uint*)dane.Scan0;
                
                

                kolorpobrany = wsk[y*width + x];
                


                ZbadajSegment(x, y);

                while (q.Count != 0)
                {
                    len =  q.Pop();
                    y =  q.Pop();
                    x =  q.Pop();
                    for (int i = 0; i < len; i++)
                        wsk[y * width + x + i] = kolor;
                    
                    if (y > 0 && y <height - 1) SzukajSegmentu(x, y - 1, len);
                    if (y > 0 && y < height - 1) SzukajSegmentu(x, y + 1, len);

                }
                
            }
            Referencja.warstwy[warstwa.aktualna].obrazek.UnlockBits(dane);
            warstwa.Odswiez();

            }
        }
    }


    /// <summary>
    /// Rysuje prostokąt
    /// </summary>

    public class Prostokat : Przybornik
    {

        public Pen olowek;
        public bool rysuj;

        public Prostokat()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 1;
        }

        ~Prostokat()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorLewy), Kolor.Grubosc);
                }
                if (e.Button == MouseButtons.Right)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorPrawy), Kolor.Grubosc);
                }

                punkty[0] = e.Location;

            }
            else
                if (typ == 2)
                {
                    if (punkty[0].IsEmpty == true) return;

                    punkty[1] = e.Location;

                    if (punkty[0].X < punkty[1].X) if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[0].X, punkty[0].Y, punkty[1].X - punkty[0].X, punkty[1].Y - punkty[0].Y);
                        else rect = new Rectangle(punkty[0].X, punkty[1].Y, punkty[1].X - punkty[0].X, punkty[0].Y - punkty[1].Y);
                    else if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[1].X, punkty[0].Y, punkty[0].X - punkty[1].X, punkty[1].Y - punkty[0].Y);
                    else rect = new Rectangle(punkty[1].X, punkty[1].Y, punkty[0].X - punkty[1].X, punkty[0].Y - punkty[1].Y);
                    Referencja.edytor.pictureBox1.Refresh();
                }
                else if (typ == 3)
                {
                    if (punkty[0].IsEmpty == true) return;

                    Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
                    
                    graf.DrawRectangle(olowek, new Rectangle(rect.X - Referencja.warstwy[warstwa.aktualna].Location.X, rect.Y - Referencja.warstwy[warstwa.aktualna].Location.Y, rect.Width, rect.Height));
                    graf.Dispose();
                    punkty[0] = new Point();
                    warstwa.Odswiez();
                }
        }


        public override void Paint(PaintEventArgs e)
        {
            if (punkty[0].IsEmpty == true) return;
            e.Graphics.DrawRectangle(olowek, rect);
        }
    }


    /// <summary>
    /// Rysuje elipse
    /// </summary>

    public class Elipsa : Przybornik
    {

        public Pen olowek;
        public bool rysuj;

        public Elipsa()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 1;
        }

        ~Elipsa()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorLewy), Kolor.Grubosc);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorPrawy), Kolor.Grubosc);
                }

                punkty[0] = e.Location;

            }
            else
                if (typ == 2)
                {
                    if (punkty[0].IsEmpty == true) return;

                    punkty[1] = e.Location;

                    if (punkty[0].X < punkty[1].X) if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[0].X, punkty[0].Y, punkty[1].X - punkty[0].X, punkty[1].Y - punkty[0].Y);
                        else rect = new Rectangle(punkty[0].X, punkty[1].Y, punkty[1].X - punkty[0].X, punkty[0].Y - punkty[1].Y);
                    else if (punkty[0].Y < punkty[1].Y) rect = new Rectangle(punkty[1].X, punkty[0].Y, punkty[0].X - punkty[1].X, punkty[1].Y - punkty[0].Y);
                    else rect = new Rectangle(punkty[1].X, punkty[1].Y, punkty[0].X - punkty[1].X, punkty[0].Y - punkty[1].Y);
                    Referencja.edytor.pictureBox1.Refresh();;
                }
                else if (typ == 3)
                {
                    if (punkty[0].IsEmpty == true) return;

                    Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
                    graf.DrawEllipse(olowek, new Rectangle(rect.X - Referencja.warstwy[warstwa.aktualna].Location.X, rect.Y - Referencja.warstwy[warstwa.aktualna].Location.Y, rect.Width, rect.Height));
                    graf.Dispose();
                    punkty[0] = new Point();
                    warstwa.Odswiez();
                }
        }


        public override void Paint(PaintEventArgs e)
        {
            if (punkty[0].IsEmpty == true) return;
            e.Graphics.DrawEllipse(olowek, rect);
        }
    }


    /// <summary>
    /// Rysuje tekst
    /// </summary>

    public class Napis : Przybornik
    {
        public Napis()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 0;

            Tekst okno = new Tekst();
            okno.Show();


        }

        ~Napis()
        {

        }

        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
            Pen olowek;
            if (typ == 1) punkty[0] = e.Location;
            if (e.Button.Equals(MouseButtons.Left))
            {
                olowek = new Pen(new SolidBrush(Kolor.KolorLewy));
                graf.DrawLine(olowek, punkty[0], e.Location);
                punkty[0] = e.Location;
                olowek.Dispose();
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                olowek = new Pen(new SolidBrush(Kolor.KolorPrawy));
                graf.DrawLine(olowek, punkty[0], e.Location);
                punkty[0] = e.Location;
                olowek.Dispose();
            }
            warstwa.Odswiez();
            graf.Dispose();
        }
    }


    /// <summary>
    /// Tworzy krzywą Beziera
    /// </summary>

    public class Bezier : Przybornik
    {
        private Pen olowek;
        private int klikniecia = 0 , aktualnypkt = -1, licznikpkt = 0;
        private Point[] pkt_beziera = new Point[4];

        public Bezier()
            : base()
        {
            aktywny = true;
            aktualne_narzedzie = 0;
        }

        public override void ZniszczObiekt()
        {
            if (klikniecia >= 4)
            {
                Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
                graf.DrawBezier(olowek, pkt_beziera[0], pkt_beziera[2], pkt_beziera[3], pkt_beziera[1]);
                graf.Dispose();
                warstwa.Odswiez();
            }
            if (olowek != null) olowek.Dispose();
        }


        public override void Zdarzenie(int typ, MouseEventArgs e)
        {
            if (typ == 1)
            {
                klikniecia++;
                if(klikniecia < 2)
                if (e.Button == MouseButtons.Left)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorLewy), Kolor.Grubosc);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (olowek != null) olowek.Dispose();
                    olowek = new Pen(new SolidBrush(Kolor.KolorPrawy), Kolor.Grubosc);
                }

                if (pkt_beziera[0].IsEmpty == false) 
                { if (Math.Sqrt((Math.Pow((double)(e.Location.X - pkt_beziera[0].X), 2) + Math.Pow((double)(e.Location.Y - pkt_beziera[0].Y), 2))) < 7) aktualnypkt = 0; }
                if (pkt_beziera[1].IsEmpty == false)
                { if (Math.Sqrt((Math.Pow((double)(e.Location.X - pkt_beziera[1].X), 2) + Math.Pow((double)(e.Location.Y - pkt_beziera[1].Y), 2))) < 7) aktualnypkt = 1; }
                if (pkt_beziera[2].IsEmpty == false)
                { if (Math.Sqrt((Math.Pow((double)(e.Location.X - pkt_beziera[2].X), 2) + Math.Pow((double)(e.Location.Y - pkt_beziera[2].Y), 2))) < 7) aktualnypkt = 2; }
                if (pkt_beziera[3].IsEmpty == false)
                { if (Math.Sqrt((Math.Pow((double)(e.Location.X - pkt_beziera[3].X), 2) + Math.Pow((double)(e.Location.Y - pkt_beziera[3].Y), 2))) < 7) aktualnypkt = 3; }
                else


                    if (klikniecia < 5)
                    {
                        pkt_beziera[klikniecia - 1] = e.Location;
                        licznikpkt++;
                    }
                
                Graphics graf = Graphics.FromImage(Referencja.warstwy[warstwa.aktualna].obrazek);
                
                
                graf.Dispose();
                punkty[0] = punkty[1];
                punkty[1] = e.Location;
                warstwa.Odswiez();
            }
            if (typ == 2)
            {
                if(klikniecia < 2)
                punkty[1] = e.Location;


                if (aktualnypkt != -1)
                {
                    pkt_beziera[aktualnypkt] = e.Location;
                }


                Referencja.edytor.pictureBox1.Refresh();
            }

            if (typ == 3)
            {
                aktualnypkt = -1;
            }

        }

        public override void Paint(PaintEventArgs e)
        {
            if (klikniecia > 1) 
            e.Graphics.DrawBezier(olowek, pkt_beziera[0], pkt_beziera[2], pkt_beziera[3], pkt_beziera[1]);

            using (Pen pen1 = new Pen(Brushes.Black))
            {
                using (Pen pen2 = new Pen(Brushes.Yellow))
                {
                    using (Pen pen3 = new Pen(Brushes.Green))
                    {
                        for (int i = 0; i < licznikpkt; i++)
                        {
                            e.Graphics.DrawRectangle(pen1, new Rectangle(pkt_beziera[i].X - 2, pkt_beziera[i].Y - 2, 4, 4));
                            e.Graphics.DrawRectangle(pen2, new Rectangle(pkt_beziera[i].X - 4, pkt_beziera[i].Y - 4, 8, 8));
                            e.Graphics.DrawRectangle(pen3, new Rectangle(pkt_beziera[i].X - 6, pkt_beziera[i].Y - 6, 12, 12));
                        }
                    }
                }
            }
        }
    }
   
}
