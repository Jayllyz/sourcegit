using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SourceGit.Views.Controls {
    /// <summary>
    ///     展示正在拖拽的视图
    /// </summary>
    public class DragDropAdorner : Adorner {
        private Size renderSize;
        private Brush renderBrush;

        public struct PInPoint {
            public int X;
            public int Y;

            public PInPoint(int x, int y) { X = x; Y = y; }
            public PInPoint(double x, double y) { X = (int)x; Y = (int)y; }
        }

        [DllImport("user32.dll")]
        static extern void GetCursorPos(ref PInPoint p);

        public DragDropAdorner(FrameworkElement elem) : base(elem) {
            renderSize = elem.RenderSize;
            renderBrush = new VisualBrush(elem);
            IsHitTestVisible = false;
            Window.AddAdorner(elem, this);
        }

        public void Remove() {
            Window.RemoveAdorner(this);
        }

        protected override void OnRender(DrawingContext dc) {
            base.OnRender(dc);

            PInPoint p = new PInPoint();
            GetCursorPos(ref p);

            Point pos = PointFromScreen(new Point(p.X, p.Y));
            Rect rect = new Rect(pos.X, pos.Y, renderSize.Width, renderSize.Height);

            dc.PushOpacity(1);
            dc.DrawRectangle(FindResource("Brush.Window") as Brush, null, rect);
            dc.DrawRectangle(renderBrush, null, rect);
            dc.DrawRectangle(null, new Pen(Brushes.DeepSkyBlue, 2), rect);
        }
    }
}
