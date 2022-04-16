using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Rootobject
    {
        public Widget Widget { get; set; }
    }

    public class Widget
    {
        public string Debug { get; set; }
        public Window Window { get; set; }
        public Image Image { get; set; }
        public Text Text { get; set; }
    }

    public class Window
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Image
    {
        public string Src { get; set; }
        public string Name { get; set; }
        public int HOffset { get; set; }
        public int VOffset { get; set; }
        public string Alignment { get; set; }
    }

    public class Text
    {
        public string Data { get; set; }
        public int Size { get; set; }
        public string Style { get; set; }
        public string Name { get; set; }
        public int HOffset { get; set; }
        public int VOffset { get; set; }
        public string Alignment { get; set; }
        public string OnMouseUp { get; set; }
    }
}
