using GameLibrary;
using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Config.App;
using GameLibrary.Extensions;
using GameLibrary.Player;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace MonoGameTests
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont arial;
        Rotator rTater;
        ConfigurationData configData;
        PlayerContainer player;
        Microsoft.Xna.Framework.Graphics.Texture2D baseJeep;
        Dictionary<KeyMapping, Keys> player1Keys;
        public KeyboardState previousKeyState { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            Content.RootDirectory = "Content";

            this.configData = Configuration.Manager
                    .LoadJsonFile("opts.json")
                    .LoadJsonFile("opts2.json")
                    .Build();

            var player1Dictionary = configData.ToResultType<Dictionary<string, string>>("Player1Controls");
            var player2Dictionary = configData.ToResultType<Dictionary<string, string>>("Player2Controls");
            player1Keys = GeneralExtensions.ConvertToKeySet<KeyMapping>(player1Dictionary);
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            arial = this.Content.Load<SpriteFont>("Arial");

            this.rTater = new Rotator(348, 225);
            baseJeep = Texture2d.FromFileName(this.GraphicsDevice, "Content/Jeep.png");
            var jeepFrames = FramesGenerator.GenerateFrames(new FrameInfo(243, 243), new Point(baseJeep.Width, baseJeep.Height));
            player = new PlayerContainer(this.spriteBatch, this.baseJeep, new Character(jeepFrames), this.rTater, player1Keys, new Point(100, 125));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //  UpdateTheLine(kState);
            // TODO: Add your update logic here
            player.Update(gameTime, kState, GamePad.GetState(0));
            //this.rTater.Update(deltaTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            player.Draw();

            this.spriteBatch.DrawString(this.arial, Math.Floor(this.rTater.CurrentAngle).ToString(), new Vector2(10, 10), Color.DarkGreen);
            this.spriteBatch.DrawLine(new Vector2(75, 80), 50, this.rTater.CurrentAngle, Color.White);
            this.spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
