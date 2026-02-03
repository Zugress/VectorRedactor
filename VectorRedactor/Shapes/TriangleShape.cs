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
                using (var pen = new Pen(Color.Red, 2)
                {
                    DashPattern = new float[] { 3, 3 }
                })
                {
                    graphics.DrawPolygon(pen, points);

                    graphics.DrawRectangle(pen,
                        Location.X - 2,
                        Location.Y - 2,
                        Size.Width + 4,
                        Size.Height + 4);
                }
            }
        }

        public override bool Contains(Point point)
        {
            return new Rectangle(Location, Size).Contains(point);
        }
    }
}