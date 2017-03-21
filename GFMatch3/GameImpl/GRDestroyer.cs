using System.Windows;
using System.Windows.Media;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class GRDestroyer : GRBoardElement {
        private int _direction;

        private const double GapHalfSize = 2.5;

        public GRDestroyer(int coloredType, int direction) : base(coloredType) {
            _direction = direction;
        }

        public override void OnDraw(DrawingContext dc) {
            double size = BoardGameConfig.BoardCellSize - 20;
            double sizeHalf = size / 2;

            switch (_direction) {
                case GameMath.DirectionUp:
                    dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                        new PathFigure(new Point(0, -sizeHalf), new[] {
                            new LineSegment(new Point(sizeHalf, -GapHalfSize), false),
                            new LineSegment(new Point(-sizeHalf, -GapHalfSize), false)
                        }, true)
                    }));
                    break;
                case GameMath.DirectionDown:
                    dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                        new PathFigure(new Point(0, sizeHalf), new[] {
                            new LineSegment(new Point(sizeHalf, GapHalfSize), false),
                            new LineSegment(new Point(-sizeHalf, GapHalfSize), false)
                        }, true)
                    }));
                    break;
                case GameMath.DirectionLeft:
                    dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                        new PathFigure(new Point(-sizeHalf, 0), new[] {
                            new LineSegment(new Point(-GapHalfSize, sizeHalf), false),
                            new LineSegment(new Point(-GapHalfSize, -sizeHalf), false)
                        }, true)
                    }));
                    break;
                case GameMath.DirectionRight:
                    dc.DrawGeometry(_brush, null, new PathGeometry(new[] {
                        new PathFigure(new Point(sizeHalf, 0), new[] {
                            new LineSegment(new Point(GapHalfSize, sizeHalf), false),
                            new LineSegment(new Point(GapHalfSize, -sizeHalf), false)
                        }, true)
                    }));
                    break;
            }
        }
    }
}