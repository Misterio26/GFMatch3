using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class BoardStateActionCondense : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            if (!((GOBoard) GameObject).CondenseStep(false)) {
                GameObject.AddAction(new BorderStateActionActivateMatches());
                RemoveFromParent();
            }
        }
    }
}