using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPsoft.Teletext
{
     /// <summary>
     /// Default codepage class for teletext renderer. Each code is mapped directly to the output.
     /// </summary>
    public class CodePage
    {
        public int[] codes { get; protected set; }
        public Dictionary<char, byte> unicodes { get; protected set; }
        public string Name { get; protected set; }
        public CodePage(int Index)
        {
            codes = new int[256];
            for (int i = 0; i < 256; i++)
            {
                codes[i] = i;
            }
            unicodes = new Dictionary<char, byte>();
            if (Index==0)
            {
                Name = "default (abstract)";
                for (byte i=32; i<128; i++)
                {
                    Char ch = (Char)i;
                    unicodes.Add(ch, i);
                }
            }
        }
        public CodePage(): this(0)
        {
        }
    }
}
