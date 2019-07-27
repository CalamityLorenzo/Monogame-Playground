using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.AppObjects
{
    public struct Dimensions
    {
        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width { get; set; }
        public int Height { get; set; }

        public static Dimensions Zero => new Dimensions(0, 0);

        public override string ToString()
        {
            return $"{Width}:{Height}";
        }
    }
}
