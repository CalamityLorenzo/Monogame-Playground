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
        private Vector2 _oldPosition;
        private readonly Rectangle _drawRange;
        private Rectangle _destination; // The area we draw too. (Should effectively be the viewingport)
        private Rectangle[] _sourceRects = new Rectangle[4];

        public BackgroundLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator roation, int velocity, double velocityFactor, Vector2 startOffset)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            // The area we have to update each iteration.
            frameDimensions = images.Length > 0 ? new Point(images[0].Width * images.Length, images[0].Height * images.Length) : Point.Zero;
            this.currentRotation = roation;
            this._velocity = velocity;
            // Where we start in relation to our frames. so 0,0 means the top left of the first texture is at 0,0on the screen,
            // 50,50 would be :put texture starting at 50,50 at 0,0 on the screen,
            // ie whats' the starting co-ordinate for the top-left of the screen.
            this._currentPosition = startOffset;
            this._oldPosition = _currentPosition;
            this._oldPosition.X += 1;
            this._drawRange = spriteBatch.GraphicsDevice.Viewport.Bounds;
            _destination = new Rectangle(0, 0, _drawRange.Width, _drawRange.Height);
        }
        public void Update(GameTime gameTime)
        {
            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // calculate drawing start vector
            //var directionVector = GeneralExtensions.UnitAngleVector(180);
            //// now updated our start position (Where on the textture we are drawing ) to pass to the rectangle
            //this._currentPosition += directionVector * _velocity * delta;

            if (_currentPosition.X != this._oldPosition.X || _currentPosition.Y != this._oldPosition.Y)
            {
                // Make sure we are in the bounds of the width of the background.
                // if it is > that width or > height reset back to 0 plus the amount of difference.
                // In this case I have an array 2 wide 1 high

                // can have upto 4 tins being drawn on screen at once. 
                // The idea is : We have a certain amount of sceen to fill (width x height)
                // using the power of maths we will ensure the whole screen is filled.
                // each pass through the loop will add an entry to our _sourceRects 
                // we will zero out any we don't use.

                var totalWidth = frameDimensions.X;
                var totalHeight = frameDimensions.Y;
                var displayRectsBuilt = false;

                int  selectedX =0, selectedY = 0;
                

                while(!displayRectsBuilt)
                {

                }

                if (_currentPosition.X > totalWidth)
                {
                    _currentPosition = new Vector2(_currentPosition.X - totalWidth, _currentPosition.Y);
                }

                if (_currentPosition.Y > totalHeight)
                {
                    _currentPosition = new Vector2(_currentPosition.X, _currentPosition.Y - totalHeight);
                }
                // Obviously can't be less than 0
                if (_currentPosition.X < 0)
                {
                    _currentPosition = new Vector2(totalWidth + _currentPosition.X, _currentPosition.Y);
                }

                if (_currentPosition.Y < 0)
                {
                    _currentPosition = new Vector2(_currentPosition.X, totalHeight + _currentPosition.Y);
                }

                // Okay now find the area we are mapping.
                // Take the _currentPosition, and find where it in the array.

                // if we can fill the whole area, if not onlydraw a certain amount

                // if the length of x draws over the edge, scale it back to what we can draw
                float drawWidth = (_currentPosition.X + this._drawRange.Width) > totalWidth ? totalWidth - _currentPosition.X : _currentPosition.X + this._drawRange.Width;
                float drawHeight = (_currentPosition.Y + this._drawRange.Height) > totalHeight ? totalHeight - _currentPosition.Y : _currentPosition.Y + this._drawRange.Height;
                //  this._destination = new Rectangle(_currentPosition.ToPoint(), new Point((int)drawWidth, (int)drawHeight));
                this._sourceRects[0] = new Rectangle((int)_currentPosition.X, (int)_currentPosition.Y, (int)drawWidth, (int)drawHeight);

                this._oldPosition = _currentPosition;
            }
        }

        public void Draw()
        {
            // we draw a section of our "canvas" that is currently drawable.
            var (x, y) = (50, 50);

            var source = new Rectangle(50, 50, images[0].Width - x, images[0].Height - y);
            spriteBatch.Draw(images[0], Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
