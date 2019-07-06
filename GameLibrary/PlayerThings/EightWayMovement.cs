using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.PlayerThings
{
    public enum EightWayPositions // Yes there is nine
    {
        Dead = 0,
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }
    // This class allows the an object to move in 8 directions, and stop
    // It hjas nothing to do with any inputs nor if a person is controlling it.
    // HOW EIghtWayPosition is reached, is different code.
    public class EightWayMovement
    {
        private EightWayPositions _previousDirection;
        private EightWayPositions _currentDirection;
        internal Dictionary<EightWayPositions, Action> ActionToTake = new Dictionary<EightWayPositions, Action>();
        public EightWayMovement( Dictionary<EightWayPositions, Action> Actions)
        {
            this._currentDirection = EightWayPositions.Dead;
            this._previousDirection = EightWayPositions.Dead;
        }

        public void Update(float Deltatime, EightWayPositions eightWayPositions)
        {
            if (this._currentDirection != eightWayPositions)
            {
                this._previousDirection = this._currentDirection;
                this._currentDirection = eightWayPositions;
                ActionToTake[this._currentDirection]();
            }
        }

    }
}
