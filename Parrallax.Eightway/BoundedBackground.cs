using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameLibrary;
using GameLibrary.AppObjects;
using GameLibrary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parrallax.Eightway
{
    internal class BoundedBackground
    {
        private readonly SpriteBatch spriteBatch;
        private Texture2D sprite;
        private Rectangle[] atlasRects;
        private List<int> map;
        private Dimensions tileDimensions;
        private Rectangle bounds;
        private FourWayDirection fourway;
        private Vector2 _currentPosition;
        private List<DisplayRectInfo> _sourceRects;
        private Vector2 _previousPosition;
        private Viewport viewport;

        public BoundedBackground(SpriteBatch spriteBatch, Texture2D sprite, Rectangle[] atlasRects, List<int> map, Dimensions tileDimensions, Rectangle bounds, FourWayDirection fourway, Vector2 backgroundStartPos, Viewport viewPort)
        {
            this.spriteBatch = spriteBatch;
            this.sprite = sprite;
            this.atlasRects = atlasRects;
            this.map = map;
            this.tileDimensions = tileDimensions;
            this.bounds = bounds;
            this.fourway = fourway;
            this._currentPosition = backgroundStartPos;
            this._previousPosition = Vector2.Add(_currentPosition, Vector2.One);
            this.viewport =  viewPort;
            this.viewport.Width += tileDimensions.Width;
            this.viewport.Height += tileDimensions.Height;
        }

        public void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var currentDirection = fourway.CurrentDirectionVector;
            var currentVelocity = fourway.Velocity();

            this._currentPosition += (currentDirection * currentVelocity) * delta;

            if (_currentPosition != _previousPosition)
            {
                //Debug.WriteLine(_currentPosition);
                var displayRects = GetDisplayInfo(_currentPosition);
                _sourceRects = displayRects;
                _previousPosition = _currentPosition;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundTopLeft">That is the top left of the screen (this can be negative)</param>
        private List<DisplayRectInfo> GetDisplayInfo(Vector2 backgroundTopLeft)
        {
            var displayRects = new List<DisplayRectInfo>();
            var mapCols = bounds.Width / tileDimensions.Width;
            var mapRows = bounds.Height / tileDimensions.Height;

            // From the current Position, calculate the map index.
            // This can be negative (if the map is off screen, or offset for whatever reason)
            // so be careful
            // floatRoat
            var mantissa = backgroundTopLeft.GetMantissa();
            // no floating info
            var integralPos = Vector2.Subtract(backgroundTopLeft, mantissa);
            var moduloX = integralPos.X % tileDimensions.Width;
            var moduloY = integralPos.Y % tileDimensions.Height;
            var normalised = Vector2.Subtract(integralPos, new Vector2(moduloX, moduloY));
            for (var y = 0; y < viewport.Height; y += tileDimensions.Height)
            {
                var bgNormRow = normalised.AddY(y);
                for (var x = 0; x < viewport.Width; x += tileDimensions.Width)
                {
                    var bgNormCols = bgNormRow.AddX(x);
                    // 0 and above we are in the map!
                    if (bgNormCols.X >= 0)
                    {
                        var mapIndexX = (int)Math.Floor(bgNormCols.X) / tileDimensions.Width;
                        var mapIndexY = (int)Math.Floor(bgNormCols.Y) / tileDimensions.Height;

                        if (mapIndexX > (mapCols - 1) || mapIndexY > (mapRows - 1))
                            break;
                        
                        var currentMapIndex = ((mapIndexY * mapCols) + (mapIndexX));

                        if (currentMapIndex >= 0 && currentMapIndex < this.map.Count)
                        {
                            var displRect = this.map[currentMapIndex];
                            var displayRectIndex = displRect;
                            if (displayRectIndex > -1)
                            {
                                displayRects.Add(new DisplayRectInfo(sprite, Rectangle.Empty,
                                                this.atlasRects[displayRectIndex]
                                                , Vector2.Add(new Vector2(x - moduloX, y - moduloY), mantissa)));
                            }
                        }
                    }
                }
            }
            return displayRects;
        }

        public void Draw()
        {
            foreach (var rect in _sourceRects)
            {
                spriteBatch.Draw(rect.Texture, rect.DestinationStart, rect.SourceArea, Color.White);
            }
        }
    }
}