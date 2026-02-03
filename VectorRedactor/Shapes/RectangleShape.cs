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
                using (var pen = new Pen(Color.Black, 1) { DashPattern = new float[] { 2, 2 } })
                {
                    graphics.DrawRectangle(pen, new Rectangle(Location, Size));
                }
            }
        }

        public override bool Contains(Point point)
        {
            return new Rectangle(Location, Size).Contains(point);
        }
    }
}