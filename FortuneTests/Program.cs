using System;

namespace FortuneTests
{
    class Program
    {
        static void Main(string[] args)
        {

            var inboundsPosition = new Vector2() { X = 0, Y = 0 };
            var destination = new Dimensions { Height = 600, Width = 150 };
            var tileDimensions = new Dimensions { Height = 25, Width = 25 };
            var frameDimensiosn = new Dimensions { Height = 500, Width = 500 };

            var moduloXOffset = (int)Math.Floor(inboundsPosition.X) % tileDimensions.Width;
            var moduloYOffset = (int)Math.Floor(inboundsPosition.Y) % tileDimensions.Height;

            var normalisedPosition = new Vector2(inboundsPosition.X - moduloXOffset, inboundsPosition.Y - moduloYOffset);

            var displayRowLength = destination.Width / tileDimensions.Width;
            var displayColLength = destination.Height / tileDimensions.Height;

            // Each step is the width/height of a rectangle.
            // So if you have the fist position correct...Then it's an additive process.
            for (var y = 0; y < displayRowLength; y += 1)
            {
                // convert our currentTopLeft (currentPosition) 
                // into a position on our map.
                for (var x = 0; x < displayColLength; x += 1)
                {
                    var backgroundPosition = Program.EnsureBoundries(new Vector2(normalisedPosition.X + x*25, normalisedPosition.Y + y*25), frameDimensiosn.Width, frameDimensiosn.Height);

                    Console.Write($"{backgroundPosition}");// : ({x},{y})");
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
        }

        private static Vector2 EnsureBoundries(Vector2 v, int totalWidth, int totalHeight)
        {
            var x = v.X;
            var y = v.Y;
            if (x < 0)
                x = totalWidth + x;
            if (y < 0)
                y = totalWidth + y;
            return new Vector2(x >= totalWidth ? x - totalWidth : x, y >= totalHeight ? y - totalHeight : y);
        }
    }

    public class Vector2
    {

        public Vector2() { }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public override string? ToString()
        {
            return $"({X},{Y})";
        }
    }

    public class Dimensions
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
