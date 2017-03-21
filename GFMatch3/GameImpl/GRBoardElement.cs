using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public abstract class GRBoardElement : GameRenderer {
        protected SolidColorBrush _brush;

        protected GRBoardElement(int coloredType) {
            _brush = new SolidColorBrush(BoardGameConfig.ColoredTypes[coloredType]);
        }
    }
}