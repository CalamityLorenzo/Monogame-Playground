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
        private readonly Dictionary<ControlMapping, Keys> _keyMap;

        public KeyboardRotation(Rotator rotator, Dictionary<ControlMapping, Keys> keyMap)
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
        public static void CreateKeyboardMappings(KeyboardManager manager, Dictionary<ControlMapping, Keys> keyMap, Rotator rotator)
        {
            manager.AddMovingActions(new Dictionary<IEnumerable<Keys>, Action>
            {
                { new[] {  keyMap[ControlMapping.Up],  keyMap[ControlMapping.Right] },()=> { rotator.SetDestinationAngle(45f);  } },
                { new[] {  keyMap[ControlMapping.Up],  keyMap[ControlMapping.Left] },()=> { rotator.SetDestinationAngle(315f);  }},
                { new[] {  keyMap[ControlMapping.Down],  keyMap[ControlMapping.Right] },()=> { rotator.SetDestinationAngle(135f);  }},
                { new[] {  keyMap[ControlMapping.Down],  keyMap[ControlMapping.Left] }, ()=> { rotator.SetDestinationAngle(225f);  }},
                { new[] {  keyMap[ControlMapping.Left] },()=> { rotator.SetDestinationAngle(270f);  }},
                { new[] {  keyMap[ControlMapping.Right] },()=> {rotator.SetDestinationAngle(90f);  }},
                { new[] {  keyMap[ControlMapping.Up] },()=> {rotator.SetDestinationAngle(0f);  }},
                { new[] {  keyMap[ControlMapping.Down] },()=> { rotator.SetDestinationAngle(180f);  }},
            }, () => rotator.StopRotation());
        }

    }

}
