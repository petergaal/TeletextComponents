using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GPsoft.Teletext
{
    /// <summary>
    /// This is a Font class which is able to load teletext font.
    /// Each pixel of the font is encoded into bits in the binary data.
    /// The font should be a 1bit (black & white) bitmap with size 23040 bytes.
    /// The bitmap size is 384x480 pixels (384x480/8=23040).
    /// One row contains 32 characters (each 12 pixels width).
    /// </summary>
    public class Font
    {
        private byte[] font = null;
        public bool initialized { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }

        public Font()
        {
            width = 0;
            height = 0;
            LoadFont(FontResource.FontData);
        }

        public byte[] GetFont()
        {
            return font;
        }

        public void LoadFont(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                if (fs.Length==23040)
                {
                    font = new byte[(int)fs.Length];
                    fs.Read(font, 0, (int)fs.Length);
                    fs.Close();
                    initialized = true;
                    width = 384;
                    height = 480;
                }
                else
                {
                    fs.Close();
                }
            }
        }

        public void LoadFont(byte[] fontData)
        {
            if (fontData.Length == 23040)
            {
                font = fontData;
                initialized = true;
                width = 384;
                height = 480;
            }
        }
    }
}
