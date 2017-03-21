using GFMatch3.GameCore;
using GFMatch3.GameImpl;

namespace GFMatch3.GameTools {
    public class AnimationTransformToDestory : GAAnimation {
        private readonly GameTransform _toTransform;
        private GameTransform _fromTransform;
        private readonly double _transitionTime;

        private GameTransform _currentTransform;
        private double _workTime;

        private bool _destroy;

        public AnimationTransformToDestory(GameTransform toTransform, double transitionTime):this(toTransform, transitionTime, false) {
        }

        public AnimationTransformToDestory(GameTransform toTransform, double transitionTime, bool blockeable):base(blockeable) {
            _toTransform = toTransform;
            _transitionTime = transitionTime;
        }

        protected override void OnAnimationStart() {
            _fromTransform = GameObject.Transform;
        }

        protected override void OnAnimate() {
            if (_destroy) {
                GameObject.Transform = _currentTransform;
                GameObject.RemoveFromParent();
                return;
            }
            _workTime += GameDirector.Instance.DeltaTime;
            if (_workTime > _transitionTime) {
                _workTime = _transitionTime;
                _destroy = true;
            }

            _currentTransform = (_toTransform - _fromTransform) * (_workTime / _transitionTime) + _fromTransform;
            GameObject.Transform = _currentTransform;
        }
    }
}