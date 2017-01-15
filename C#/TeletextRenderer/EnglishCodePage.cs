using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPsoft.Teletext
{
    /// <summary>
    /// English codepage class for teletext renderer.
    /// </summary>

    public class EnglishCodePage : CodePage
    {
        public EnglishCodePage() : base(1)
        {
            // Latin G0 Primary Set + English Sub Set, defined on Page 114 & 115, ETS 300 706: May 1997

            Name = "English CodePage";

            // The rest is defined in the base class, here we will have the differences
            codes[0x23] = 0x083; // £
                     
            codes[0x5b] = 0x181; // ←
            codes[0x5c] = 0x09d; // ½
            codes[0x5d] = 0x183; // →
            codes[0x5e] = 0x182; // ↑
            codes[0x5f] = 0x023; // #

            codes[0x60] = 0x173; // ―

            codes[0x7b] = 0x09c; // ¼
            codes[0x7c] = 0x161; // ║
            codes[0x7d] = 0x09e; // ¾
            codes[0x7e] = 0x0d7; // ÷
            codes[0x7f] = 0x2df; // █ - different from a base one

            // replace with space for higher characters
            for (int i = 0x80; i <= 0xff; i++)
            {
                codes[i] = 0x20; // space
            }

            for (byte i = 0x20; i <= 0x22; i++)
            {
                unicodes.Add((Char)i, i);
            }
            unicodes.Add('£', 0x23);

            for (byte i = 0x24; i <= 0x5a; i++)
            {
                unicodes.Add((Char)i, i);
            }
            unicodes.Add('←', 0x5b);
            unicodes.Add('½', 0x5c);
            unicodes.Add('→', 0x5d);
            unicodes.Add('↑', 0x5e);
            unicodes.Add('#', 0x5f);
            unicodes.Add('―', 0x60);

            for (byte i = 0x61; i <= 0x7a; i++)
            {
                unicodes.Add((Char)i, i);
            }

            unicodes.Add('¼', 0x7b);
            unicodes.Add('║', 0x7c);
            unicodes.Add('¾', 0x7d);
            unicodes.Add('÷', 0x7e);
            unicodes.Add('█', 0x7f);
        }
    }
}
