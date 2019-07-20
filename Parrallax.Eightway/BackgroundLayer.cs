﻿using GameLibrary;
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
            var directionVector = GeneralExtensions.UnitAngleVector(90);
            // now updated our start position (Where on the textture we are drawing ) to pass to the rectangle
            this._currentPosition += directionVector * _velocity * delta;

            if (_currentPosition.X != this._oldPosition.X || _currentPosition.Y != this._oldPosition.Y)
            {
                // Make sure we are in the bounds of the width of the background.
                // if it is > that width or > height reset back to 0 plus the amount of difference.
                // In this case I have an array 2 wide 1 high

                var totalWidth = images[0].Width;
                var totalHeight = images[0].Height;

                if (_currentPosition.X > totalWidth)
                {
                    _currentPosition = new Vector2(_currentPosition.X - totalWidth, _currentPosition.Y);
                }

                if (_currentPosition.Y > totalHeight)
                {
                    _currentPosition = new Vector2(_currentPosition.X, _currentPosition.Y - totalHeight);
                }

                // Okay now find the area we are mapping.
                // Take the _currentPosition, and find where it in the array.

                // if we can fill the whole area, if not onlydraw a certain amount

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
