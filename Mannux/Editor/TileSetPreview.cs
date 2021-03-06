
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Editor {
    delegate void ChangeTileHandler(int t);

    class TileSetPreview : ScrollableControl {
        class DoubleBufferedPanel : Panel {
            public DoubleBufferedPanel() {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint,
                    true
                );
            }
        }

        Tileset tileset;
        Panel panel = new DoubleBufferedPanel();

        public TileSetPreview(Tileset t) {
            Text = "Tile set";

            BackColor = Color.Black;

            tileset = t;

            AutoScroll = true;

            DoubleBuffered = true;
            panel.Size = new Size(t.Bitmap.Width, t.Bitmap.Height);
            Controls.Add(panel);
            panel.Paint += Draw;
            panel.Show();

            ClientSize = new Size(t.Bitmap.Width, ClientSize.Height);
            Refresh();

            Resize += OnResize;
            panel.MouseDown += OnClick;
        }

        void Draw(object o, PaintEventArgs e) {
            e.Graphics.DrawImage(tileset.Bitmap, new Rectangle(0, 0, tileset.Bitmap.Width, tileset.Bitmap.Height));
        }

        void OnResize(object o, EventArgs e) {
            Refresh();
        }

        void OnClick(object o, MouseEventArgs e) {
            int i = tileset.TileFromPoint(e.X, e.Y);

            Console.WriteLine("Click {0},{1} on tile: {2}", e.X, e.Y, i);
            Point p = tileset.PointFromTile(i);
            Console.WriteLine("{0},{1}", p.X, p.Y);

            if (ChangeTile != null) {
                ChangeTile(i);
            }
        }

        public event ChangeTileHandler ChangeTile;
    }
}
