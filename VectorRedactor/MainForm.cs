using System;
using System.Drawing;
using System.Windows.Forms;

namespace VectorEditor
{
    public partial class MainForm : Form
    {
        private ShapeManager shapeManager = new ShapeManager();
        private Color currentColor = Color.Blue;
        private Point startPoint;
        private bool isDrawing = false;
        private Shape currentShape = null;
        private Tool currentTool = Tool.Select;

        private enum Tool { Select, Rectangle, Ellipse, Triangle }

        public MainForm()
        {
            InitializeComponent();
            SetupToolbar();
            SetupCanvas();
        }

        private void SetupToolbar()
        {
            var colorButton = new Button
            {
                Text = "Color",
                Location = new Point(10, 10),
                Size = new Size(80, 30)
            };
            colorButton.Click += ColorButton_Click;
            this.Controls.Add(colorButton);

            var selectButton = new Button
            {
                Text = "Select",
                Location = new Point(100, 10),
                Size = new Size(80, 30)
            };
            selectButton.Click += (s, e) => currentTool = Tool.Select;
            this.Controls.Add(selectButton);

            var rectButton = new Button
            {
                Text = "Rectangle",
                Location = new Point(190, 10),
                Size = new Size(80, 30)
            };
            rectButton.Click += (s, e) => currentTool = Tool.Rectangle;
            this.Controls.Add(rectButton);

            var ellipseButton = new Button
            {
                Text = "Ellipse",
                Location = new Point(280, 10),
                Size = new Size(80, 30)
            };
            ellipseButton.Click += (s, e) => currentTool = Tool.Ellipse;
            this.Controls.Add(ellipseButton);

            var triangleButton = new Button
            {
                Text = "Triangle",
                Location = new Point(370, 10),
                Size = new Size(80, 30)
            };
            triangleButton.Click += (s, e) => currentTool = Tool.Triangle;
            this.Controls.Add(triangleButton);
        }

        private void SetupCanvas()
        {
            var canvas = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;

            this.Controls.Add(canvas);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            shapeManager.DrawAll(e.Graphics);
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;

            if (currentTool == Tool.Select)
            {
                currentShape = shapeManager.SelectShape(e.Location);
                ((Panel)sender).Invalidate();
            }
            else
            {
                isDrawing = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || currentTool == Tool.Select) return;

            Panel canvas = (Panel)sender;
            int width = e.X - startPoint.X;
            int height = e.Y - startPoint.Y;

            // Временная отрисовка
            using (var g = canvas.CreateGraphics())
            {
                // Очищаем только область рисования
                g.Clear(Color.White);
                shapeManager.DrawAll(g);

                var rect = new Rectangle(startPoint.X, startPoint.Y, width, height);
                using (var brush = new SolidBrush(Color.FromArgb(128, currentColor))) // Полупрозрачный цвет
                {
                    switch (currentTool)
                    {
                        case Tool.Rectangle:
                            g.FillRectangle(brush, rect);
                            break;
                        case Tool.Ellipse:
                            g.FillEllipse(brush, rect);
                            break;
                        case Tool.Triangle:
                            Point[] points = new Point[]
                            {
                                new Point(startPoint.X + width / 2, startPoint.Y),
                                new Point(startPoint.X, startPoint.Y + height),
                                new Point(startPoint.X + width, startPoint.Y + height)
                            };
                            g.FillPolygon(brush, points);
                            break;
                    }
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            Panel canvas = (Panel)sender;
            int width = e.X - startPoint.X;
            int height = e.Y - startPoint.Y;

            if (Math.Abs(width) > 2 && Math.Abs(height) > 2) // Минимальный размер
            {
                Shape newShape = null;
                var location = new Point(
                    Math.Min(startPoint.X, e.X),
                    Math.Min(startPoint.Y, e.Y));
                var size = new Size(Math.Abs(width), Math.Abs(height));

                switch (currentTool)
                {
                    case Tool.Rectangle:
                        newShape = new RectangleShape();
                        break;
                    case Tool.Ellipse:
                        newShape = new EllipseShape();
                        break;
                    case Tool.Triangle:
                        newShape = new TriangleShape();
                        break;
                }

                if (newShape != null)
                {
                    newShape.Location = location;
                    newShape.Size = size;
                    newShape.FillColor = currentColor;
                    shapeManager.AddShape(newShape);
                }
            }

            isDrawing = false;
            canvas.Invalidate();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = colorDialog.Color;
            }
        }
    }
}