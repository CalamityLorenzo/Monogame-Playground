using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Drawing
{
    public class Bresenham
    {
        // We have a vector and a points method.
        public static List<Point> GetLine(Point StartPos, Point EndPos)
        {
            List<Point> AllPoints = new List<Point>();
            // Vertical Line
            if (StartPos.X == EndPos.X && StartPos.Y != EndPos.Y)
            {
                var y0 = 0;
                var y1 = 0;
                // Which end is up
                if (EndPos.Y < StartPos.Y)
                {
                    y0 = EndPos.Y;
                    y1 = StartPos.Y;
                }
                else
                {
                    y0 = StartPos.Y;
                    y1 = EndPos.Y;
                }

                for (int y = y0; y < y1; y++)
                {
                    AllPoints.Add(new Point(StartPos.X, y));
                }
                return AllPoints;
            }
            // Horizontal Line
            if (StartPos.Y == EndPos.Y && EndPos.X != StartPos.Y)
            {
                var x0 = 0;
                var x1 = 0;
                // which end is most left.
                if (EndPos.X < StartPos.X)
                {
                    x0 = EndPos.X;
                    x1 = StartPos.X;
                }
                else
                {
                    x0 = StartPos.X;
                    x1 = EndPos.X;
                }

                for (int x = x0; x < x1; ++x)
                {
                    AllPoints.Add(new Point(x, StartPos.Y));
                }
                return AllPoints;
            }

            // All the other lines
            //  Is the total height, longer than than the total width
            bool steep = Math.Abs(EndPos.Y - StartPos.Y) > Math.Abs(EndPos.X - StartPos.X);

            // Exchange positions
            if (steep)
            {
                StartPos = new Point(StartPos.Y, StartPos.X);
                EndPos = new Point(EndPos.Y, EndPos.X);
            }
            // we draw from left to right/lowest to highest
            // so work out which one is closest and fliip them if required.
            if (StartPos.X > EndPos.X)
            {
                var Temp = new Point(StartPos.X, StartPos.Y); ;
                StartPos = new Point(EndPos.X, EndPos.Y);
                EndPos = new Point(Temp.X, Temp.Y);

            }

            // THe difference between the two Vector2s
            int diffX = EndPos.X - StartPos.X;
            int diffY = Math.Abs(EndPos.Y - StartPos.Y);
            var error = diffX / 2;

            // Are we going up or down?
            int yStep = (StartPos.Y < EndPos.Y) ? 1 : -1;
            // Where Y is is starting
            var currentY = StartPos.Y;

            // Horizontal dimension of the line
            // Bresenahm only tracks x, and updates y as the error builds
            for (int x = StartPos.X; x <= EndPos.X; ++x)
            {
                Point nextPoint;
                if (steep)
                {
                    nextPoint = new Point(currentY, x);
                }
                else
                {
                    nextPoint = new Point(x, currentY);
                }

                /*yield return nextPoint*/;
                AllPoints.Add(nextPoint);
                error = error - diffY;
                if (error < 0)
                {
                    currentY += yStep;
                    error += diffX;
                }

            }
            return AllPoints;
        }
    
        public static IEnumerable<Vector2> GetLine(Vector2 StartPosition, Vector2 EndPosition)
        {

            var StartPos = StartPosition.ToPoint();
            var EndPos = EndPosition.ToPoint();

            // Vertical Line
            if (StartPos.X == EndPos.X && StartPos.Y != EndPos.Y)
            {
                var y0 = 0;
                var y1 = 0;
                // Which end is up
                if (EndPos.Y < StartPos.Y)
                {
                    y0 = EndPos.Y;
                    y1 = StartPos.Y;
                }
                else
                {
                    y0 = StartPos.Y;
                    y1 = EndPos.Y;
                }

                for (int y = y0; y < y1; y++)
                {
                    yield return new Vector2(StartPos.X, y);
                }
            }
            // Horizontal Line
            if (StartPos.Y == EndPos.Y && EndPos.X != StartPos.Y)
            {
                var x0 = 0;
                var x1 = 0;
                // which end is left.
                if (EndPos.X < StartPos.X)
                {
                    x0 = EndPos.X;
                    x1 = StartPos.X;
                }
                else
                {
                    x0 = StartPos.X;
                    x1 = EndPos.X;
                }

                for (int x = x0; x < x1; ++x)
                {
                    yield return new Vector2(x, StartPos.Y);
                }
            }

            // All the other lines
            //iS the total height, longer than than the total width
            bool steep = Math.Abs(EndPos.Y - StartPos.Y) > Math.Abs(EndPos.X - StartPos.X);

            // Exchange positions
            if (steep)
            {
                StartPos = new Point(StartPos.Y, StartPos.X);
                EndPos = new Point(EndPos.Y, EndPos.X);
            }
            // if the start horiz is in front of the end, flipem over.
            if (StartPos.X > EndPos.X)
            {
                var Temp = new Point(StartPos.X, StartPos.Y); ;
                StartPos = new Point(EndPos.X, EndPos.Y);
                EndPos = new Point(Temp.X, Temp.Y);

            }

            // THe difference between the two Vector2s
            int diffX = EndPos.X - StartPos.X;
            int diffY = Math.Abs(EndPos.Y - StartPos.Y);
            var error = diffX / 2;

            // Are we going up or down?
            int yStep = (StartPos.Y < EndPos.Y) ? 1 : -1;
            // Where Y is is starting
            var currentY = StartPos.Y;

            // Horizontal dimension of the line
            // Bresenahm only tracks x, and updates y as the error builds
            for (int x = StartPos.X; x <= EndPos.X; ++x)
            {
                Vector2 DrawVector;
                if (steep)
                {
                    DrawVector = new Vector2(currentY, x);
                }
                else
                {
                    DrawVector = new Vector2(x, currentY);
                }

                yield return DrawVector;

                error = error - diffY;
                if (error < 0)
                {
                    currentY += yStep;
                    error += diffX;
                }
            }
        }
    }
}
