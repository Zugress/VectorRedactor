using System.Drawing;

namespace VectorEditor
{
    public class EllipseShape : Shape
    {
        public override void Draw(Graphics graphics)
        {
            using (var brush = new SolidBrush(FillColor))
            {
                graphics.FillEllipse(brush, new Rectangle(Location, Size));
            }

            if (IsSelected)
            {
                using (var pen = new Pen(Color.Black, 1) { DashPattern = new float[] { 2, 2 } })
                {
                    graphics.DrawEllipse(pen, new Rectangle(Location, Size));
                }
            }
        }

        public override bool Contains(Point point)
        {
            return new Rectangle(Location, Size).Contains(point);
        }
    }
}