using GameLibrary.AppObjects;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrallax.Eightway
{
    class Keyboard4Way
    {
        private readonly FourWayDirection _fourwayDirection;
        private KeyboardManager _keyManager = new KeyboardManager();
        private readonly Dictionary<ControlMapping, Keys> _keyMap;

        public Keyboard4Way(FourWayDirection fourwayDirection, Dictionary<ControlMapping, Keys> keyMap)
        {
            this._fourwayDirection = fourwayDirection;
            this._keyMap = keyMap;
            CreateKeyboardMappings(_keyManager, _keyMap, _fourwayDirection);
        }

        private void CreateKeyboardMappings(KeyboardManager keyManager, Dictionary<ControlMapping, Keys> keyMap, FourWayDirection fourwayDirection)
        {
            keyManager.AddMovingActions(new Dictionary<IEnumerable<Keys>, Action>
            {
                { new []{ keyMap[ControlMapping.Up]}, ()=>fourwayDirection.SetDirection(FourDirections.Up) },
                { new []{ keyMap[ControlMapping.Down]}, ()=>fourwayDirection.SetDirection(FourDirections.Down) },
                { new []{ keyMap[ControlMapping.Left]}, ()=>fourwayDirection.SetDirection(FourDirections.Left) },
                { new []{ keyMap[ControlMapping.Right]}, ()=>fourwayDirection.SetDirection(FourDirections.Right) },
            }, () => fourwayDirection.SetDirection(FourDirections.Stopped));

        }

        public void Update(GameTime time, KeyboardState keystate, GamePadState padState)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;
            _keyManager.Update(delta, keystate);
        }


    }
}
