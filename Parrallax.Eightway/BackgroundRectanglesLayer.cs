using GameLibrary;
using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parrallax.Eightway.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parrallax.Eightway
{
    // we are alwasys moving 
    public class BackgroundRectanglesLayer
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D[] images;
        private Dimensions frameDimensions;
        private Rotator currentRotation;
        private float _velocity;
        private Vector2 _currentPosition;
        private Vector2 _previousPosition;
        private Rectangle _destination; // The area we draw too. (Should effectively be the viewingport)
        private DisplayRectInfo[] _sourceArea;

        public BackgroundRectanglesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator rotation, float velocity, Vector2 startOffset)
            : this(spriteBatch, images, rotation, velocity, startOffset, spriteBatch.GraphicsDevice.Viewport.Bounds)
        {

        }

        public BackgroundRectanglesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator roation, float velocity, Vector2 startOffset, Rectangle ViewPort)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            frameDimensions = images.Length > 0 ? new Dimensions(images[0].Width, images[0].Height) : Dimensions.Zero;
            this.currentRotation = roation;
            this._velocity = velocity;
            // Where we start in relation to our frames. so 0,0 means the top left of the first texture is at 0,0on the screen,
            // 50,50 would be: display background from position 50,50 at screen co-ords 0,0.
            // ie whats' the starting co-ordinate for the top-left of the screen.
            this._currentPosition = startOffset;
            // not perfect, ensures it DOES update.
            this._previousPosition = _currentPosition.AddX(-1);
            _destination = new Rectangle(Point.Zero, new Point(ViewPort.Width, ViewPort.Height));
        }

        public void Update(GameTime gameTime)
        {
            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var directionVector = GeneralExtensions.UnitAngleVector(this.currentRotation.CurrentAngle);
            // This is where the window top left points in relation to our background.
            this._currentPosition += directionVector * _velocity * delta;

            // Make sure we are in the bounds of the width of the background itself. 
            // if it is > that width or > height reset back to 0 plus the amount of difference. (clock maths)
            if (this._previousPosition != _currentPosition)
            {
                var totalWidth = frameDimensions.Width * images.Length;
                var totalHeight = frameDimensions.Height;
                // Stay in the range of our background layer;
                _currentPosition = EnsureBoundries(_currentPosition, totalWidth, totalHeight);
                // These are the recntagles required and describes all the destination rectnagles
                var destinationRects = CreateDisplayRectangles(_currentPosition.ToPoint(), _destination);
                _sourceArea = destinationRects.ToArray();
                _previousPosition = _currentPosition;
            }
        }

        private DisplayRectInfo[] CreateDisplayRectangles(Point backgroundPosition, Rectangle cameraBoundaries)
        {

            // Now we take it down to the background image level
            // we know where on the background we are drawing (as if it were one contiguouis object)
            // now we need to know 1. Which image in the array 2. Where we start.
            // we need to fill up the destination rectangle
            // but we may have to draw 1,2, 4 etc  rectangles to achieve this.

            var MaxWidth = cameraBoundaries.Width >= frameDimensions.Width ? frameDimensions.Width : cameraBoundaries.Width;
            var MaxHeight = cameraBoundaries.Height >= frameDimensions.Height ? frameDimensions.Height : cameraBoundaries.Height;

            // lets calculate the first rectangle that we will draw from
            // having this determins the rest of the rectangles
            var imageId = (backgroundPosition.X / frameDimensions.Width);
            // we need x,y for the tile inside the background.
            var xStart = backgroundPosition.X - (frameDimensions.Width * imageId);
            var yStart = backgroundPosition.Y;
            // The largest it can be is the amount of display space left over inside the boundary.
            var tSwidth = frameDimensions.Width - xStart;
            var tsHeight = frameDimensions.Height - yStart;
            var sourceWidth = tSwidth >= MaxWidth ? MaxWidth : tSwidth;
            var sourceHeight = tsHeight >= MaxHeight ? MaxHeight : tsHeight;

            var rootRectangle = new Rectangle(cameraBoundaries.X, cameraBoundaries.Y, sourceWidth, sourceHeight);
            var currentRect = rootRectangle;
            var rects = new List<Rectangle>();
            rects.Add(rootRectangle);

            long totalAreaToCover = cameraBoundaries.Width * cameraBoundaries.Height;
            // The boundaries are all about the viewing port.
            // For each rectangle generated they can be only as a large as the smallest boundry (Background Tile, cameraBoundary)
            for (var heightY = cameraBoundaries.Y; heightY < cameraBoundaries.Height;)
            {
                for (var rowX = cameraBoundaries.X; rowX + MaxWidth <= cameraBoundaries.Width;)
                {
                    // we are in the row. Our start point is the previous plus the width of the previous element.
                    rowX += currentRect.Width;
                    // We don't updat the vertical in the row
                    var setWidth = cameraBoundaries.Width - rowX; // currentRect.Width;
                    var currentMaxWidth = cameraBoundaries.Width - rowX;
                    var nextRect = new Rectangle(rowX, currentRect.Y, setWidth >= MaxWidth ? MaxWidth : setWidth > currentMaxWidth ? currentMaxWidth : setWidth, currentRect.Height);
                    rects.Add(nextRect);
                    currentRect = nextRect;
                }
                // Move the camera, direction down, maths up.
                heightY += currentRect.Height;

                // Calculate the total area of recs so far.
                // if we exceed the total area then we are obviously out of bounds.
                long totalAreaSoFar = rects.Sum(o => o.Width * o.Height);
                if (totalAreaSoFar < totalAreaToCover)
                {
                    // and we also need to move back to beginning of the row
                    // so we are using the same dimensions are the first one. (with adjustmes ifwe are hnaning off the page
                    // and will also be the same width as the original
                    var setHeight = cameraBoundaries.Height - heightY;
                    var currentMaxHeight = cameraBoundaries.Height - heightY;
                    currentRect = new Rectangle(cameraBoundaries.X, currentRect.Y + currentRect.Height, sourceWidth, setHeight >= MaxHeight ? MaxHeight : setHeight > currentMaxHeight ? currentMaxHeight : setHeight);
                    rects.Add(currentRect);
                }
            }



            var displayRects = new List<DisplayRectInfo>();

            int cellId = 0;
            int rowId = 0;
            // need to calculate how many rows vs columns. mod is on the horizontal.
            var modVal = RowCells(rects);
            foreach (var destRect in rects)
            {
                (var x, var y, var w, var h) = destRect;

                var sourceX = (x + backgroundPosition.X) - (frameDimensions.Width * cellId);
                var sourceY = y + backgroundPosition.Y - (frameDimensions.Width * rowId);
                var ImageId = x / frameDimensions.Width;

                var sourceRect = new Rectangle(
                                    sourceX >= MaxWidth ? sourceX - MaxWidth : sourceX,
                                    sourceY, w, h);

                displayRects.Add(new DisplayRectInfo(images[imageId], destRect, sourceRect));

                cellId = (cellId + 1) % modVal;
                if (cellId == 0)
                    rowId += 1;
            }
            return displayRects.ToArray();

        }
        /// <summary>
        /// how many rectangles in a 'row'
        /// </summary>
        /// <param name="fullRects"></param>
        /// <returns></returns>
        private int RowCells(List<Rectangle> fullRects)
        {
            if (fullRects.Count > 1)
            {
                // get the value of the first x
                // ALl Rows MUST start with the same value
                // And a value (XPos) cannot be repeated in a row.
                var measure = fullRects[0].X;
                for (var x = 1; x < fullRects.Count; ++x)
                    // found the start of the next row. so return the value as the number cells
                    if (measure == fullRects[x].X) return x;
            }
            return 1;
        }
        private Vector2 EnsureBoundries(Vector2 v, int totalWidth, int totalHeight)
        {
            var x = v.X;
            var y = v.Y;
            if (x < 0)
                x = totalWidth + x;
            if (y < 0)
                y = totalWidth + y;
            return new Vector2(x >= totalWidth ? x - totalWidth : x, y >= totalHeight ? y - totalHeight : y);
        }
        public void Draw()
        {
            // we draw a section of our "canvas" that is currently drawable.
            for (var x = 0; x < _sourceArea.Length; ++x)
            {
                spriteBatch.Draw(_sourceArea[x].Texture, _sourceArea[x].DestinationArea, _sourceArea[x].SourceArea, Color.White);
            }
        }
    }
}
