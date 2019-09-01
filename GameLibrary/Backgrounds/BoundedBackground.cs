using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameLibrary;
using GameLibrary.AppObjects;
using GameLibrary.Extensions;
using GameLibrary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Drawing.Backgrounds
{
    /// <summary>
    /// Draws a background of a certain size, 
    /// but does not repeat it (hence bounde
    /// </summary>
    public class BoundedBackground
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
        private Dimensions mapDimensions;

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
            // expresses the column/Rows in block count (How many rows, with how many cols)
            this.mapDimensions = new Dimensions(bounds.Width / tileDimensions.Width, bounds.Height / tileDimensions.Height);
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

            // From the current Position, calculate the map index.
            // This can be negative (if the map is off screen, or offset for whatever reason)
            // so be careful
            // floatRoat
            var mantissa = backgroundTopLeft.GetMantissa();
            // no floating info
            var integralPos = Vector2.Subtract(backgroundTopLeft, mantissa);
            var moduloX = integralPos.X % tileDimensions.Width;
            var moduloY = integralPos.Y % tileDimensions.Height;
            // removed t
            var normalised = Vector2.Subtract(integralPos, new Vector2(moduloX, moduloY)).ToPoint();
            for (var y = 0; y < viewport.Height; y += tileDimensions.Height)
            {
                var bgNormRow = normalised.AddY(y);
                for (var x = 0; x < viewport.Width; x += tileDimensions.Width)
                {
                    var bgNormCols = bgNormRow.AddX(x);
                    // 0 and above we are in the map!
                    if (bgNormCols.X > -1)
                    {
                        var mapIndexX = bgNormCols.X / tileDimensions.Width;
                        var mapIndexY = bgNormCols.Y / tileDimensions.Height;

                        if (mapIndexX > (this.mapDimensions.Width - 1) || mapIndexY > (mapDimensions.Height - 1))
                            break;
                        
                        var currentMapIndex = ((mapIndexY * this.mapDimensions.Width ) + (mapIndexX));

                        if (currentMapIndex >= 0 && currentMapIndex < this.map.Count)
                        {
                            var displRect = this.map[currentMapIndex];
                            var displayRectIndex = displRect;
                            if (displayRectIndex > -1)
                            {
                                displayRects.Add(new DisplayRectInfo(sprite, Rectangle.Empty,
                                                this.atlasRects[displayRectIndex]
                                                , Vector2.Subtract(new Vector2(x - moduloX, y - moduloY), mantissa)));
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