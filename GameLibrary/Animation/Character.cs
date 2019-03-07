using GameLibrary.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Animation
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
        public Character(Rectangle[] displayFrames)
        {
            DisplayFrames = displayFrames;
            PreviousState = JeepState.Unknown;
            CurrentState = JeepState.Unknown;
        }

        public Rectangle[] DisplayFrames { get; }
        public JeepState CurrentState { get; private set}
        private JeepState PreviousState { get; set; }
        public void Update(float mlSinceupdate)
        {
            // If we had animation then things woulf be occuring here.

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
