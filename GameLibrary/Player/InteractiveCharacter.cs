using GameLibrary.Interfaces;
using GameLibrary.PlayerThings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Player
{
    public class InteractiveCharacter : IGameContainerDrawing
    {
        private KeyboardState _previousKeyboard;
        private KeyboardState _currentKeyboard;

        private readonly Dictionary<ControlMapping, Keys> _keyMap;

        public InteractiveCharacter(Dictionary<ControlMapping, Keys> keyMap)
        {
            this._keyMap = keyMap;
        }

        public void Update(GameTime time, KeyboardState keystate, GamePadState padState)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

    }
}
