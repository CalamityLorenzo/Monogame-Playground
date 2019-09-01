using GameLibrary.Extensions;
using GameLibrary.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.AppObjects
{

    public enum FourDirections
    {
        Unknown = 0,
        Up,
        Down,
        Left,
        Right,
        Stopped

    }

    public class FourWayDirection : IGameObjectUpdate
    {
        private FourDirections currentDirection;

        public Vector2 CurrentDirectionVector { get; private set; }

        private FourDirections _previousDirection;
        private float velocity;
        private float referenceVelocity;
        public float Velocity() => velocity;
        public FourWayDirection(FourDirections initialDirection, float initialVelocity)
        {
            currentDirection = initialDirection;
            if (currentDirection != FourDirections.Unknown)
                _previousDirection = FourDirections.Unknown;
            this.CurrentDirectionVector = GetVectorFromDirection(currentDirection);
            velocity = initialVelocity;
            referenceVelocity = initialVelocity;
        }

        public void Update(float mlSinceupdate)
        {

            if ((currentDirection != FourDirections.Unknown) && currentDirection != _previousDirection)
            {
                if (currentDirection != FourDirections.Stopped)
                {
                    this.velocity = referenceVelocity;
                    CurrentDirectionVector = GetVectorFromDirection(currentDirection);
                }
                else
                {
                    this.velocity = 0f;
                }
                _previousDirection = currentDirection;
            }


        }

        private Vector2 GetVectorFromDirection(FourDirections direction)
        {
            switch (direction)
            {
                case FourDirections.Up:
                    return GeneralExtensions.UnitAngleVector(0f);
                case FourDirections.Left:
                    return GeneralExtensions.UnitAngleVector(270f);
                case FourDirections.Right:
                    return GeneralExtensions.UnitAngleVector(90f);
                case FourDirections.Down:
                    return GeneralExtensions.UnitAngleVector(180f);
                case FourDirections.Stopped:
                    return CurrentDirectionVector;
                case FourDirections.Unknown:
                    throw new ArgumentException("Where are you going?s");
                    break;
                default:
                    return GeneralExtensions.UnitAngleVector(123f);
            }
        }

        public void SetDirection(FourDirections direction)
        {
            if (this.currentDirection != direction)
                this.currentDirection = direction;
        }
    }
}
