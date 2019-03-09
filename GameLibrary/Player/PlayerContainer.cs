using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Player
{
    // ALl the logics, and stuffs to make you empower the character to achieve their potential
    // or something.
    public class PlayerContainer: IGameContainerDrawing
    {
        public PlayerContainer(SpriteBatch spriteBatch,Texture2D atlas, Character gameChar, Rotator rTater, Point startPosition)
        {
            _spriteBatch = spriteBatch;
            Atlas = atlas;
            playerCharacter = gameChar;
            this.rTater = rTater;
            _currentPosition = startPosition;
        }
        private KeyboardState previousKeyboardState;
        private GamePadState previousPadState;
        public SpriteBatch _spriteBatch { get; }
        public Texture2D Atlas { get; }
        public Character playerCharacter { get; }
        public Rotator rTater { get; }

        private Point _currentPosition;
        public Point CurrentPosition => this._currentPosition;
        public void Draw()
        {
            _spriteBatch.Draw(this.Atlas, CurrentPosition.ToVector2(),  null, this.playerCharacter.CurrentDisplayFrame, Vector2.Zero, 0f, new Vector2(0.25f, 0.25f));
        }

        public void Update(GameTime time, KeyboardState keystate, GamePadState padState)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            var currentKeyboardState = keystate;
            var currentPadState= padState;

            // manage the angle
            InputUpdate(currentKeyboardState, padState);
            // Set the angle
            this.rTater.Update(delta);
            // Mange the current state
            playerCharacterCurrentState(this.rTater.CurrentAngle);
            // set the character state
            this.playerCharacter.Update(delta);
            // MIsc
            this.previousPadState = currentPadState;
            this.previousKeyboardState = currentKeyboardState;
        }

        private void playerCharacterCurrentState(float currentAngle)
        {
            // convert to floor integer for simpler times
            var angle =(int)Math.Floor(currentAngle);
            switch (angle)
            {
                case int num when num<30:
                    this.playerCharacter.SetState(JeepState.North);
                    break;
                case int num when num < 60:
                    this.playerCharacter.SetState(JeepState.NorthNorthEast);
                    break;
                case int num when num < 90:
                    this.playerCharacter.SetState(JeepState.NorthEast);
                    break;
                case int num when num < 120:
                    this.playerCharacter.SetState(JeepState.East);
                    break;
                case int num when num < 150:
                    this.playerCharacter.SetState(JeepState.SouthEast);
                    break;
                case int num when num < 180:
                    this.playerCharacter.SetState(JeepState.SouthSouthEast);
                    break;
                case int num when num < 210:
                    this.playerCharacter.SetState(JeepState.South);
                    break;
                case int num when num < 240:
                    this.playerCharacter.SetState(JeepState.SouthSouthWest);
                    break;
                case int num when num < 270:
                    this.playerCharacter.SetState(JeepState.SouthWest);
                    break;
                case int num when num < 300:
                    this.playerCharacter.SetState(JeepState.West);
                    break;
                case int num when num < 330:
                    this.playerCharacter.SetState(JeepState.NorthWest);
                    break;
                case int num when num < 360:
                    this.playerCharacter.SetState(JeepState.NorthNorthWest);
                    break;
            }
        }

        private void InputUpdate(KeyboardState currentKeyboardState, GamePadState padState)
        {
            Action<Keys, float> keyPressed = (key, angle) =>
            {
                if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
                {
                    this.rTater.SetDestinationAngle(angle);
                }

                if (currentKeyboardState.IsKeyUp(key) && !previousKeyboardState.IsKeyUp(key))
                {
                    this.rTater.StopRotation();
                }
            };

            keyPressed(Keys.D, 90f);
            keyPressed(Keys.W, 0f);
            keyPressed(Keys.S, 180f);
            keyPressed(Keys.A, 270f);

        }
    }
}
