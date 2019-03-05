using GameLibrary.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTests.AppObjects;
using System;

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

        public KeyboardState previousKeyState { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.rTater = new Rotator(0, 36);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            arial = this.Content.Load<SpriteFont>("Arial");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
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
            UpdateTheLine(kState);
            // TODO: Add your update logic here
            this.rTater.Update(deltaTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.arial, this.rTater.CurrentAngle.ToString(), new Vector2(50, 50), Color.DarkGreen); 
            this.spriteBatch.DrawLine(new Vector2(400, 400), 100, this.rTater.CurrentAngle, Color.White);
            this.spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
