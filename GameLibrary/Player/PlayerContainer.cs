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
        private Dictionary<KeyMapping, Action<KeyboardState, KeyboardState>> KeyPressToAction = new Dictionary<KeyMapping, Action<KeyboardState, KeyboardState>>();
        private Dictionary<Keys, Action<KeyboardState, KeyboardState>> KeyboardActions;
        private Dictionary<Keys, Action<List<Keys>, KeyboardState, KeyboardState>> KeyboardMutationActions;

        private Dictionary<IEnumerable<Keys>, Action> MovementActions;


        private IEnumerable<Keys> pressedKeys;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private GamePadState previousPadState;
        public SpriteBatch _spriteBatch { get; }
        public Texture2D Atlas { get; }
        public Character playerCharacter { get; }
        public Rotator rTater { get; }

        private Point _currentPosition;
        public Point CurrentPosition => this._currentPosition;

        private Action<Keys, float, KeyboardState, KeyboardState, Rotator> KenkeyPressed = (key, angle, currentKeyboardState, previousKeyboardState, rTater) =>
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

        private Action<Keys, float, KeyboardState, KeyboardState, Rotator> keyPressed = (key, angle, currentKeyboardState, previousKeyboardState, rTater) =>
        {
            rTater.SetDestinationAngle(angle);

        };

        private Action<Keys, Keys, float, List<Keys>, KeyboardState, KeyboardState, Rotator> keyAndMutaorPressed = (widdershinsKey, clockwiseKey, angle, currentKeys, currentKeyboardState, previousKeyboardState, rTater) =>
        {
            var tempDestAngle = currentKeys.Contains(widdershinsKey) ? angle - 45 : currentKeys.Contains(clockwiseKey) ? angle + 45 : angle;
            var destinationAngle = 0f;
            destinationAngle = tempDestAngle > 360 ? tempDestAngle % 360 : tempDestAngle < 0 ? 360 - tempDestAngle : tempDestAngle;

            rTater.SetDestinationAngle(destinationAngle);
        };

        private Action<Keys, Keys, float, float, float, KeyboardState, KeyboardState, Rotator> keysPressed = (key, key2, angle, firstAngle, secondAngle, currentKeyboardState, previousKeyboardState, rTater) =>
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

        public PlayerContainer(SpriteBatch spriteBatch, Texture2D atlas, Character gameChar, Rotator rTater, Dictionary<KeyMapping, Keys> keyMap, Point startPosition)
        {
            _spriteBatch = spriteBatch;
            Atlas = atlas;
            playerCharacter = gameChar;
            this.rTater = rTater;
            this.keyMap = keyMap;
            _currentPosition = startPosition;

            ConfigureKeyMappings(keyMap);
            pressedKeys = new HashSet<Keys>();
        }

        private void ConfigureKeyMappings(Dictionary<KeyMapping, Keys> keyMap)
        {
            this.KeyboardActions = new Dictionary<Keys, Action<KeyboardState, KeyboardState>>
            {
                { keyMap[KeyMapping.Up], (c,p) => keyPressed(keyMap[KeyMapping.Up], 0f, c,p, this.rTater) },
                { keyMap[KeyMapping.Down], (c,p) => keyPressed(keyMap[KeyMapping.Down], 180f, c,p, this.rTater) },
                { keyMap[KeyMapping.Left], (c,p) => keyPressed(keyMap[KeyMapping.Left], 270f, c,p, this.rTater) },
                { keyMap[KeyMapping.Right], (c,p) => keyPressed(keyMap[KeyMapping.Right], 90f, c,p, this.rTater) }
            };

            this.KeyboardMutationActions = new Dictionary<Keys, Action<List<Keys>, KeyboardState, KeyboardState>>
            {
                { keyMap[KeyMapping.Up], (k, c,p) => keyAndMutaorPressed(keyMap[KeyMapping.Left], keyMap[KeyMapping.Right], 0f, k, c,p, this.rTater)},
                { keyMap[KeyMapping.Down], (k,c,p) => keyAndMutaorPressed(keyMap[KeyMapping.Right], keyMap[KeyMapping.Left], 180f,k, c,p, this.rTater) },
                { keyMap[KeyMapping.Left], (k,c,p) => keyAndMutaorPressed(keyMap[KeyMapping.Down],keyMap[KeyMapping.Up], 270f, k,c,p, this.rTater) },
                { keyMap[KeyMapping.Right], (k, c,p) => keyAndMutaorPressed(keyMap[KeyMapping.Up], keyMap[KeyMapping.Down], 90f,k, c,p, this.rTater) }
            };

            this.MovementActions = new Dictionary<IEnumerable<Keys>, Action>
            {
                { new[] { this.keyMap[KeyMapping.Up], this.keyMap[KeyMapping.Right] },()=> this.rTater.SetDestinationAngle(315f)},
                { new[] { this.keyMap[KeyMapping.Up], this.keyMap[KeyMapping.Left] },()=> this.rTater.SetDestinationAngle(45f)},
                { new[] { this.keyMap[KeyMapping.Down], this.keyMap[KeyMapping.Right] },()=> this.rTater.SetDestinationAngle(135f)},
                { new[] { this.keyMap[KeyMapping.Down], this.keyMap[KeyMapping.Left] },()=> this.rTater.SetDestinationAngle(225f)},
                //{ new[] { this.keyMap[KeyMapping.Left] },()=> this.rTater.SetDestinationAngle(270f)},
                //{ new[] { this.keyMap[KeyMapping.Right] },()=> this.rTater.SetDestinationAngle(90f)},
                //{ new[] { this.keyMap[KeyMapping.Up] },()=> this.rTater.SetDestinationAngle(0f)},
                //{ new[] { this.keyMap[KeyMapping.Down] },()=> this.rTater.SetDestinationAngle(180f)},
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
            // this hadles movement and direection
            foreach (var keyBox in MovementActions)
            {
                //if (keyBox.Ke.All(o=>availableKeys.`))
                //{
                //    keyBox.Value();
                //}
            }
        }

        public void Draw()
        {
            _spriteBatch.Draw(this.Atlas, CurrentPosition.ToVector2(), null, this.playerCharacter.CurrentDisplayFrame, Vector2.Zero, 0f, new Vector2(0.25f, 0.25f));
        }
    }


}

//private void InputProcessor()
//{
//    //if(this.KeyboardActions.ContainsKey(currentKeyboardState.)

//    var currentKeys = this.currentKeyboardState.GetPressedKeys().ToList();
//    //if (currentKeys.Length == 0) return;

//    // Keys being pressed.....
//    foreach (var key in currentKeys)
//    {
//        if (this.KeyboardActions.ContainsKey(key))
//        {
//            if (currentKeyboardState.IsKeyDown(key))
//            {
//                this.KeyboardMutationActions[key](currentKeys, currentKeyboardState, previousKeyboardState);
//                break;
//            }
//        }
//    }

//    // no keys are being pressed so it
//    if (currentKeys.Count == 0)
//        rTater.StopRotation();
//    //foreach(var releasedKey in this.KeyboardActions.Keys)
//    //{
//    //    if (currentKeyboardState.IsKeyUp(releasedKey) && !previousKeyboardState.IsKeyUp(releasedKey))
//    //    {
//    //        rTater.StopRotation();
//    //    }
//    //}

//}

//private void InputUpdate(KeyboardState currentKeyboardState, GamePadState padState)
//{
//    // Check for multiples first
//    if (currentKeyboardState.GetPressedKeys().Length > 1)
//    {
//        keysPressed(Keys.W, Keys.D, 45f, 0f, 90f, currentKeyboardState, previousKeyboardState, rTater);
//        keysPressed(Keys.W, Keys.A, 315f, 0f, 270f, currentKeyboardState, previousKeyboardState, rTater);
//        keysPressed(Keys.S, Keys.A, 225f, 180f, 270f, currentKeyboardState, previousKeyboardState, rTater);
//        keysPressed(Keys.S, Keys.D, 135f, 180f, 90f, currentKeyboardState, previousKeyboardState, rTater);

//    }
//    else
//    {
//        keyPressed(Keys.D, 90f, currentKeyboardState, previousKeyboardState, rTater);
//        keyPressed(Keys.W, 0f, currentKeyboardState, previousKeyboardState, rTater);
//        keyPressed(Keys.S, 180f, currentKeyboardState, previousKeyboardState, rTater);
//        keyPressed(Keys.A, 270f, currentKeyboardState, previousKeyboardState, rTater);
//    }
//}

