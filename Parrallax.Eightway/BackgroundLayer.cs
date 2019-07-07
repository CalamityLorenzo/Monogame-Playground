using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrallax.Eightway
{
    public class BackgroundLayer
    {
        private readonly Texture2D image;
        private readonly int canvasWidth;
        private readonly int canvasHeight;

        public BackgroundLayer(Texture2D image, int totalRepeats, int canvasWidth, int canvasHeight)
        {
            this.image = image;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
        }
        public void Update(GameTime gameTime)
        {
            // Draw our
        }

        public void Draw()
        {
            // we draw a section of our canvas that is currently drawable.
        }
    }
}
