using GameLibrary;
using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Config.App;
using GameLibrary.Extensions;
using GameLibrary.Models;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parrallax.Eightway
{
    internal class MapsHost : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arial;
        FourWayDirection fourway;
        private Keyboard4Way _keyboard4Way;
        private ConfigurationData configData;
        private KeyboardState pKState;
        private BoundedBackground backgroundMap;

        public MapsHost(ConfigurationData configData)
        {
            this.configData = configData;
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            arial = this.Content.Load<SpriteFont>("Arial");
            // This is the global rotator. All Rotatiosn made through the keyboard are propogated by this object.

        }

        protected override void Initialize()
        {
            var screenData = configData.ToResultType<ScreenData>("ScreenOptions");
            var player1Dictionary = configData.ToResultType<Dictionary<string, string>>("Player1Controls");
            var player1Keys = GeneralExtensions.ConvertToKeySet<ControlMapping>(player1Dictionary);
            // Configure the screen.
            graphics.PreferredBackBufferWidth = screenData.ScreenWidth;
            graphics.PreferredBackBufferHeight = screenData.ScreenHeight;
            graphics.IsFullScreen = screenData.FullScreen;
            graphics.ApplyChanges();
            // Assign the drawerer
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Can rotate
            this.fourway = new FourWayDirection (FourDirections.Stopped, 0.02f);
            // Allows you to rotate.
            this._keyboard4Way = new Keyboard4Way(this.fourway, player1Keys);

            var slowCloud = this.GraphicsDevice.TextureFromFileName("Content/backBackground.png");// spriteBatch.CreateFilleRectTexture( new Rectangle(0,0, gameWidth + 50, gameHEight + 50), Color.LightCyan);
            var fastCloud = this.GraphicsDevice.TextureFromFileName("Content/frontBackground.png");  //spriteBatch.CreateFilleRectTexture(new Rectangle(0, 0, gameWidth + 50, gameHEight + 50), Color.Orange);
            var sprite = this.GraphicsDevice.TextureFromFileName("Content/FatBlock.png");
            // all rects on a particular atlas.
            // It's also the entire map for the backfround.
            var atlasRects = FramesGenerator.GenerateFrames(new FrameInfo[] { new FrameInfo(32, 32) }, new Dimensions(96, 64));
            var map = GeneralExtensions.LoadCsvMapData("Maps/BorderMap.csv");
            var topLeft = new Vector2(-123, -127);
            backgroundMap = new BoundedBackground(spriteBatch, sprite, atlasRects, map, new Dimensions(32, 32), new Rectangle(0,0, 1024,1152), fourway, topLeft, GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // abort
            var kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keys = KeyboardFunctions.CurrentPressedKeys(kState.GetPressedKeys(), kState, pKState);
            this._keyboard4Way.Update(gameTime, kState, new GamePadState());
            this.fourway.Update(delta);
            backgroundMap.Update(gameTime);

            pKState = kState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // We've divided the screen top and main
            //spriteBatch.DrawFilledRect(new Vector2(0, 0), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height/10, Color.White);
            backgroundMap.Draw();

            spriteBatch.End();

        }
    }
}