using GameLibrary.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Extensions
{
    public static class SpriteBatchExtensions
    {
        private static Lazy<Microsoft.Xna.Framework.Graphics.Texture2D> OnePixel;

        private static Texture2D GetOnePixelTexture(GraphicsDevice device)
        {
            if (OnePixel == null)
            {
                var textureData = new Color[] { Color.White };
                OnePixel = new Lazy<Microsoft.Xna.Framework.Graphics.Texture2D>(() => new Microsoft.Xna.Framework.Graphics.Texture2D(device, 1, 1));
                OnePixel.Value.SetData<Color>(textureData);
            }
            return OnePixel.Value;
        }

        public static void DrawFilledRect(this SpriteBatch @this, Vector2 start, int width, int height, Color Colour)
        {
            var texture = SpriteBatchExtensions.GetOnePixelTexture(@this.GraphicsDevice);

            @this.Draw(texture, new Rectangle(start.ToPoint(), new Point(width, height)), Colour);

        }

        public static Texture2D CreateFilleRectTexture(this SpriteBatch @this, Rectangle dimensions, Color colour)
        {
            var filledColour = Enumerable.Range(0, dimensions.Width * dimensions.Height).Select(o=>colour);
            var texture = new Texture2D(@this.GraphicsDevice, dimensions.Width, dimensions.Height);
            texture.SetData<Color>(filledColour.ToArray());
            return texture;
        }

        // simple drawing of 1 pixel line using bresnenhams
        public static void DrawLine(this SpriteBatch @this, Vector2 start, int length, float angleInDegrees, Color Colour)
        {
            var texture = SpriteBatchExtensions.GetOnePixelTexture(@this.GraphicsDevice);

            var endVector = GeneralExtensions.UnitAngleVector(angleInDegrees, length);
            // negation of x was ngation of y not sure why it works.
            var endPoint = start + endVector;
            // create the list of points
            var lineData = Bresenham.GetLine(start.ToPoint(), endPoint.ToPoint());

            lineData.ToList().ForEach(a => @this.Draw(texture, a.ToVector2(), Color.White));
        }
    }
}
