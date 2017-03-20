using System.Windows;
using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GAColorableOnClick : GameAction {

        public override void OnUpdate() {
            if (GameDirector.Instance.IsMouseDown) {
                Point point = GameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                Rect textRect = ((IRected) GameObject.Renderer).Rect;
                if (textRect.Contains(point)) {
                    ((IColorable) GameObject.Renderer).Color = Colors.DarkGray;
                }
            }
            if (GameDirector.Instance.IsMouseUp) {
                ((IColorable) GameObject.Renderer).Color = Colors.White;
            }
        }
    }
}