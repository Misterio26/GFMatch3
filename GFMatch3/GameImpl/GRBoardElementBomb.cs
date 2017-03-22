using System.Windows;
using System.Windows.Media;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class GRBoardElementBomb : GRBoardElement, IColorable {
        public Color Color { get; set; }

        public GRBoardElementBomb(int coloredType) : base(coloredType) {
            Color = Color.FromArgb(255, 255, 255, 255);
        }

        public override void OnDraw(DrawingContext dc) {
            double size = BoardGameConfig.BoardCellSize - 15;
            double sizeHalf = size / 2;
            var brushColor = Brush.Color;
            brushColor.A = Color.A;
            Brush.Color = brushColor;
            dc.DrawEllipse(Brush, null, new Point(), sizeHalf, sizeHalf);
        }
    }
}