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
        Increase,
        Decrease,
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
                    this.State = RotatorState.Decrease;
                }
                else
                {
                    this.State = RotatorState.Increase;
                }
            }
            else
            {
                if (CurrentAngle - DestinationAngle > 179f)
                    this.State = RotatorState.Increase;
                else
                {
                    this.State = RotatorState.Decrease;

                }
            }
        }

        public void SetState(RotatorState state)
        {
            this.State = state;
        }

        // Basically when you lift a finer it stops
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
                    //// if the integer matches we can stop
                    //if (Math.Floor(CurrentAngle) == Math.Floor(DestinationAngle))
                    //{
                    this.State = RotatorState.Stopped;
                    this.CurrentAngle = DestinationAngle;
                    //}
                }
                this.PreviousAngle = CurrentAngle;

            }
        }

        private bool IsAngeMatched2(RotatorState state, float destinationAngle, float currentAngle, float previousAngle)
        {
            // Degenerate case 
            if (destinationAngle == currentAngle)
                return true;

            // COs we are dealing with velocities
            // we can miss our angle, so we need to check a range.
            // Howver it's not a simple number line, but a clock. so caution is erquired,
            if (state != RotatorState.Increase && state != RotatorState.Decrease)
                throw new Exception("Rotator all out of whack");
            // 1. Get the difference between current and previous update 
            var angleRange = (state == RotatorState.Increase) ? currentAngle - previousAngle : previousAngle - currentAngle;


            return false;
        }

        private bool IsAngleMatched(RotatorState state, float destinationAngle, float currentAngle, float previousAngle)
        {
            // Degenerate case 
            if (destinationAngle == currentAngle)
                return true;

            // COs we are dealing with velocities
            // we can miss our angle, so we need to check a range.
            // Howver it's not a simple number line, but a clock. so caution is erquired,
            if (state != RotatorState.Increase && state != RotatorState.Decrease)
                throw new Exception("Rotator all out of whack");
            // 1. Get the difference between current and previous update 
            var angleRange = (state == RotatorState.Increase) ? currentAngle - previousAngle : previousAngle - currentAngle;
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

            if (state == RotatorState.Increase)
            {
                //lowerbound = (currentAngle - angleDistance > 0) ? currentAngle - angleDistance : (destinationAngle != 0f) ? 360 + (currentAngle - angleDistance) : currentAngle - angleDistance;
                lowerbound = (currentAngle - angleDistance > 0) ? currentAngle - angleDistance : 360 + (currentAngle - angleDistance);

                upperbound = (int)Math.Floor(currentAngle);
            }
            else
            {
                upperbound =  currentAngle + angleDistance;
                lowerbound = (int)Math.Floor(currentAngle);

            }

            /// swap if one twas bigger than t'other
            if (lowerbound > upperbound)
            {
                var temp = lowerbound;
                lowerbound = upperbound;
                upperbound = temp;
            }

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
                case RotatorState.Increase:
                    this.CurrentAngle = (CurrentAngle + (RatePerSecond * delta)) % 360f;
                    break;
                case RotatorState.Decrease:
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
