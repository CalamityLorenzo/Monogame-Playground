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
        public float AnglesPerSecond { get; private set; }
        public RotatorState State { get; private set; }
        public float DestinationAngle { get; private set; }

        public Rotator(int startAngle, float anglesPerSecond)
        {
            CurrentAngle = startAngle;
            this.DestinationAngle = startAngle;
            AnglesPerSecond = anglesPerSecond;
            this.State = RotatorState.Unknown;
        }

        public void SetDestinationAngle(float angleToSet)
        {
            this.DestinationAngle = angleToSet % 360;
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
        }

        public void Update(float delta)
        {
            // delta time since last update
            if (this.State != RotatorState.Stopped && this.State != RotatorState.Unknown)
            {
                // if the integer matches we can stop
                if (Math.Floor(CurrentAngle) == Math.Floor(DestinationAngle))
                {
                    this.State = RotatorState.Stopped;
                    this.CurrentAngle = DestinationAngle;
                }
                else
                {
                    UpdatePosition(delta);
                }
            }
        }

        private void UpdatePosition(float delta)
        {
            switch (this.State)
            {
                case RotatorState.Increase:
                    this.CurrentAngle = (CurrentAngle + (AnglesPerSecond * delta)) % 360f;
                    break;
                case RotatorState.Decrease:
                    this.CurrentAngle = (CurrentAngle - (AnglesPerSecond * delta)) % 360f;
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
