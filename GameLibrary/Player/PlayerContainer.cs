using GameLibrary.Animation;
using GameLibrary.AppObjects;
using GameLibrary.Interfaces;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Player
{
    // ALl the logics, and stuffs to make you empower the character to achieve their potential
    // or something.
    public class PlayerContainer : IGameContainerDrawing
    {

        private readonly Dictionary<KeyMapping, Keys> keyMap;
        private Dictionary<IEnumerable<Keys>, Action> MovementActions;
        private IEnumerable<Keys> pressedKeys;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private GamePadState previousPadState;

        private float currentAngle; // Cheaper to compare/calculate this per update than the direction vector

        private Vector2 directionNormal; // Where we are pointing in space. Apply force to this to move.

        public SpriteBatch _spriteBatch { get; }
        public Texture2D Atlas { get; }
        public Character playerCharacter { get; }
        internal Rotator Rotatation { get; }

        private Vector2 _currentPosition;
        private float _velocity = 0f;
        public Vector2 CurrentPosition => this._currentPosition;

        public PlayerContainer(SpriteBatch spriteBatch, Texture2D atlas, Character gameChar, Rotator rTater, Dictionary<KeyMapping, Keys> keyMap, Point startPosition)
        {
            _spriteBatch = spriteBatch;
            Atlas = atlas;
            playerCharacter = gameChar;
            this.Rotatation = rTater;
            this.keyMap = keyMap;
            _currentPosition = startPosition.ToVector2();

            ConfigureMovementKeyMappings(keyMap);
            pressedKeys = new HashSet<Keys>();
            this.currentAngle = rTater.DestinationAngle; // The vehicle turns but the movement does not.
            directionNormal = GeneralExtensions.AngledVectorFromDegrees(rTater.DestinationAngle);
        }

        private void ConfigureMovementKeyMappings(Dictionary<KeyMapping, Keys> keyMap)
        {
            this.MovementActions = new Dictionary<IEnumerable<Keys>, Action>
            {
                { new[] { this.keyMap[KeyMapping.Up], this.keyMap[KeyMapping.Right] },()=> {this.Rotatation.SetDestinationAngle(45f); this.EnableVelocity(); } },
                { new[] { this.keyMap[KeyMapping.Up], this.keyMap[KeyMapping.Left] },()=> {this.Rotatation.SetDestinationAngle(315f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Down], this.keyMap[KeyMapping.Right] },()=> {this.Rotatation.SetDestinationAngle(135f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Down], this.keyMap[KeyMapping.Left] },()=> {this.Rotatation.SetDestinationAngle(225f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Left] },()=> {this.Rotatation.SetDestinationAngle(270f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Right] },()=> {this.Rotatation.SetDestinationAngle(90f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Up] },()=> {this.Rotatation.SetDestinationAngle(0f); this.EnableVelocity(); }},
                { new[] { this.keyMap[KeyMapping.Down] },()=> {this.Rotatation.SetDestinationAngle(180f); this.EnableVelocity(); }},
            };
        }

        public void Update(GameTime time, KeyboardState keystate, GamePadState padState)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            this.currentKeyboardState = keystate;
            var currentPadState = padState;
            // manage the angle
            this.pressedKeys = KeyboardFunctions.CurrentPressedKeys(pressedKeys, currentKeyboardState, previousKeyboardState);
            //InputProcessor();
            MovementKeyUpdate(pressedKeys);
            // Set the angle
            this.Rotatation.Update(delta);

            // Make sure the movement diretion is correct
            if (Rotatation.State != RotatorState.Stopped && this.currentAngle != this.Rotatation.DestinationAngle)
            {
                this.directionNormal = GeneralExtensions.UnitAngleVector(Rotatation.DestinationAngle);
                this.currentAngle = this.Rotatation.DestinationAngle;
            }


            // Mange the current state
            playerCharacterCurrentAnimState(this.Rotatation.CurrentAngle);
            // set the character state
            this.playerCharacter.Update(delta);
            this.UpdatePosition(delta);
            // MIsc
            this.previousPadState = currentPadState;
            this.previousKeyboardState = currentKeyboardState;
        }

        private void UpdatePosition(float delta)
        {
            if (_velocity != 0f)
            {
                this._currentPosition += directionNormal * _velocity* delta;
            }
        }

        private void playerCharacterCurrentAnimState(float currentAngle)
        {
            // convert to floor integer for simpler times
            var angle = (int)Math.Floor(currentAngle);
            switch (angle)
            {
                case int num when num < 30:
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

        private void MovementKeyUpdate(IEnumerable<Keys> availableKeys)
        {
            var keysActivated = false;
            // this hadles movement and direection
            foreach (var keyBox in MovementActions)
            {
                if (keyBox.Key.All(o => availableKeys.Contains(o)))
                {
                    keyBox.Value();
                    keysActivated = true;
                    break;
                }
            }

            // no movement keys are being pressed.
            if (!keysActivated)
            {
                Rotatation.StopRotation();
                DisableVelocity();
            }
        }

        private void EnableVelocity() => this._velocity = 34f;

        private void DisableVelocity() => this._velocity = 0f;

        public void Draw()
        {
            _spriteBatch.Draw(this.Atlas, CurrentPosition, null, this.playerCharacter.CurrentDisplayFrame, Vector2.Zero, 0f, new Vector2(0.25f, 0.25f));
        }
    }


}

