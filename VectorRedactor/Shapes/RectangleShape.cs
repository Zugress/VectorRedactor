using System.Drawing;

namespace VectorEditor
{
    public class RectangleShape : Shape
    {
        public override void Draw(Graphics graphics)
        {
            using (var brush = new SolidBrush(FillColor))
            {
                graphics.FillRectangle(brush, new Rectangle(Location, Size));
            }

            if (IsSelected)
            {
                using (var pen = new Pen(Color.Red, 2)
                {
                    DashPattern = new float[] { 3, 3 }
                })
                {
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