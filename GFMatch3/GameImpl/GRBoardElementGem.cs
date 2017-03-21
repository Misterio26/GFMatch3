using System.Windows;
using System.Windows.Media;

namespace GFMatch3.GameImpl {
    public class GRBoardElementGem : GRBoardElement {

        public GRBoardElementGem(int coloredType) : base(coloredType) {
        }

        public override void OnDraw(DrawingContext dc) {
//            ImageBrush imageBrush = new ImageBrush(ResourcesManager.Instance.BackgroundBitmap);
//            dc.DrawEllipse(_brush, null, new Point(0, 0), BoardGameConfig.BoardCellSize / 2 - 10,
//                BoardGameConfig.BoardCellSize / 2 - 10);
            double size = BoardGameConfig.BoardCellSize - 20;
            double sizeHalf = size / 2;
            dc.DrawRoundedRectangle(_brush, null, new Rect(-sizeHalf, -sizeHalf, size, size), 14, 14);
        }
    }
}