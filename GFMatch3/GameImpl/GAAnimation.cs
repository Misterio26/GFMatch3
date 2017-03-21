using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public abstract class GAAnimation : GameAction {

        private readonly bool _blockeable;

        protected GAAnimation() {
        }

        protected GAAnimation(bool blockeable) {
            this._blockeable = blockeable;
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