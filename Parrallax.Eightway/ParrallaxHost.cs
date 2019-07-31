using GameLibrary;
using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Config.App;
using GameLibrary.Extensions;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parrallax.Eightway
{
    internal class ParrallaxHost : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arial;
        Rotator rotator;
        private KeyboardRotation _keyboardRotator;
        private ConfigurationData configData;
        private BackgroundTilesLayer _backgroundTiles;
        private BackgroundRectanglesLayer _foregroundLayter;
        private BackgroundRectanglesLayer _foregroundLayter2;
        private Vector2 centrePoint;
        private KeyboardState pKState;

        public ParrallaxHost()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";

        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            arial = this.Content.Load<SpriteFont>("Arial");
            // This is the global rotator. All Rotatiosn made through the keyboard are propogated by this object.

        }

        protected override void Initialize()
        {
            this.configData = Configuration.Manager
             .LoadJsonFile("opts.json")
             .LoadJsonFile("opts2.json")
             .Build();
            this.rotator = new Rotator(0, 202);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            var player1Dictionary = configData.ToResultType<Dictionary<string, string>>("Player1Controls");
            var player1Keys = GeneralExtensions.ConvertToKeySet<ControlMapping>(player1Dictionary);
            var gameWidth = this.GraphicsDevice.Viewport.Width / 2;
            var gameHEight = this.GraphicsDevice.Viewport.Height / 2;
            var slowCloud = this.GraphicsDevice.TextureFromFileName("Content/backBackground.png");// spriteBatch.CreateFilleRectTexture( new Rectangle(0,0, gameWidth + 50, gameHEight + 50), Color.LightCyan);
            var fastCloud = this.GraphicsDevice.TextureFromFileName("Content/frontBackground.png");  //spriteBatch.CreateFilleRectTexture(new Rectangle(0, 0, gameWidth + 50, gameHEight + 50), Color.Orange);

            // all rects on a particular atlas.
            // It's also the entire map for the backfround.
            var atlasRects = FramesGenerator.GenerateFrames(new FrameInfo[] { new FrameInfo(25, 25) }, new Dimensions(500, 500));
            var map = Enumerable.Range(0, atlasRects.Length ).Select(i => i).ToList();
            this._backgroundTiles = new BackgroundTilesLayer(spriteBatch, new Texture2D[] { fastCloud, fastCloud }, atlasRects, map, rotator, 0.15f, new Vector2(0, 0));


            this._foregroundLayter = new BackgroundRectanglesLayer(spriteBatch, new Texture2D[] { fastCloud, fastCloud }, rotator, 0.35f, new Vector2(876, 486));
            this._foregroundLayter2 = new BackgroundRectanglesLayer(spriteBatch, new Texture2D[] { slowCloud, slowCloud }, rotator, 0.20f, new Vector2(876, 486));

            centrePoint = new Point(gameWidth, gameHEight).ToVector2();
            this._keyboardRotator = new KeyboardRotation(this.rotator, player1Keys);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var kState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            var keys = KeyboardFunctions.CurrentPressedKeys(kState.GetPressedKeys(), kState, pKState);
            //if (keys.Contains(Keys.Space))
            //{
            //    _foregroundLayter.SwitchRenderer(!_foregroundLayter.UseRectangleRender);
            //    _foregroundLayter2.SwitchRenderer(!_foregroundLayter.UseRectangleRender);
            //}
            //_foregroundLayter.Update(gameTime);
            //_foregroundLayter2.Update(gameTime);

            _backgroundTiles.Update(gameTime);

            _keyboardRotator.Update(gameTime, kState, GamePadState.Default);

            rotator.Update(delta);

            pKState = kState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            //_foregroundLayter2.Draw();
            //_foregroundLayter.Draw();

            _backgroundTiles.Draw();

            // We've divided the screen top and main
            //spriteBatch.DrawFilledRect(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height/10, Color.White);
            spriteBatch.DrawString(arial, Math.Floor(this.rotator.CurrentAngle).ToString(), new Vector2(10, 10), Color.Plum);


            //// not an effective way of doing this.
            spriteBatch.DrawLine(centrePoint.AddX(1), 99, this.rotator.CurrentAngle, Color.White);
            spriteBatch.DrawLine(centrePoint, 100, this.rotator.CurrentAngle, Color.White);
            spriteBatch.DrawLine(centrePoint.AddX(-1), 99, this.rotator.CurrentAngle, Color.White);


            spriteBatch.End();

        }
    }
}