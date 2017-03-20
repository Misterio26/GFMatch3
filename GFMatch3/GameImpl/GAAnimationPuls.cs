using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GAAnimationPuls : GameAction {
        private const double TO_SCALE = 1.15;

        private double spendTime;

        public override void OnUpdate() {
            spendTime += GameDirector.Instance.DeltaTime;

            double delta = spendTime % 2.0;
            double scale = delta < 1.0
                ? ((TO_SCALE - 1.0) * delta + 1.0)
                : ((1.0 - TO_SCALE) * (delta - 1.0) + TO_SCALE);

            GameObject.Transform.ScaleX *= scale;
            GameObject.Transform.ScaleY *= scale;
        }
    }
}