using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameLibrary;
using GameLibrary.AppObjects;
using GameLibrary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.AppObjects
{
    public class BackgroundTilesLayer
    {
        private SpriteBatch spriteBatch;
        private Texture2D[] images;
        private Rectangle[] spiteMap;
        private List<int> map;
        private Rotator rotator;
        private float velocity;
        private Vector2 _currentPosition;
        private DisplayRectInfo[] _sourceArea;
        private Vector2 _previousPosition;
        private Rectangle _destination;
        private Dimensions frameDimensions;
        private Dimensions tileDimensions;
        private int totalWidth;
        private int totalHeight;

        public BackgroundTilesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rectangle[] imageAtlas, List<int> map, Rotator rotator, float initialVelocity, Vector2 startingOffset)
            : this(spriteBatch, images, imageAtlas, map, rotator, initialVelocity, startingOffset, spriteBatch.GraphicsDevice.Viewport.Bounds) { }


        public BackgroundTilesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rectangle[] imageAtlas, List<int> map, Rotator rotator, float initialVelocity, Vector2 startingOffset, Rectangle ViewPort)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            this.spiteMap = imageAtlas;
            this.map = map;
            this.rotator = rotator;
            this.velocity = initialVelocity;
            this._currentPosition = startingOffset;
            this._previousPosition = _currentPosition.AddX(-3);
            this._destination = ViewPort;
            frameDimensions = images.Length > 0 ? new Dimensions(images[0].Width, images[0].Height) : Dimensions.Zero;
            tileDimensions = new Dimensions(imageAtlas[0].Width, imageAtlas[0].Height);

            this.totalWidth = frameDimensions.Width;
            this.totalHeight = frameDimensions.Height;
        }

        public void Update(GameTime gameTime)
        {

            // elasped since last update.
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var directionVector = GeneralExtensions.UnitAngleVector(this.rotator.CurrentAngle);
            // This is where the window top left points in relation to our background.
            this._currentPosition += directionVector * velocity * delta;


            if (_previousPosition != _currentPosition)
            {

                // Stay in the range of our background layer;
                _currentPosition = EnsureBoundries(_currentPosition, totalWidth, totalHeight);
                var Things  = CreateDisplayRectangles(_currentPosition, _destination);
                _sourceArea = Things.ToArray();
                _previousPosition = _currentPosition;
            }
        }

        private DisplayRectInfo[] CreateDisplayRectangles(Vector2 inboundsPosition, Rectangle destination)
        {
            var displayInfo = new List<DisplayRectInfo>();
            // Fractions for accuracy are added back later
            var offsetFraction = inboundsPosition.GetMantissa();

            // Ensure we are at the top left corner
            // These are subtracted out again 
            var moduloOffset = new Vector2((int)Math.Floor(inboundsPosition.X) % tileDimensions.Width, (int)Math.Floor(inboundsPosition.Y) % tileDimensions.Height);
            var normalisedBackgroundPosition = Vector2.Subtract(Vector2.Subtract(inboundsPosition, moduloOffset), offsetFraction);

            var displayRowLength = destination.Width / tileDimensions.Width;
            var displayColLength = destination.Height / tileDimensions.Height;

            var backgroundRowLength = frameDimensions.Width / tileDimensions.Width;

            var ViewPortStart = Vector2.Zero;
            var viewPortPosition = ViewPortStart;
            // Each step is the width/height of the tile inside viewing rectangle.
            // So if you have the fist position correct...Then it's an additive process.
            for (var y = 0; y < displayRowLength; y += 1)
            {
                // convert our currentTopLeft (currentPosition) 
                // into a position on our map.
                viewPortPosition = ViewPortStart.AddY(y * tileDimensions.Height);
                if (y > 0)
                    normalisedBackgroundPosition = EnsureBoundries(new Vector2(inboundsPosition.X - moduloOffset.X, tileDimensions.Height * (y)-moduloOffset.Y), frameDimensions.Width, frameDimensions.Height);
                for (var x = 0; x < displayColLength; x += 1)
                {
                    var backgroundPosition = normalisedBackgroundPosition;
                    var mapPiece = (int)((Math.Floor(backgroundPosition.Y) / tileDimensions.Height) * backgroundRowLength + ((int)Math.Floor(backgroundPosition.X) / tileDimensions.Width));
                    viewPortPosition = viewPortPosition.AddX(tileDimensions.Width);
                    var screenPostion = viewPortPosition;

                    displayInfo.Add(new DisplayRectInfo(this.images[0], Rectangle.Empty, this.spiteMap[mapPiece], Vector2.Subtract(screenPostion,Vector2.Zero)));

                    //Debug.Write($"{backgroundPosition} : {mapPiece}"); //: {viewPortPosition}");  //: ({x},{y})");
                    //Debug.Write("\t");
                    if (x < displayColLength)
                        normalisedBackgroundPosition = EnsureBoundries(normalisedBackgroundPosition.AddX(tileDimensions.Width), frameDimensions.Width, frameDimensions.Height);
                }
                Debug.WriteLine("");
            }

            return displayInfo.ToArray();
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
                var item = _sourceArea[x];

                spriteBatch.Draw(item.Texture, item.DestinationStart, item.SourceArea, Color.White);
            }
        }
    }
}