using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Interfaces;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameLibrary.Player
{
    // ALl the logics, and stuffs to make you empower the character to achieve their potential
    // or something.
    public class PlayerContainer: IGameContainerDrawing
    {

        private Dictionary<KeyMapping, Action> KeyPressToAction = new Dictionary<KeyMapping, Action>();

        public PlayerContainer(SpriteBatch spriteBatch,Texture2D atlas, Character gameChar, Rotator rTater, Dictionary<Keys, KeyMapping> keyMap, Point startPosition)
        {
            _spriteBatch = spriteBatch;
            Atlas = atlas;
            playerCharacter = gameChar;
            this.rTater = rTater;
            this.keyMap = keyMap;
            _currentPosition = startPosition;
        }
        private KeyboardState previousKeyboardState;
        private GamePadState previousPadState;
        public SpriteBatch _spriteBatch { get; }
        public Texture2D Atlas { get; }
        public Character playerCharacter { get; }
        public Rotator rTater { get; }

        private readonly Dictionary<Keys, KeyMapping> keyMap;
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
        private Action<Keys, float, KeyboardState, KeyboardState, Rotator> keyPressed = (key, angle, currentKeyboardState, previousKeyboardState, rTater) =>
        {
            if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
            {
                rTater.SetDestinationAngle(angle);
            }

            if (currentKeyboardState.IsKeyUp(key) && !previousKeyboardState.IsKeyUp(key))
            {
                rTater.StopRotation();
            }
        };

        private Action<Keys,Keys, float, float, float, KeyboardState, KeyboardState, Rotator> keysPressed = (key, key2, angle, firstAngle, secondAngle, currentKeyboardState, previousKeyboardState, rTater) =>
        {
            if (currentKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyDown(key2))
            {
                rTater.SetDestinationAngle(angle);
            }
            
            if (currentKeyboardState.IsKeyUp(key) && !previousKeyboardState.IsKeyUp(key) && (currentKeyboardState.IsKeyDown(key2)))
            {
                rTater.SetDestinationAngle(secondAngle);
            }

            if (currentKeyboardState.IsKeyUp(key2) && !previousKeyboardState.IsKeyUp(key2) && (currentKeyboardState.IsKeyDown(key)))
            {
                rTater.SetDestinationAngle(firstAngle);
            }
        };


        private void InputUpdate(KeyboardState currentKeyboardState, GamePadState padState)
        {
            // Check for multiples first
            if (currentKeyboardState.GetPressedKeys().Length>1){
                keysPressed(Keys.W, Keys.D, 45f, 0f, 90f, currentKeyboardState, previousKeyboardState, rTater);
                keysPressed(Keys.W, Keys.A, 315f, 0f, 270f, currentKeyboardState, previousKeyboardState, rTater);
                keysPressed(Keys.S, Keys.A, 225f, 180f, 270f, currentKeyboardState, previousKeyboardState, rTater);
                keysPressed(Keys.S, Keys.D, 135f, 180f, 90f, currentKeyboardState, previousKeyboardState, rTater);

            }
            else
            {
                keyPressed(Keys.D, 90f, currentKeyboardState, previousKeyboardState, rTater);
                keyPressed(Keys.W, 0f, currentKeyboardState, previousKeyboardState, rTater);
                keyPressed(Keys.S, 180f, currentKeyboardState, previousKeyboardState, rTater);
                keyPressed(Keys.A, 270f, currentKeyboardState, previousKeyboardState, rTater);
            }
        }
    }
}
