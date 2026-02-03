using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VectorEditor
{
    public class ShapeManager
    {
        private List<Shape> shapes = new List<Shape>();
        private Shape selectedShape = null;

        public IEnumerable<Shape> Shapes => shapes;
        public Shape SelectedShape => selectedShape;

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public void RemoveSelectedShape()
        {
            if (selectedShape != null)
            {
                shapes.Remove(selectedShape);
                selectedShape = null;
            }
        }

        public void ClearSelection()
        {
            if (selectedShape != null)
            {
                selectedShape.IsSelected = false;
                selectedShape = null;
            }
        }

        public Shape SelectShape(Point point)
        {
            ClearSelection();

            selectedShape = shapes.LastOrDefault(s => s.Contains(point));

            if (selectedShape != null)
            {
                selectedShape.IsSelected = true;
            }

            return selectedShape;
        }

        public void StartMoving(Point startPoint)
        {
            if (selectedShape != null)
            {
                selectedShape.MoveOffset = new Point(
                    startPoint.X - selectedShape.Location.X,
                    startPoint.Y - selectedShape.Location.Y);
            }
        }

        public void MoveSelectedShape(Point newPosition)
        {
            if (selectedShape != null && selectedShape.MoveOffset.HasValue)
            {
                selectedShape.Location = new Point(
                    newPosition.X - selectedShape.MoveOffset.Value.X,
                    newPosition.Y - selectedShape.MoveOffset.Value.Y);
            }
        }

        public void FinishMoving()
        {
            if (selectedShape != null)
            {
                selectedShape.MoveOffset = null;
            }
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