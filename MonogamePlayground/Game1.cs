using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Extensions;
using GameLibrary.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        PlayerContainer player;
        Microsoft.Xna.Framework.Graphics.Texture2D baseJeep;
        public KeyboardState previousKeyState { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            Content.RootDirectory = "Content";
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

            this.rTater = new Rotator(348, 180);
            baseJeep = Texture2d.FromFileName(this.GraphicsDevice, "Content/Jeep.png");
            var jeepFrames = FramesGenerator.GenerateFrames( new FrameInfo(243, 243), new Point(baseJeep.Width, baseJeep.Height));
            player = new PlayerContainer(this.spriteBatch, this.baseJeep, new Character(jeepFrames), this.rTater, new Point(100, 125));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void UpdateTheLine(KeyboardState kState)
        {

            Action<Keys, float> keyPressed = (key, angle) =>
            {
                if (kState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key))
                {
                    this.rTater.SetDestinationAngle(angle);
                }

                if (kState.IsKeyUp(key) && !previousKeyState.IsKeyUp(key))
                {
                    this.rTater.StopRotation();
                }

            };

            keyPressed(Keys.D, 90f);
            keyPressed(Keys.W, 0f);
            keyPressed(Keys.S, 180f);
            keyPressed(Keys.A, 270f);


            this.previousKeyState = kState;
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

            this.spriteBatch.DrawString(this.arial, Math.Floor(this.rTater.CurrentAngle).ToString(), new Vector2(10,10), Color.DarkGreen);
            this.spriteBatch.DrawLine(new Vector2(75, 80), 50, this.rTater.CurrentAngle, Color.White);
            this.spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
