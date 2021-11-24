using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Edytor
{
    partial class Referencja
    {
        public static void Jasnosc(int value, Bitmap bmp)
        {

            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width * 4;

                if (value >= 0)
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width * 4; x += 4)
                        {
                            byte* pixel = &wsk[y * width + x];
                            if (pixel[0] + value < 0xff)
                                pixel[0] += (byte)value;
                            else pixel[0] = 0xff;
                            if (pixel[1] + value < 0xff)
                                pixel[1] += (byte)value;
                            else pixel[1] = 0xff;
                            if (pixel[2] + value < 0xff)
                                pixel[2] += (byte)value;
                            else pixel[2] = 0xff;
                        }
                    }
                else
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width * 4; x += 4)
                        {
                            byte* pixel = &wsk[y * width + x];
                            if (pixel[0] + value > 0)
                                pixel[0] += (byte)value;
                            else pixel[0] = 0;
                            if (pixel[1] + value > 0)
                                pixel[1] += (byte)value;
                            else pixel[1] = 0;
                            if (pixel[2] + value > 0)
                                pixel[2] += (byte)value;
                            else pixel[2] = 0;
                        }
                    }


            }
            bmp.UnlockBits(dane);
        }


        public static void Nasycenie_kolorow(int r, int g, int b, Bitmap bmp)
        {
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width * 4;


                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width * 4; x += 4)
                    {
                        byte* pixel = &wsk[y * width + x];

                        if (b > 0)
                            if (pixel[0] + b < 0xff)
                                pixel[0] += (byte)b;
                            else pixel[0] = 0xff;
                        else
                            if (pixel[0] + b > 0)
                                pixel[0] += (byte)b;
                            else pixel[0] = 0;

                        if (g > 0)
                            if (pixel[1] + g < 0xff)
                                pixel[1] += (byte)g;
                            else pixel[1] = 0xff;
                        else
                            if (pixel[1] + g > 0)
                                pixel[1] += (byte)g;
                            else pixel[1] = 0;


                        if (r > 0)
                            if (pixel[2] + r < 0xff)
                                pixel[2] += (byte)r;
                            else pixel[2] = 0xff;
                        else
                            if (pixel[2] + r > 0)
                                pixel[2] += (byte)r;
                            else pixel[2] = 0;
                    }
                }
            }
            bmp.UnlockBits(dane);
        }



        public static void Rozmazanie(Bitmap bmp)
        {
            Bitmap temp = new Bitmap(bmp);
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData temp_dane = temp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk_bmp = (byte*)dane.Scan0;
                byte* wsk_temp = (byte*)temp_dane.Scan0;
                int width = bmp.Width * 4;


                for (int y = 1; y < bmp.Height - 1; y++)
                {
                    for (int x = 4; x < bmp.Width * 4 - 4; x += 4)
                    {
                        byte* pixel_bmp = &wsk_bmp[y * width + x];
                        byte* pixel_temp = &wsk_temp[y * width + x];

                        for (int i = 0; i < 3; i++)
                        {
                            *pixel_bmp = (byte)((*(pixel_temp - width - 4) + *(pixel_temp - width) + *(pixel_temp - width + 4) +
                    *(pixel_temp - 4) + *pixel_temp + *(pixel_temp + 4) +
                    *(pixel_temp + width - 4) + *(pixel_temp + width) + *(pixel_temp + width + 4)) / 9);
                            pixel_bmp++;
                            pixel_temp++;
                        }
                    }
                }
            }
            bmp.UnlockBits(dane);
            temp.UnlockBits(temp_dane);

        }


        public static void Wyostrzanie(Bitmap bmp)
        {
            Bitmap temp = new Bitmap(bmp);
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData temp_dane = temp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk_bmp = (byte*)dane.Scan0;
                byte* wsk_temp = (byte*)temp_dane.Scan0;
                int width = bmp.Width * 4;


                for (int y = 1; y < bmp.Height - 1; y++)
                {
                    for (int x = 4; x < bmp.Width * 4 - 4; x += 4)
                    {
                        byte* pixel_bmp = &wsk_bmp[y * width + x];
                        byte* pixel_temp = &wsk_temp[y * width + x];

                        for (int i = 0; i < 3; i++)
                        {
                            int wart = (-(*(pixel_temp - width) +
                    *(pixel_temp - 4) - *pixel_temp * 20 + *(pixel_temp + 4) +
                     +*(pixel_temp + width)) / 16);

                            if (wart < 0) { wart = 0; }
                            if (wart > 255) { wart = 255; }
                            *pixel_bmp = (byte)wart;
                            pixel_bmp++;
                            pixel_temp++;
                        }
                    }
                }
            }
            bmp.UnlockBits(dane);
            temp.UnlockBits(temp_dane);

        }

        public static void Szarosc(Bitmap bmp)
        {
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width * 4;


                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width * 4; x += 4)
                    {
                        byte* pixel = &wsk[y * width + x];
                        byte kolor = (byte)((pixel[0] + pixel[1] + pixel[2]) / 3);
                        pixel[0] = kolor;
                        pixel[1] = kolor;
                        pixel[2] = kolor;
                    }
                }
            }
            bmp.UnlockBits(dane);
        }

        public static void Sepia(Bitmap bmp)
        {
            BitmapData dane = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* wsk = (byte*)dane.Scan0;
                int width = bmp.Width * 4;


                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width * 4; x += 4)
                    {
                        byte* pixel = &wsk[y * width + x];
                        byte kolor = (byte)((pixel[0] + pixel[1] + pixel[2]) / 3);
                        pixel[0] = kolor;
                        if (kolor + 30 < 255) pixel[1] = (byte)(kolor + 30);
                        else pixel[1] = 255;
                        if (kolor + 60 < 255) pixel[2] = (byte)(kolor + 60);
                        else pixel[2] = 255;

                    }
                }
            }
            bmp.UnlockBits(dane);
        }
    
    
    }
    
}
