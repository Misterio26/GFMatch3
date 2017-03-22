using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBackground : GameObject {
        public GOBackground() {
            Transform.Z = -100;
            GRBackground background = new GRBackground();
            Renderer = background;
            AddAction(new GameActionDelegated(gameObject => {
                ((GRBackground) gameObject.Renderer).Size.Width = GameDirector.Instance.ScreenWidth;
                ((GRBackground) gameObject.Renderer).Size.Height = GameDirector.Instance.ScreenHeight;
            }, null, null));
        }
    }
}