using GameLibrary.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamePlayground.Animation
{

    public enum JeepState
    {
        Unknown=0,
        Stopped,
        North,
        NorthNorthEast,
        NorthEast,
        East,
        SouthEast,
        SouthSouthEast,
        South,
        SouthSouthWest,
        SouthWest,
        West,
        NorthWest,
        NorthNorthWest
    }
    // where the animation lives and the hitboxes are set.
    // frames, Textures, bounding boxes
    public class Character : IGameObjectUpdate
    {
        public Rectangle CurrentDisplayFrame { get; private set; }
        public Rectangle[] DisplayFrames { get; }
        public JeepState CurrentState { get; private set; }

        public Character(Rectangle[] displayFrames)
        {
            DisplayFrames = displayFrames;
            PreviousState = JeepState.Unknown;
            CurrentState = JeepState.Unknown;
        }

        private JeepState PreviousState { get; set; }


        public void Update(float mlSinceupdate)
        {
            // If we had animation then things woulf be occuring here.
            if (PreviousState != CurrentState)
            {
                UpdateCurrentFrame(CurrentState);
            }
        }

        private void UpdateCurrentFrame(JeepState characterState)
        {
            switch (characterState)
            {
                case JeepState.North:
                    this.CurrentDisplayFrame = this.DisplayFrames[0];
                    break;
                case JeepState.NorthNorthEast:
                    this.CurrentDisplayFrame = this.DisplayFrames[1];
                    break;
                case JeepState.NorthEast:
                    this.CurrentDisplayFrame = this.DisplayFrames[2];
                    break;
                case JeepState.East:
                    this.CurrentDisplayFrame = this.DisplayFrames[3];
                    break;
                case JeepState.South:
                    this.CurrentDisplayFrame = this.DisplayFrames[6];

                    break;
                case JeepState.SouthSouthEast:
                    this.CurrentDisplayFrame = this.DisplayFrames[5];

                    break;
                case JeepState.SouthEast:
                    this.CurrentDisplayFrame = this.DisplayFrames[4];

                    break;
                case JeepState.SouthSouthWest:
                    this.CurrentDisplayFrame = this.DisplayFrames[7];
                    break;
                case JeepState.SouthWest:
                    this.CurrentDisplayFrame = this.DisplayFrames[8];
                    break;
                case JeepState.West:
                    this.CurrentDisplayFrame = this.DisplayFrames[9];
                    break;
                case JeepState.NorthNorthWest:
                    this.CurrentDisplayFrame = this.DisplayFrames[11];
                    break;
                case JeepState.NorthWest:
                    this.CurrentDisplayFrame = this.DisplayFrames[10];
                    break;
            }
        }

        public void SetState(JeepState newState)
        {
            if (newState == CurrentState)
                return;
            PreviousState = CurrentState;
            CurrentState = newState;
        }
    }
}
