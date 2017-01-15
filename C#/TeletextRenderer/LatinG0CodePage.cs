using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPsoft.Teletext
{
    public class LatinG0CodePage: CodePage
    {
        public LatinG0CodePage() : base(1)
        {
            // some meaningful description for this charset:
            // Latin G0 Primary Set, defined on Page 114, ETS 300 706: May 1997
            Name = "Latin G0 Primary Set";

            // some of the characters are defined also in the base class but here will do it again
            codes[0x20] = 0x20; // space
            codes[0x21] = 0x21; // !
            codes[0x22] = 0x22; // "
            codes[0x23] = 0x23; // #
            codes[0x24] = 0x84; // ¤ - this is different from base one
            codes[0x25] = 0x25; // %
            codes[0x26] = 0x26; // &
            codes[0x27] = 0x27; // '
            codes[0x28] = 0x28; // (
            codes[0x29] = 0x29; // )
            codes[0x2a] = 0x2a; // *
            codes[0x2b] = 0x2b; // +
            codes[0x2c] = 0x2c; // ,
            codes[0x2d] = 0x2d; // -
            codes[0x2e] = 0x2e; // .
            codes[0x2f] = 0x2f; // /

            codes[0x30] = 0x30; // 0
            codes[0x31] = 0x31; // 1
            codes[0x32] = 0x32; // 2
            codes[0x33] = 0x33; // 3
            codes[0x34] = 0x34; // 4
            codes[0x35] = 0x35; // 5
            codes[0x36] = 0x36; // 6
            codes[0x37] = 0x37; // 7
            codes[0x38] = 0x38; // 8
            codes[0x39] = 0x39; // 9
            codes[0x3a] = 0x3a; // :
            codes[0x3b] = 0x3b; // ;
            codes[0x3c] = 0x3c; // <
            codes[0x3d] = 0x3d; // =
            codes[0x3e] = 0x3e; // >
            codes[0x3f] = 0x3f; // ?

            codes[0x40] = 0x40; // @
            codes[0x41] = 0x41; // A
            codes[0x42] = 0x42; // B
            codes[0x43] = 0x43; // C
            codes[0x44] = 0x44; // D
            codes[0x45] = 0x45; // E
            codes[0x46] = 0x46; // F
            codes[0x47] = 0x47; // G
            codes[0x48] = 0x48; // H
            codes[0x49] = 0x49; // I
            codes[0x4a] = 0x4a; // J
            codes[0x4b] = 0x4b; // K
            codes[0x4c] = 0x4c; // L
            codes[0x4d] = 0x4d; // M
            codes[0x4e] = 0x4e; // N
            codes[0x4f] = 0x4f; // O

            codes[0x50] = 0x50; // P
            codes[0x51] = 0x51; // Q
            codes[0x52] = 0x52; // R
            codes[0x53] = 0x53; // S
            codes[0x54] = 0x54; // T
            codes[0x55] = 0x55; // U
            codes[0x56] = 0x56; // V
            codes[0x57] = 0x57; // W
            codes[0x58] = 0x58; // X
            codes[0x59] = 0x59; // Y
            codes[0x5a] = 0x5a; // Z
            codes[0x5b] = 0x5b; // [
            codes[0x5c] = 0x5c; // \
            codes[0x5d] = 0x5d; // ]
            codes[0x5e] = 0x5e; // ^
            codes[0x5f] = 0x5f; // _

            codes[0x60] = 0x60; // `
            codes[0x61] = 0x61; // a
            codes[0x62] = 0x62; // b
            codes[0x63] = 0x63; // c
            codes[0x64] = 0x64; // d
            codes[0x65] = 0x65; // e
            codes[0x66] = 0x66; // f
            codes[0x67] = 0x67; // g
            codes[0x68] = 0x68; // h
            codes[0x69] = 0x69; // i
            codes[0x6a] = 0x6a; // j
            codes[0x6b] = 0x6b; // k
            codes[0x6c] = 0x6c; // l
            codes[0x6d] = 0x6d; // m
            codes[0x6e] = 0x6e; // n
            codes[0x6f] = 0x6f; // o

            codes[0x70] = 0x70; // p
            codes[0x71] = 0x71; // q
            codes[0x72] = 0x72; // r
            codes[0x73] = 0x73; // s
            codes[0x74] = 0x74; // t
            codes[0x75] = 0x75; // u
            codes[0x76] = 0x76; // v
            codes[0x77] = 0x77; // w
            codes[0x78] = 0x78; // x
            codes[0x79] = 0x79; // y
            codes[0x7a] = 0x7a; // z
            codes[0x7b] = 0x7b; // {
            codes[0x7c] = 0x7c; // |
            codes[0x7d] = 0x7d; // }
            codes[0x7e] = 0x7e; // ~
            codes[0x7f] = 0x2df; // █ - different from a base one

            // replace with space for higher characters
            for (int i=0x80; i<=0xff; i++)
            {
                codes[i] = 0x20; // space
            }

            for (byte i=0x20; i<=0x23; i++)
            {
                unicodes.Add((Char)i, i);
            }

            for (byte i = 0x25; i <= 0x7e; i++)
            {
                unicodes.Add((Char)i, i);
            }
            unicodes.Add('¤', 0x24);
            unicodes.Add('█', 0x7f);
        }
    }
}
