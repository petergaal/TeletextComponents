using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GPsoft.Teletext.Font font = new GPsoft.Teletext.Font();
            //GPsoft.Teletext.CodePage codePage = new GPsoft.Teletext.CodePage();
            //GPsoft.Teletext.CodePage codePage = new GPsoft.Teletext.LatinG0CodePage();
            GPsoft.Teletext.CodePage codePage = new GPsoft.Teletext.EnglishCodePage();

            GPsoft.Teletext.TTXRenderer renderer = new GPsoft.Teletext.TTXRenderer(font, codePage);
            renderer.LoadPage("characters.page");
            
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            Bitmap bitmap = renderer.GetBitmap();

            stopWatch.Stop();
            double elapsedMs = stopWatch.ElapsedMilliseconds;
            label1.Text = elapsedMs.ToString() + " ms elapsed";
            pictureBox1.Image = bitmap;
        }
    }
}
