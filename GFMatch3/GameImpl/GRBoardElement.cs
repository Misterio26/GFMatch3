using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public abstract class GRBoardElement : GameRenderer {
        protected SolidColorBrush Brush;

        protected GRBoardElement(int coloredType) {
            Brush = new SolidColorBrush(BoardGameConfig.ColoredTypes[coloredType]);
        }
    }
}