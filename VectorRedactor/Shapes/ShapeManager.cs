using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VectorEditor
{
    public class ShapeManager
    {
        private List<Shape> shapes = new List<Shape>();

        public IEnumerable<Shape> Shapes => shapes;

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void RemoveShape(Shape shape)
        {
            shapes.Remove(shape);
        }

        public void ClearSelection()
        {
            foreach (var shape in shapes)
            {
                shape.IsSelected = false;
            }
        }

        public Shape SelectShape(Point point)
        {
            var selected = shapes.LastOrDefault(s => s.Contains(point));

            ClearSelection();
            if (selected != null)
            {
                selected.IsSelected = true;
            }

            return selected;
        }

        public void DrawAll(Graphics graphics)
        {
            foreach (var shape in shapes)
            {
                shape.Draw(graphics);
            }
        }
    }
}