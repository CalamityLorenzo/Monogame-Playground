using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 AddX(this Vector2 @this, int X)
        {
            return new Vector2(@this.X + X, @this.Y);
        }
        public static Vector2 AddY(this Vector2 @this, int Y)
        {
            return new Vector2(@this.X, @this.Y + Y);
        }
        public static Vector2 AddX(this Vector2 @this, float X)
        {
            return new Vector2(@this.X + X, @this.Y);
        }
        public static Vector2 AddY(this Vector2 @this, float Y)
        {
            return new Vector2(@this.X, @this.Y + Y);
        }

        public static Vector2 GetMantissa(this Vector2 @this)
        {
            // math dot floor rounds...THat may be a problem
            var oX = @this.X - (float)Math.Floor(@this.X);
            var oY = @this.Y - (float)Math.Floor(@this.Y);
            return new Vector2(oX, oY);
        }
    }
}
