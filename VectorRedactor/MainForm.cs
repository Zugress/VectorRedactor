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
        private bool isMoving = false;
        private Tool currentTool = Tool.Select;
        private Shape currentDrawingShape = null;

        private enum Tool { Select, Rectangle, Ellipse, Triangle }

        public MainForm()
        {
            InitializeComponent();
            SetupToolbar();
            SetupCanvas();
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
        }

        private void SetupToolbar()
        {
            var toolPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, 40),
                BackColor = SystemColors.Control
            };

            var colorButton = new Button
            {
                Text = "Color",
                Location = new Point(10, 8),
                Size = new Size(80, 24)
            };
            colorButton.Click += ColorButton_Click;
            toolPanel.Controls.Add(colorButton);

            var selectButton = new Button
            {
                Text = "Select",
                Location = new Point(100, 8),
                Size = new Size(80, 24),
                BackColor = SystemColors.Highlight,
                ForeColor = SystemColors.HighlightText
            };
            selectButton.Click += (s, e) =>
            {
                currentTool = Tool.Select;
                UpdateButtonSelection(selectButton);
            };
            toolPanel.Controls.Add(selectButton);

            var rectButton = new Button
            {
                Text = "Rectangle",
                Location = new Point(190, 8),
                Size = new Size(80, 24)
            };
            rectButton.Click += (s, e) =>
            {
                currentTool = Tool.Rectangle;
                UpdateButtonSelection(rectButton);
            };
            toolPanel.Controls.Add(rectButton);

            var ellipseButton = new Button
            {
                Text = "Ellipse",
                Location = new Point(280, 8),
                Size = new Size(80, 24)
            };
            ellipseButton.Click += (s, e) =>
            {
                currentTool = Tool.Ellipse;
                UpdateButtonSelection(ellipseButton);
            };
            toolPanel.Controls.Add(ellipseButton);

            var triangleButton = new Button
            {
                Text = "Triangle",
                Location = new Point(370, 8),
                Size = new Size(80, 24)
            };
            triangleButton.Click += (s, e) =>
            {
                currentTool = Tool.Triangle;
                UpdateButtonSelection(triangleButton);
            };
            toolPanel.Controls.Add(triangleButton);

            this.Controls.Add(toolPanel);
        }

        private void UpdateButtonSelection(Button selectedButton)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel && panel.Top == 0)
                {
                    foreach (Control btn in panel.Controls)
                    {
                        if (btn is Button button && button.Text != "Color")
                        {
                            button.BackColor = (button == selectedButton) ? SystemColors.Highlight : SystemColors.Control;
                            button.ForeColor = (button == selectedButton) ? SystemColors.HighlightText : SystemColors.ControlText;
                        }
                    }
                    break;
                }
            }
        }

        private void SetupCanvas()
        {
            var canvas = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 40),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;

            this.Controls.Add(canvas);
        }

        private Panel GetCanvas()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel && panel.Top == 40)
                {
                    return panel;
                }
            }
            return null;
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
                var shape = shapeManager.SelectShape(e.Location);
                if (shape != null)
                {
                    isMoving = true;
                    shapeManager.StartMoving(e.Location);
                }
                ((Panel)sender).Invalidate();
            }
            else
            {
                isDrawing = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Panel canvas = (Panel)sender;

            if (isMoving && shapeManager.SelectedShape != null)
            {
                shapeManager.MoveSelectedShape(e.Location);
                canvas.Invalidate();
            }
            else if (isDrawing && currentTool != Tool.Select)
            {
                int width = e.X - startPoint.X;
                int height = e.Y - startPoint.Y;

                if (Math.Abs(width) > 0 && Math.Abs(height) > 0)
                {
                    Shape tempShape = null;
                    var location = new Point(
                        Math.Min(startPoint.X, e.X),
                        Math.Min(startPoint.Y, e.Y));
                    var size = new Size(Math.Abs(width), Math.Abs(height));

                    switch (currentTool)
                    {
                        case Tool.Rectangle:
                            tempShape = new RectangleShape();
                            break;
                        case Tool.Ellipse:
                            tempShape = new EllipseShape();
                            break;
                        case Tool.Triangle:
                            tempShape = new TriangleShape();
                            break;
                    }

                    if (tempShape != null)
                    {
                        tempShape.Location = location;
                        tempShape.Size = size;
                        tempShape.FillColor = Color.FromArgb(128, currentColor);

                        using (var bitmap = new Bitmap(canvas.Width, canvas.Height))
                        using (var tempGraphics = Graphics.FromImage(bitmap))
                        {
                            shapeManager.DrawAll(tempGraphics);

                            tempShape.Draw(tempGraphics);

                            using (var g = canvas.CreateGraphics())
                            {
                                g.DrawImage(bitmap, 0, 0);
                            }
                        }
                    }
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            Panel canvas = (Panel)sender;

            if (isMoving)
            {
                shapeManager.FinishMoving();
                isMoving = false;
            }
            else if (isDrawing)
            {
                int width = e.X - startPoint.X;
                int height = e.Y - startPoint.Y;

                if (Math.Abs(width) > 2 && Math.Abs(height) > 2)
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
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && shapeManager.SelectedShape != null)
            {
                shapeManager.RemoveSelectedShape();
                GetCanvas()?.Invalidate();
            }
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            colorDialog.Color = currentColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = colorDialog.Color;
            }
        }
    }
}