using GameLibrary.AppObjects;
using GameLibrary.Extensions;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrallax.Eightway
{
    internal class KeyboardRotation
    {
        private readonly Rotator _rotator;
        private KeyboardManager _keyManager = new KeyboardManager();
        private readonly Dictionary<PlayerControls, Keys> _keyMap;

        public KeyboardRotation(Rotator rotator, Dictionary<PlayerControls, Keys> keyMap)
        {
            this._rotator = rotator;
            this._keyMap = keyMap;
            EightWayKeyboard.CreateKeyboardMappings(_keyManager, _keyMap, rotator);
        }

        public void Update(GameTime time, KeyboardState keystate, GamePadState padState)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            _keyManager.Update(delta, keystate);
        }

    }

    class EightWayKeyboard
    {
        public static void CreateKeyboardMappings(KeyboardManager manager, Dictionary<PlayerControls, Keys> keyMap, Rotator rotator)
        {
            manager.AddMovingActions(new Dictionary<IEnumerable<Keys>, Action>
            {
                { new[] {  keyMap[PlayerControls.Up],  keyMap[PlayerControls.Right] },()=> { rotator.SetDestinationAngle(45f);  } },
                { new[] {  keyMap[PlayerControls.Up],  keyMap[PlayerControls.Left] },()=> { rotator.SetDestinationAngle(315f);  }},
                { new[] {  keyMap[PlayerControls.Down],  keyMap[PlayerControls.Right] },()=> { rotator.SetDestinationAngle(135f);  }},
                { new[] {  keyMap[PlayerControls.Down],  keyMap[PlayerControls.Left] }, ()=> { rotator.SetDestinationAngle(225f);  }},
                { new[] {  keyMap[PlayerControls.Left] },()=> { rotator.SetDestinationAngle(270f);  }},
                { new[] {  keyMap[PlayerControls.Right] },()=> {rotator.SetDestinationAngle(90f);  }},
                { new[] {  keyMap[PlayerControls.Up] },()=> {rotator.SetDestinationAngle(0f);  }},
                { new[] {  keyMap[PlayerControls.Down] },()=> { rotator.SetDestinationAngle(180f);  }},
            }, () => rotator.StopRotation());
        }

    }

}
