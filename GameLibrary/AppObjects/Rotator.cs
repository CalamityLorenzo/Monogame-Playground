using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.AppObjects
{

    public enum RotatorState
    {
        Unknown = 0,
        Clockwise,
        Widdershins,
        Stopped
    }

    public class Rotator : IGameObjectUpdate
    {

        public float CurrentAngle { get; private set; }
        private float PreviousAngle { get; set; }
        public float RatePerSecond { get; private set; }
        public RotatorState State { get; private set; }
        public float DestinationAngle { get; private set; }

        public Rotator(int startAngle, float anglesPerSecond)
        {
            CurrentAngle = startAngle;
            this.PreviousAngle = CurrentAngle;
            this.DestinationAngle = startAngle;
            RatePerSecond = anglesPerSecond;
            this.State = RotatorState.Unknown;
        }

        public void UpdateRate(float anglesPerSecond)
        {
            RatePerSecond = anglesPerSecond;
        }

        public void SetDestinationAngle(float angleToSet)
        {
            this.DestinationAngle = angleToSet % 360;
            if (DestinationAngle == CurrentAngle) return;
            // Now set the Wsdirect we need to go.
            if (this.DestinationAngle > CurrentAngle)
            {
                if (DestinationAngle - CurrentAngle > 179f)
                {
                    this.State = RotatorState.Widdershins;
                }
                else
                {
                    this.State = RotatorState.Clockwise;
                }
            }
            else
            {
                if (CurrentAngle - DestinationAngle > 179f)
                    this.State = RotatorState.Clockwise;
                else
                {
                    this.State = RotatorState.Widdershins;

                }
            }
        }

        public void SetState(RotatorState state)
        {
            this.State = state;
        }

        // Basically when you lift a finger it stops
        public void StopRotation()
        {
            this.State = RotatorState.Stopped;
            this.PreviousAngle = CurrentAngle;
        }

        public void Update(float delta)
        {
            // delta time since last update
            // are we stopped or moving?
            if (this.State != RotatorState.Stopped && this.State != RotatorState.Unknown)
            {

                UpdatePosition(delta);

                if (IsAngleMatched(this.State, DestinationAngle, CurrentAngle, PreviousAngle))
                {
                    this.State = RotatorState.Stopped;
                    this.CurrentAngle = DestinationAngle;
                }

                this.PreviousAngle = CurrentAngle;
            }
        }

        private bool IsAngleMatched(RotatorState state, float destinationAngle, float currentAngle, float previousAngle)
        {
            // Degenerate case 
            if (destinationAngle == currentAngle)
                return true;

            // COs we are dealing with velocities
            // we can miss our angle, so we need to check a range.
            // Howver it's not a simple number line, but a clock. so caution is erquired,
            if (state != RotatorState.Clockwise && state != RotatorState.Widdershins)
                throw new Exception("Rotator all out of whack");
            // 1. Get the difference between current and previous update 
            var angleRange = (state == RotatorState.Clockwise) ? currentAngle - previousAngle : previousAngle - currentAngle;
            // 2. Make sure it's modulo 360 (handling negatives)
            var cleanNegative = 0f;
            //cleanNegative = (angleRange < 0f) ? 
            //        360f + angleRange 
            //        : angleRange;
            if (angleRange < 0f)
                cleanNegative = 360f + angleRange;
            else
                cleanNegative = angleRange;

            var angleDistance = angleRange; // (float)Math.Floor(cleanNegative % 360);
            // 3. Is Our angle in the range from Current to Current-DistanceSinceLastUpdate.
            var lowerbound = 0f;
            var upperbound = 0f;

            if (state == RotatorState.Clockwise)
            {
                lowerbound = currentAngle - angleDistance;
                upperbound = (int)Math.Floor(currentAngle);
                if (lowerbound > upperbound)
                {
                    var temp = lowerbound;
                    upperbound = upperbound + 360f;
                }
                // if upper is greater than lower
                // mash it from 360f, as we have clockwised the clock.
                if (lowerbound > upperbound)
                {
                    lowerbound = lowerbound - 360f;
                }
            }
            else
            {
                lowerbound = (int)Math.Floor(currentAngle);
                upperbound = (angleDistance > 0f) ? currentAngle + angleDistance : 360f + angleDistance;
                // if lower is greater than upper
                // subtract it from 360f, we have widdershined around the clock.
                if (lowerbound > upperbound)
                {
                    lowerbound = lowerbound - 360f;
                }
            }
            // Finally is our destination angle between the lower/upperbound
            if (lowerbound <= destinationAngle && upperbound >= destinationAngle)
            {
                return true;
            }
            return false;

        }

        private void UpdatePosition(float delta)
        {
            switch (this.State)
            {
                case RotatorState.Clockwise:
                    this.CurrentAngle = (CurrentAngle + (RatePerSecond * delta)) % 360f;
                    break;
                case RotatorState.Widdershins:
                    this.CurrentAngle = (CurrentAngle - (RatePerSecond * delta)) % 360f;
                    break;
                case RotatorState.Stopped:
                case RotatorState.Unknown:
                    this.CurrentAngle = CurrentAngle;
                    break;
            }

            if (this.CurrentAngle < 0)
            {
                // We add this becuase Current angle is a negative.
                // and require the modulo version of the angle.((Counterclockwise from 0)
                this.CurrentAngle = 360f + CurrentAngle;
            }
        }
    }
}
