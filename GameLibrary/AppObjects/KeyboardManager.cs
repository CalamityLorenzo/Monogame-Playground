using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.AppObjects
{
    /// <summary>
    ///  handles boilterplate code for whats up, whats down and when
    ///  plus  will fire off actions at the correct times.
    /// </summary>
    public class KeyboardManager
    {
        private KeyboardState _previousKeyboard;
        private KeyboardState _currentKeyboard;
        private IEnumerable<Keys> _pressedKeys;
        // If these keys are pressed (No order) then do action.
        // This does mean you have to be careful of the order  eg [Up] matches [Up,Left] and [Up,Right]
        private Dictionary<IEnumerable<Keys>, Action> _keyboardActions = new Dictionary<IEnumerable<Keys>, Action>();
        public KeyboardManager() {
            _pressedKeys = new HashSet<Keys>();
            
        }

        public IEnumerable<Keys> ActiveKeys => _pressedKeys;
        public void Update(KeyboardState keystate)
        {
            // Swap the states and update the current keys
            this._previousKeyboard = _currentKeyboard;
            this._currentKeyboard = keystate;
            this._pressedKeys = KeyboardFunctions.CurrentPressedKeys(_pressedKeys, _currentKeyboard, _previousKeyboard);
            // Now test for all possible movements
            this.CallToAction(_pressedKeys);
        }

        private void CallToAction(IEnumerable<Keys> activeKeys)
        {
            // once 
            foreach (var keyBox in _keyboardActions)
            {
                if (keyBox.Key.All(o => activeKeys.Contains(o)))
                {
                    keyBox.Value();
                    break;
                }
            }
        }
    }
}
