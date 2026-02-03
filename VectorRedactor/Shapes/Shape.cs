using System.Drawing;

namespace VectorEditor
{
    public abstract class Shape
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Color FillColor { get; set; } = Color.Blue;
        public bool IsSelected { get; set; } = false;
        public Point? MoveOffset { get; set; } = null;

        public abstract void Draw(Graphics graphics);
        public abstract bool Contains(Point point);

        public void Move(Point offset)
        {
            Location = new Point(Location.X + offset.X, Location.Y + offset.Y);
        }
    }
}