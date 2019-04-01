using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.PlayerThings
{
    // Handles all the keyboard interactions in an object,
    public class KeyboardManager
    {
        private KeyboardState _currentKeyboard;
        private KeyboardState _previousKeyboard;
        private IEnumerable<Keys> _pressedKeys;
        // If a key is pressed in this collection, then do the thing.
        // Becareful of the order as Singles can Match collections eg [Up] eq [Up,Right] [Up, Left]
        private Dictionary<IEnumerable<Keys>, Action> _movingActions;
        private Action _deadZone; // no kesys pressed whlist moving.
        private Dictionary<IEnumerable<Keys>, Action> _firingActions;
        private Dictionary<IEnumerable<Keys>, Action> _specialActions;

        // If the following keys are NOT pressed, do an action. eg no movement keys [Up,Down,Left,Right]- Stop things
        // Same caveats as before.
        private Dictionary<IEnumerable<Keys>, Action> _unpressedActions;


        public KeyboardManager()
        {
            _movingActions = new Dictionary<IEnumerable<Keys>, Action>();
            _firingActions = new Dictionary<IEnumerable<Keys>, Action>();
            _specialActions = new Dictionary<IEnumerable<Keys>, Action>();
            _pressedKeys = new HashSet<Keys>();
        }

        public void Update(float delta, KeyboardState nextState)
        {
            this._previousKeyboard = this._currentKeyboard;
            this._currentKeyboard = nextState;
            // These are the actual honest to goodness pressed keys.
            // that were not pressed before.
            _pressedKeys = KeyboardFunctions.CurrentPressedKeys(_pressedKeys, _currentKeyboard, _previousKeyboard);

            MovementFireActions(_pressedKeys, this._movingActions);
            GeneralFireActions(_pressedKeys, this._firingActions);
            GeneralFireActions(_pressedKeys, this._specialActions);
            UnpressedActions(_pressedKeys, this._unpressedActions);
        }

        public void AddMovingActions(Dictionary<IEnumerable<Keys>, Action> movement, Action action)
        {
            this._movingActions = new Dictionary<IEnumerable<Keys>, Action>(movement);
            this._deadZone = action;
        }

        public void AddFiringActions(Dictionary<IEnumerable<Keys>, Action> firing)
        {
            this._firingActions = new Dictionary<IEnumerable<Keys>, Action>(firing);
        }

        public void AddSpecialActions(Dictionary<IEnumerable<Keys>, Action> special)
        {
            this._specialActions = new Dictionary<IEnumerable<Keys>, Action>(special);
        }


        private void MovementFireActions(IEnumerable<Keys> pressedKeys, Dictionary<IEnumerable<Keys>, Action> Movements)
        {
            bool movingKeyPressed = false;
            // Becuase of the problems with multipe keys vs single keys,
            // we break as soon as one type of movement matches.
            foreach (var keysKey in Movements)
            {
                if (keysKey.Key.All(o => pressedKeys.Contains(o)))
                {
                    keysKey.Value();
                    movingKeyPressed = true;
                    break;
                }
            }

            // if no movement actions are occuring then do the deadzone thing.
            if (!movingKeyPressed)
                this._deadZone();
        }

        private void GeneralFireActions(IEnumerable<Keys> pressedKeys, Dictionary<IEnumerable<Keys>, Action> actions)
        {
            // Becuase of the problems with multipe keys vs single keys,
            // we break as soon as one type of movement matches.
            foreach (var keysKey in actions)
            {
                if (keysKey.Key.All(o => pressedKeys.Contains(o)))
                {
                    keysKey.Value();
                    break;
                }
            }

        }

        /// <summary>
        /// When these comination of keys are NOT pressed then do something.
        /// </summary>
        /// <param name="pressedKeys"></param>
        private void UnpressedActions(IEnumerable<Keys> pressedKeys, Dictionary<IEnumerable<Keys>, Action> nullActions)
        {
            this._unpressedActions = nullActions;
        }

    }
}
