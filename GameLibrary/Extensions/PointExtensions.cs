using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public static class PointExtensions
    {
        public static Point AddX(this Point @this, int x)
        {
            return new Point(@this.X + x, @this.Y);
        }

        public static Point AddY(this Point @this, int y)
        {
            return new Point(@this.X, @this.Y + y);
        }
    }
}
