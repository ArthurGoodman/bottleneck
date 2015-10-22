using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace bottleneck {
    public partial class MainForm : Form {
        private Bitmap input, output;
        private Random random = new Random();

        private float scale = 1.0f;
        private PointF offset, lastPos;

        public MainForm() {
            InitializeComponent();

            CenterToScreen();

            DoubleBuffered = true;

            Timer t = new Timer();
            t.Interval = 10;
            t.Tick += new EventHandler(Tick);
            t.Start();

            input = new Bitmap(100, 100);
            output = new Bitmap(100, 100);

            for (int x = 0; x < input.Width; x++)
                for (int y = 0; y < input.Height; y++) {
                    input.SetPixel(x, y, Color.FromArgb(255, random.Next() % 256, random.Next() % 256, random.Next() % 256));
                    output.SetPixel(x, y, Color.FromArgb(255, random.Next() % 256, random.Next() % 256, random.Next() % 256));
                }
        }

        private void Defaults() {
            scale = 1.0f;
            offset = new PointF();
        }

        private void Tick(object sender, EventArgs e) {
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Escape:
                    Close();
                    break;

                case Keys.Back:
                    Defaults();
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            lastPos.X = e.X;
            lastPos.Y = e.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            switch (e.Button) {
                case MouseButtons.Left:
                    offset.X += e.X - lastPos.X;
                    offset.Y += e.Y - lastPos.Y;

                    lastPos.X = e.X;
                    lastPos.Y = e.Y;
                    break;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            scale *= e.Delta > 0 ? 1.1f : 1.0f / 1.1f;
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));

            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            g.TranslateTransform(ClientSize.Width / 2 + offset.X, ClientSize.Height / 2 + offset.Y);
            g.ScaleTransform(scale, scale);

            g.DrawImage(input, -input.Width - 10, -input.Height / 2);
            g.DrawImage(output, 10, -output.Height / 2);
        }
    }
}
