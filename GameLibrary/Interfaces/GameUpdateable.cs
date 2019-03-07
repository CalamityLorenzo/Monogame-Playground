using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Interfaces
{
    // unlike monogame object we only pass the detla times
    // not the full time.
    public interface IGameObjectUpdate
    {
        void Update(float mlSinceupdate);
    }

    public interface IGameContainerUpdate
    {
        void Update(GameTime time, KeyboardState keystate, GamePadState padState);
    }
}
