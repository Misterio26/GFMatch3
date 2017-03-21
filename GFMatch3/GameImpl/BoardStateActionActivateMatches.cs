using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class BorderStateActionActivateMatches : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            if (((GOBoard) GameObject).ActivateMatches(false)) {
                GameObject.AddAction(new BoardStateActionCondense());
            } else {
                GameObject.AddAction(new BoardStateActionPlayer());
            }
            RemoveFromParent();
        }
    }
}