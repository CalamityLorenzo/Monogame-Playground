using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public static class KeyboardFunctions
    {
        /// <summary>
        /// puts the currently pressed keys into an array
        /// Removes the dropped ones
        /// </summary>
        /// <param name="CurrentPressedKeys"></param>
        /// <param name="currentState"></param>
        /// <param name="previousState"></param>
        /// <returns></returns>
        public static IEnumerable<Keys> CurrentPressedKeys(IEnumerable<Keys> CurrentPressedKeys, KeyboardState currentState, KeyboardState previousState)
        {
            if (previousState == null)
                return new List<Keys>();
            HashSet<Keys> pressedKeysState = new HashSet<Keys>(CurrentPressedKeys);
            var appKeys = currentState.GetPressedKeys();
            // add newly pressed keys
            foreach (var key in appKeys)
            {
                if (!previousState.IsKeyDown(key))
                    pressedKeysState.Add(key);
            }
            var oldKeys = new HashSet<Keys>();
            foreach (var pressedKey in pressedKeysState)
            {
                if (previousState.IsKeyDown(pressedKey) && !appKeys.Contains(pressedKey))
                {
                    oldKeys.Add(pressedKey);
                }
            }
            // remove newly lifted keys
            pressedKeysState.RemoveWhere(prk => oldKeys.Any(ok => ok == prk));
            return pressedKeysState;
        }
    }
}
