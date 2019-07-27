using GameLibrary;
using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parrallax.Eightway.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Dimensions frameDimensions;
        private Rotator currentRotation;
        private float _velocity;
        private readonly float _velocityFactor;
        private Vector2 _currentPosition;
        private Vector2 _previousPosition;
        private readonly Rectangle _drawRange;
        private Rectangle _destination; // The area we draw too. (Should effectively be the viewingport)
        private DisplayRectInfo[] _sourceArea;

        public BackgroundLayer(SpriteBatch spriteBatch, Texture2D[] images, Rotator roation, float velocity, float velocityFactor, Vector2 startOffset)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            frameDimensions = images.Length > 0 ? new Dimensions(images[0].Width, images[0].Height) : Dimensions.Zero;
            this.currentRotation = roation;
            this._velocity = velocity;
            this._velocityFactor = velocityFactor;
            // Where we start in relation to our frames. so 0,0 means the top left of the first texture is at 0,0on the screen,
            // 50,50 would be :put texture starting at 50,50 at 0,0 on the screen,
            // ie whats' the starting co-ordinate for the top-left of the screen.
            this._currentPosition = startOffset;
            this._previousPosition = _currentPosition.AddX(-1);
            this._drawRange = spriteBatch.GraphicsDevice.Viewport.Bounds;
            _destination = new Rectangle(Point.Zero, new Point(_drawRange.Width, _drawRange.Height));
        }

        public void Update(GameTime gameTime)
        {
            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // calculate drawing start vector
            var directionVector = GeneralExtensions.UnitAngleVector(this.currentRotation.CurrentAngle);

            // Our screen is a window
            // This is where the window top left points in relation to our background.
            this._currentPosition += directionVector * _velocity * delta;

            // Make sure we are in the bounds of the width of the background itself. 
            // if it is > that width or > height reset back to 0 plus the amount of difference.
            // In this case I have an array 2 wide 1 high
            if (this._previousPosition != _currentPosition)
            {
                var totalWidth = frameDimensions.Width * images.Length;
                var totalHeight = frameDimensions.Height;

                // Stay in back ground frame;
                _currentPosition = EnsureBoundries(_currentPosition, totalWidth, totalHeight);

                // Now we take it down to the background image level
                // we know where on the background we are drawing (as if it were one contiguouis object)
                // now we need to know 1. Which image in the array 2. Where we start.
                // we need to fill up the destination rectangle
                // but we may have to draw 1,2 or 4 rectangles to achieve this.
                // We shift this along to work out if we need another rectangle
                // or we have completed.
                var backgroundCursor = _currentPosition.ToPoint();
                var destinationRect = _destination;

                // These are the recntagles required and describes all the destination rectnagles
                var destinationRects = GetAllTheRectangles(backgroundCursor, destinationRect);
                // now we create all the source rectangles

                _sourceArea = destinationRects.ToArray();
                _previousPosition = _currentPosition;
            }
        }

        private DisplayRectInfo[] GetAllTheRectangles(Point backgroundPosition, Rectangle cameraBoundaries)
        {
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
            var rects = new Rectangle[24];
            var rectCounter = 1;
            rects[0] = rootRectangle;

            long totalAreaToCover = cameraBoundaries.Width * cameraBoundaries.Height;

            for (var heightY = cameraBoundaries.Y; heightY < MaxHeight;)
            {
                for (var rowX = cameraBoundaries.X; rowX + MaxWidth <= cameraBoundaries.Width;)
                {
                    // we are in the row based on the previous.
                    rowX += currentRect.Width;
                    // We don't updat the vertical in the row
                    var setWidth = cameraBoundaries.Width - currentRect.Width;

                    var nextRect = new Rectangle(rowX, currentRect.Y, setWidth>=MaxWidth?MaxWidth:setWidth , currentRect.Height);
                    rects[rectCounter] = nextRect;
                    rectCounter += 1;
                    currentRect = nextRect;
                }
                heightY += currentRect.Height;

                // Calculate the total area of recs so far.
                long totalAreaSoFar = rects.Sum(o => o.Width * o.Height);

                if (totalAreaSoFar < totalAreaToCover)
                {
                    // and we also need to move back to beginning
                    // and will also be the same width as the original
                    var setHeight = cameraBoundaries.Height - heightY;
                    currentRect = new Rectangle(rootRectangle.X, rootRectangle.Y + rootRectangle.Height, rootRectangle.Width, setHeight);
                    rects[rectCounter] = currentRect;
                    rectCounter += 1;
                }
            }

            var fullRects = rects.Where(o => o != Rectangle.Empty).ToArray();

            var displayRects = new List<DisplayRectInfo>();

            int cellId = 0;
            // need to calculate how many rows vs columns. mod is on the horizontal.
            var modId = fullRects.Length > 1 ? fullRects.Length / 2 : 1;
            foreach (var destRect in fullRects)
            {
                (var x, var y, var w, var h) = destRect;

                var sourceX = (x + backgroundPosition.X) - (frameDimensions.Width * cellId);
                var sourceY = y + backgroundPosition.Y;
                var ImageId = x / frameDimensions.Width;

                var sourceRect = new Rectangle(
                                    sourceX >= MaxWidth ? sourceX - MaxWidth : sourceX,
                                    sourceY >= MaxHeight ? sourceY - MaxHeight : sourceY, w, h);
                displayRects.Add(new DisplayRectInfo(images[0], destRect, sourceRect));

                // Debug.WriteLine($"{sourceRect.X} : {sourceRect.Y}");
                cellId = (cellId + 1) % modId;
            }
            return displayRects.ToArray();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundPosition">            
        /// background position is effectively a simple. 
        /// we can calculate all the required rectangles and populate them afterward
        /// This shifts with every entry.
        /// </param>
        /// <param name="cameraBoundaries"> Should get smaller with each pass.</param>
        /// <returns></returns>
        private DisplayRectInfo CreateDisplayRects(Point backgroundPosition, Rectangle cameraBoundaries)
        {
            // From the background Point, caculate me the largest rectangle you can use in side the destinayionBoundary
            var imageId = (backgroundPosition.X / frameDimensions.Width);
            var selectedImage = images[imageId];

            // we need x,y for the tile inside the background.
            var x = backgroundPosition.X - (frameDimensions.Width * imageId);
            var y = backgroundPosition.Y;

            // The largest it can be is the amount of display space left over inside the boundary.
            var sourceWidth = selectedImage.Width - x >= cameraBoundaries.Width ? cameraBoundaries.Width : selectedImage.Width - x;
            var sourceHeight = selectedImage.Height - y >= cameraBoundaries.Height ? cameraBoundaries.Height : selectedImage.Height - y;
            // Append what we have taken
            var dRect = new DisplayRectInfo(selectedImage, new Rectangle(cameraBoundaries.X, cameraBoundaries.Y, sourceWidth, sourceHeight), new Rectangle(x, y, sourceWidth, sourceHeight));
            return dRect;
        }

        private DisplayRectInfo GetRectanglesToDisplay(Point backgroundPosition, Rectangle destinationSize)
        {


            var imageId = (backgroundPosition.X / frameDimensions.Width);
            var selectedImage = images[imageId];
            // background Cursor is the x,y for the layer.
            // we need x,y for the tile.
            var x = (backgroundPosition.X) - (images[imageId].Width * imageId);
            var y = (backgroundPosition.Y);

            // The largest it can be is the amount of display space left over.
            var sourceWidth = selectedImage.Width - x >= (destinationSize.Width) ? destinationSize.Width : selectedImage.Width - x;
            var sourceHeight = selectedImage.Height - y >= (destinationSize.Height) ? destinationSize.Height : selectedImage.Height - y;
            // Append what we have taken
            var dRect = new DisplayRectInfo(selectedImage, new Rectangle(destinationSize.X, destinationSize.Y, sourceWidth, sourceHeight), new Rectangle(x, y, sourceWidth, sourceHeight));
            return dRect;
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
            var source = new Rectangle(1000, 0, 50, 90);
            for (var x = 0; x < _sourceArea.Length; ++x)
            {
                if (_sourceArea[x].Texture != null)
                    spriteBatch.Draw(_sourceArea[x].Texture, _sourceArea[x].DestinationArea, _sourceArea[x].SourceArea, Color.White);
            }
        }
    }
}
