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
        public static IEnumerable<Keys> CurrentPressedKeys(IEnumerable<Keys> CurrentPressedKeys, KeyboardState currentState, KeyboardState previousState)
        {
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
            pressedKeysState.RemoveWhere(k => oldKeys.Any(ok => ok == k));
            return pressedKeysState;
        }
    }
}
