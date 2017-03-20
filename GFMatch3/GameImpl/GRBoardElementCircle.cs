using System.Windows;
using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GRBoardElementCircle : GameRenderer {

        private readonly Brush _brush = new SolidColorBrush(Colors.Red);

        public override void OnDraw(DrawingContext dc) {
            dc.DrawEllipse(_brush, null, new Point(0, 0), BoardGameConfig.BoardCellSize / 2, BoardGameConfig.BoardCellSize / 2);
        }
    }
}