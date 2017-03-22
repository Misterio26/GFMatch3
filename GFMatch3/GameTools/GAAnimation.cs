using GFMatch3.GameCore;

namespace GFMatch3.GameTools {
    public abstract class GAAnimation : GameAction {

        private readonly bool _blockeable;

        protected GAAnimation() {
        }

        protected GAAnimation(bool blockeable) {
            _blockeable = blockeable;
        }

        public sealed override void OnUpdate() {
            base.OnUpdate();
            OnAnimate();
        }

        public sealed override void OnStart() {
            base.OnStart();
            if (_blockeable) {
                AnimationsManager.Instance.BlockeableAnimationStarted();
            }
            OnAnimationStart();
        }

        public sealed override void OnStop() {
            base.OnStop();
            if (_blockeable) {
                AnimationsManager.Instance.BlockeableAnimationStoped();
            }
            OnAnimationStop();
        }

        protected virtual void OnAnimationStart() {

        }

        protected virtual void OnAnimate() {

        }

        protected virtual void OnAnimationStop() {

        }

    }
}