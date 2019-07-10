using GameLibrary;
using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrallax.Eightway
{
    // we are alwasys moving 
    public class BackgroundLayer
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D[] images;
        private Point frameDimensions;
        private Rotator currentRotation;
        private readonly int _velocity;
        private Vector2 _currentPosition;
        private readonly Rectangle _drawRange;
        private readonly Rectangle _destination; // The area we draw too. (Should effectively be the viewingport)
        private readonly Rectangle _sourceArea;

        public BackgroundLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator roation, int velocity, double velocityFactor, Vector2 startOffset)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            frameDimensions = images.Length > 0 ? new Point(images[0].Width, images[0].Height) : Point.Zero;
            this.currentRotation = roation;
            this._velocity = velocity;
            // Where we start in relation to our frames. so 0,0 means the top left of the first texture is at 0,0on the screen,
            // 50,50 would be :put texture starting at 50,50 at 0,0,
            // ie whats' the starting co-ordinate for the top-left of the screen.
            this._currentPosition = startOffset; 
            this._drawRange = spriteBatch.GraphicsDevice.Viewport.Bounds;
            _destination = new Rectangle(0, 0, _drawRange.Width, _drawRange.Height);
        }
        public void Update(GameTime gameTime)
        {
            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // calculate drawing start vector
           var directionVector =  GeneralExtensions.UnitAngleVector(90);
            // now updated our start position (Where on the textture we are drawing ) to pass to the rectangle
            this._currentPosition += directionVector * _velocity * delta;
            

        }   

        public void Draw()
        {
            // we draw a section of our "canvas" that is currently drawable.
            var source = new Rectangle(1000, 0, 50, 90);
            spriteBatch.Draw(images[0], _destination, source, Color.White);
        }
    }
}
