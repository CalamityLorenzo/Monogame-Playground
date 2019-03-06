namespace GameLibrary.Animation
{
    // DEscription object for a frame
    // USe this to create the rectangle for the frame from imported data.
    public class FrameInfo
    {
        public FrameInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }

        FrameInfo(int width, int height, int x, int y):this(width, height)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public int Width { get; }
        public int Height { get; }
    }
}