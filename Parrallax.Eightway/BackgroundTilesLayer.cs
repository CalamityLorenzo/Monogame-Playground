using System.Collections.Generic;
using GameLibrary;
using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parrallax.Eightway
{
    internal class BackgroundTilesLayer
    {
        private SpriteBatch spriteBatch;
        private Texture2D[] images;
        private Rectangle[] spriteMap;
        private List<int> map;
        private Rotator rotator;
        private float velocity;
        private Vector2 _currentPosition;
        private Vector2 _previousPosition;
        private Rectangle _destination;

        public BackgroundTilesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rectangle[] spriteMap,  List<int> map, Rotator rotator, float initialVelocity, Vector2 startingOffset)
            : this(spriteBatch, images, spriteMap, map, rotator, initialVelocity, startingOffset, spriteBatch.GraphicsDevice.Viewport.Bounds) { }


        public BackgroundTilesLayer(SpriteBatch spriteBatch, Texture2D[] images, Rectangle[] spriteMap, List<int> map, Rotator rotator, float initialVelocity, Vector2 startingOffset, Rectangle ViewPort)
        {
            this.spriteBatch = spriteBatch;
            this.images = images;
            this.spriteMap =spriteMap;
            this.map = map;
            this.rotator = rotator;
            this.velocity = initialVelocity;
            this._currentPosition = startingOffset;
            this._previousPosition = _currentPosition.AddX(-3);
            this._destination = ViewPort;
        }

        public void Update(GameTime gameTime)
        {
            if (_previousPosition != _currentPosition)
            {
                 
                _previousPosition = _currentPosition;
            }
        }

        public void Draw()
        {

        }
    }
}