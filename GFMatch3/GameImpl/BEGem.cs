using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class BEGem : GOBoardElement {

        public BEGem(int coloredType) : base(coloredType, new GRBoardElementGem(coloredType)) {

        }

        public override void OnAtivate() {
            AnimatablePart.AddAction(new AnimationTransformToDestory(GameTransform.Zero, BoardGameConfig.AnimationsCommonSpeed, true));
        }
    }
}