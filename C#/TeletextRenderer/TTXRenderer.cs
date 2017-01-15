using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPsoft.Teletext
{

    /// <summary>
    /// TTXRenderer - teletext page rendering class
    /// <para>can process binary teletext page into Bitmap</para>
    /// </summary>
    //
    //  We are using 480x500 pixels output in this class but with a different font it would be possible
    //  to change the resolution. The decoder is not fully implemented yet, there are some minor things
    //  missing. The decoder and renderer itself is very fast and it was developed and optimized
    //  for speed initially in C/C++. The conversion time (page rendering) can be about 0.2-0.3 ms 
    //  on a recent computer in C/C++ - test was made on Intel Skylake (6th generation) Core i7 (laptop).
    //  In C# the page rendering time is typically less than 0.6ms. Some additional overhead is due to
    //  conversion into bitmap but the total time should be about 3ms (measured only in one run).
    public class TTXRenderer : Component
    {
        private byte[] page;
        private Font font;
        private int[] rawImage;
        private Bitmap bitmap;
        private int[] colors = { 0x000000, 0x00ff0000, 0x0000ff00, 0x00ffff00, 0x000000ff, 0x00ff00ff, 0x0000ffff, 0x00ffffff };
        //        private int[] codes;
        private CodePage codePage;
        private bool needsUpdate;

        public int width { get; private set; }
        public int height { get; private set; }

        public TTXRenderer(Font font, CodePage codePage)
        {
            page = new byte[40 * 25];
            for (int i = 0; i < page.Length; i++)
            {
                page[i] = 0x20;
            }
            this.font = font;
            if (font.width == 384 && font.height == 480)
            {
                width = 480;
                height = 500;
                rawImage = new int[width * height];
                this.codePage = codePage;
                bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                UpdateBitmap();
            }
        }

        public void LoadPage(String fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                fs.Read(page, 0, 40 * 25);
                fs.Close();
            }
            needsUpdate = true;
        }

        public Bitmap GetBitmap()
        {
            if (needsUpdate)
            {
                RenderPage(rawImage);
                UpdateBitmap();
            }
            return bitmap;
        }

        public void WriteText(string text, int x, int y)
        {
            WriteText(text, x, y, text.Length);
        }
        public void WriteText(string text, int x, int y, int maxlength)
        {
            if (y < 0 || y > 24 || x < 0 || x > 39)
                return;
            for (int i=0; i<maxlength && i<text.Length; i++)
            {
                Char ch = text[i];
                int code = (int)ch;
                byte b;
                if (codePage.unicodes.ContainsKey(ch))
                {
                    b = codePage.unicodes[ch];
                }
                else if (code >= 32)
                {
                    b = (byte)'?';
                }
                else b = (byte)code;
                if (x + i >= 0 && x + i <= 39)
                    page[y * 40 + x + i] = b;
            }
        }

        /// <summary>
        /// Convert a bitmap to an integer array
        /// </summary>
        /// <param name="bitmap">image to convert</param>
        /// <returns>image as int[]</returns>
        private int[] ConvertBitmap(Bitmap bitmap)
        {
            BitmapData raw = null;  //used to get attributes of the image
            int[] rawImage = null; //the image as a byte[]

            try
            {
                //Freeze the image in memory
                raw = bitmap.LockBits(
                    new Rectangle(0, 0, (int)bitmap.Width, (int)bitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppRgb
                );

                int size = raw.Height * raw.Width;
                rawImage = new int[size];

                //Copy the image into the byte[]
                System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size);

            }
            finally
            {
                if (raw != null)
                {
                    //Unfreeze the memory for the image
                    bitmap.UnlockBits(raw);
                }
            }
            return rawImage;
        }

        /// <summary>
        /// Creates a bitmap given an integer pixel array
        /// </summary>
        /// <param name="rawImage">int[] Pixel data in 32bit format</param>
        /// <returns>Bitmap instance</returns>
        private Bitmap ConvertBitmap(int[] rawImage, int width, int height)
        {
            BitmapData bmd = null;  //used to get attributes of the image

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            try
            {
                // move (no longer reallying copying) the data into the bitmap
                bmd = bitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppRgb);

                System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, bmd.Scan0, rawImage.Length);
            }
            finally
            {
                if (bmd != null)
                {
                    //Unfreeze the memory for the image
                    bitmap.UnlockBits(bmd);
                }
            }
            return bitmap;
        }

        private void UpdateBitmap()
        {
            BitmapData bmd = null;  //used to get attributes of the image

            try
            {
                // move (no longer reallying copying) the data into the bitmap
                bmd = bitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppRgb);

                System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, bmd.Scan0, rawImage.Length);
            }
            finally
            {
                if (bmd != null)
                {
                    //Unfreeze the memory for the image
                    bitmap.UnlockBits(bmd);
                }
            }
        }

        private void RenderPage(int[] image)
        {
            for (int y = 1; y < 24; y++)
            {
                int fgcolor = 7;
                int bgcolor = 0;
                bool graphics = false;
                bool separated = false;
                bool conceal = false;
                bool doubleheight = false;
                bool DHline = false;
                for (int x = 0; x < 40; x++)
                    if (page[y * 40 + x] == 0x0d)
                    {
                        DHline = true;
                    }
                for (int x = 0; x < 40; x++)
                {
                    int ecc = (page[y * 40 + x]) & 0xff;
                    if (ecc < 32)
                    {
                        if (ecc >= 0 && ecc <= 7) // alpha color codes
                        {
                            fgcolor = ecc;
                            graphics = false;
                            conceal = false;
                        }
                        else if (ecc == 0x0c) // normal height
                        {
                            doubleheight = false;
                        }
                        else if (ecc == 0x0d) // double height
                        {
                            doubleheight = true;
                        }
                        else if (ecc >= 0x10 && ecc <= 0x17) // graphics color codes
                        {
                            fgcolor = ecc - 0x10;
                            graphics = true;
                            conceal = false;
                        }
                        else if (ecc == 0x1c) // black background
                        {
                            bgcolor = 0;
                        }
                        else if (ecc == 0x1d) // new background
                        {
                            bgcolor = fgcolor;
                        }
                        RenderWChar(image, 12 * x, 20 * y, 480, 500, 32, colors[fgcolor], colors[bgcolor], doubleheight);
                    }
                    else if (!graphics)
                    {
                        RenderWChar(image, 12 * x, 20 * y, 480, 500, codePage.codes[ecc], colors[fgcolor], colors[bgcolor], doubleheight);
                    }
                    else
                    {
                        int code = ecc;
                        if ((ecc >= 32 && ecc < 64) || (ecc >= 96 && ecc < 128))
                        {
                            code += 736;
                            if (separated)
                            {
                                code += 32;
                            }
                            RenderWChar(image, 12 * x, 20 * y, 480, 500, code, colors[fgcolor], colors[bgcolor], doubleheight);
                        }
                        else
                        {
                            RenderWChar(image, 12 * x, 20 * y, 480, 500, codePage.codes[ecc], colors[fgcolor], colors[bgcolor], doubleheight);
                        }
                    }
                    if (DHline && !doubleheight)
                        RenderWChar(image, 12 * x, 20 * y + 20, 480, 500, 32, colors[fgcolor], colors[bgcolor], false);
                }
                if (DHline)
                {
                    y++;
                }

            }
        }

        private void RenderWChar(int[] image, int x, int y, int width, int height, int ch, int color, int bgcolor, bool doubleheight)
        {

            int font_width = font.width / 32;    // 12
            int font_height = font.height / 48;  // 10
            int ch_address = ch - 32;
            int ch_row = ch_address / 32;
            int ch_col = ch_address % 32;
            for (int yc = ch_row * font.width / 8 * font_height, yi = 0, yi2 = 0; yi < font_height; yc += font.width / 8, yi++, yi2 += 2)
            {

                for (int xc = ch_col * font_width, xi = 0; xi < font_width; xi++, xc++)
                {
                    if ((font.GetFont()[yc + (xc >> 3)] & (1 << (xc & 7))) != 0)
                    {
                        image[xi + x + (yi2 + y) * width] = color;
                        image[xi + x + (yi2 + y + 1) * width] = color;
                        if (doubleheight)
                        {
                            image[xi + x + (yi2 + y + 2) * width] = color;
                            image[xi + x + (yi2 + y + 3) * width] = color;
                        }
                    }
                    else
                    {
                        image[xi + x + (yi2 + y) * width] = bgcolor;
                        image[xi + x + (yi2 + y + 1) * width] = bgcolor;
                        if (doubleheight)
                        {
                            image[xi + x + (yi2 + y + 2) * width] = bgcolor;
                            image[xi + x + (yi2 + y + 3) * width] = bgcolor;
                        }
                    }

                }
                if (doubleheight)
                {
                    yi2 += 2;
                }
            }
        }

        private void RenderText(int[] image, int x, int y, int width, int height, string str, int color, int bgcolor, bool doubleheight)
        {
            int font_width = font.width / 32;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                int code = (int)ch;
                RenderWChar(image, x + font_width * i, y, width, height, code, color, bgcolor, doubleheight);
            }

        }

    }
}
