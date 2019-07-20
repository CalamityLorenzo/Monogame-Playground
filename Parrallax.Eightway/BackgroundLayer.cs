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
        private Vector2 _destinationViewPosition;
        private readonly Rectangle _drawRange;
        private Rectangle _destination; // The area we draw too. (Should effectively be the viewingport)
        private readonly Rectangle _sourceArea;

        public BackgroundLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator roation, int velocity, double velocityFactor, Vector2 startOffset)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            frameDimensions = images.Length > 0 ? new Point(images[0].Width, images[0].Height) : Point.Zero;
            this.currentRotation = roation;
            this._velocity = velocity;
            // Where we start in relation to our frames. so 0,0 means the top left of the first texture is at 0,0on the screen,
            // 50,50 would be :put texture starting at 50,50 at 0,0 on the screen,
            // ie whats' the starting co-ordinate for the top-left of the screen.
            this._destinationViewPosition = startOffset; 
            this._drawRange = spriteBatch.GraphicsDevice.Viewport.Bounds;
            _destination = new Rectangle(0, 0, _drawRange.Width, _drawRange.Height);
        }
        public void Update(GameTime gameTime)
        {
            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // calculate drawing start vector
           var directionVector =  GeneralExtensions.UnitAngleVector(90);

            // Our screen is a window
            // This is where the window top left points in relation to our background.
            this._destinationViewPosition += directionVector * _velocity * delta;

            // Make sure we are in the bounds of the width of the background itself. 
            // if it is > that width or > height reset back to 0 plus the amount of difference.
            // In this case I have an array 2 wide 1 high

            var totalWidth = frameDimensions.X * images.Length;
            var totalHeight = frameDimensions.Y;
            // Stay in back ground frame;
            if(_destinationViewPosition.X> totalWidth)
            {
                _destinationViewPosition = new Vector2(_destinationViewPosition.X - totalWidth, _destinationViewPosition.Y);
            }

            if (_destinationViewPosition.Y>totalHeight)
            {
                _destinationViewPosition = new Vector2(_destinationViewPosition.X, _destinationViewPosition.Y- totalHeight);
            }

            // Now we take it down to the background image level
            // we know where on the background we are drawing (as if it were one contiguouis object)
            // now we need to know 1. Which image in the array 2. Where we start.
            // we need to fill up the destination rectangle
            // but we may have to draw 1,2 or 4 rectangles to achieve this.
            var sourceRectsComplete= false;
            var _sourceRects = new Rectangle[4];

            // we move along this iterator
            var _destinationCursor = _destinationViewPosition.ToPoint();

            while (!sourceRectsComplete)
            {
                // f.x == width 
                var imageId = (_destinationCursor.X / frameDimensions.X);

                // Create Rectangle
                var x = frameDimensions.X - _destinationCursor.X;
                var y = frameDimensions.Y - _destination.Y; // Total height of frame - target to get Y Position.
                
                var sourceWidth = _destination.Width >= images[imageId].Width - x ? images[imageId].Width - x : _destination.Width;
                var sourceHeight = _destination.Height >= images[imageId].Height - y ? images[imageId].Height - y : _destination.Height;

                var rectSection = new Rectangle(x,y, sourceWidth ,sourceHeight);

                if (x + sourceWidth >= _destination.Width && x + sourceHeight >= _destination.Height)
                    sourceRectsComplete = true;
                if (!sourceRectsComplete)
                    _destinationCursor = new Vector2(_destinationCursor.X + sourceWidth, _destinationCursor.Y + sourceHeight);

            // Okay now find the area we are mapping.
            // Take the _currentPosition, and find where it in the array.

            }


                // if the length of x draws over the edge, scale it back to what we can draw
                float drawWidth = (_currentPosition.X + this._drawRange.Width) > totalWidth ? totalWidth - _currentPosition.X : _currentPosition.X + this._drawRange.Width;
                float drawHeight = (_currentPosition.Y + this._drawRange.Height) > totalHeight ? totalHeight - _currentPosition.Y : _currentPosition.Y + this._drawRange.Height;
                this._destination = new Rectangle(_currentPosition.ToPoint(), new Point((int)drawWidth, (int)drawHeight));
            }
        }

        public void Draw()
        {
            // we draw a section of our "canvas" that is currently drawable.
            var source = new Rectangle(1000, 0, 50, 90);
            spriteBatch.Draw(images[0], _destination, source, Color.White);
        }
    }
}
