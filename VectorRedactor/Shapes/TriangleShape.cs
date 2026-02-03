using System.Drawing;

namespace VectorEditor
{
    public class TriangleShape : Shape
    {
        public override void Draw(Graphics graphics)
        {
            Point[] points = new Point[]
            {
                new Point(Location.X + Size.Width / 2, Location.Y),
                new Point(Location.X, Location.Y + Size.Height),
                new Point(Location.X + Size.Width, Location.Y + Size.Height)
            };

            using (var brush = new SolidBrush(FillColor))
            {
                graphics.FillPolygon(brush, points);
            }

            if (IsSelected)
            {
                using (var pen = new Pen(Color.Black, 1) { DashPattern = new float[] { 2, 2 } })
                {
                    graphics.DrawPolygon(pen, points);
                }
            }
        }

        public override bool Contains(Point point)
        {
            return new Rectangle(Location, Size).Contains(point);
        }
    }
}