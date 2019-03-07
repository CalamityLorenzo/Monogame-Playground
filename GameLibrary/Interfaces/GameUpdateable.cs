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
}
