using System.Windows;
using System.Windows.Media;

namespace GFMatch3.GameImpl {
    public class GRBoardElementLine : GRBoardElement {
        private bool _vertical;

        private const double GapHalfSize = 2.5;

        public GRBoardElementLine(int coloredType, bool vertical) : base(coloredType) {
            _vertical = vertical;
        }

        public override void OnDraw(DrawingContext dc) {
            double size = BoardGameConfig.BoardCellSize - 20;
            double sizeHalf = size / 2;

            if (_vertical) {
                dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                    new PathFigure(new Point(0, -sizeHalf), new[] {
                        new LineSegment(new Point(sizeHalf, -GapHalfSize), false),
                        new LineSegment(new Point(-sizeHalf, -GapHalfSize), false)
                    }, true)
                }));

                dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                    new PathFigure(new Point(0, sizeHalf), new[] {
                        new LineSegment(new Point(sizeHalf, GapHalfSize), false),
                        new LineSegment(new Point(-sizeHalf, GapHalfSize), false)
                    }, true)
                }));
            } else {
                dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                    new PathFigure(new Point(-sizeHalf, 0), new[] {
                        new LineSegment(new Point(-GapHalfSize, sizeHalf), false),
                        new LineSegment(new Point(-GapHalfSize, -sizeHalf), false)
                    }, true)
                }));

                dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                    new PathFigure(new Point(sizeHalf, 0), new[] {
                        new LineSegment(new Point(GapHalfSize, sizeHalf), false),
                        new LineSegment(new Point(GapHalfSize, -sizeHalf), false)
                    }, true)
                }));
            }
        }
    }
}