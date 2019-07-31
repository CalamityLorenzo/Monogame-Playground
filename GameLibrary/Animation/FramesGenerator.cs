using GameLibrary.AppObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Animation
{
    public static class FramesGenerator
    {
        // can be irrefgular so no real rows/columns
        public static Rectangle[] GenerateFrames(FrameInfo[] frameInfo, Dimensions atlasSize)
        {
            // Calculate the required frames
            if (frameInfo.Length == 1)
                return GenerateFrames(frameInfo[0], atlasSize);
            return frameInfo.Select(fr => new Rectangle(fr.X, fr.Y, fr.Width, fr.Height)).ToArray();
        }

        // ONe frame missing any positional information
        // can do an entire spritemap.
        public static Rectangle[] GenerateFrames(FrameInfo frameInfo, Dimensions atlasSize)
        {
            // 1 frame info for the whole set of frames.
            // cos the map is rational.
            List<Rectangle> results = new List<Rectangle>();
            var cellsPerRow = atlasSize.Width / frameInfo.Width;
            var cellsPerColumn = atlasSize.Height / frameInfo.Height;

            var cellWidth = atlasSize.Width / cellsPerRow;
            var cellHeight = atlasSize.Height / cellsPerColumn;

            var frameSize = new Point(frameInfo.Width, frameInfo.Height);
            // Across then down
            for (var y = 0; y < cellsPerColumn; ++y)
                for (var x = 0; x < cellsPerRow; ++x)
                {
                    // {x+cellsPerRow*y} 
                    results.Add(new Rectangle(new Point(cellWidth * x, cellHeight * y), frameSize));
                }

            return results.ToArray();
        }
    }
}
