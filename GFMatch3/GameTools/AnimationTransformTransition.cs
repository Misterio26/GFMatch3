using GFMatch3.GameCore;

namespace GFMatch3.GameTools {
    public class AnimationTransformTransition : GAAnimation {
        private readonly GameTransform _fromTransform;
        private readonly GameTransform _toTransform;
        private readonly double _transitionTime;

        private GameTransform _currentTransform;

        private double _workTime;
        private bool _reverse;

        public AnimationTransformTransition(GameTransform fromTransform, GameTransform toTransform,
            double transitionTime) : this(fromTransform, toTransform, transitionTime, false) {
        }

        public AnimationTransformTransition(GameTransform fromTransform, GameTransform toTransform,
            double transitionTime, bool blockeable) : base(blockeable) {
            _fromTransform = fromTransform;
            _toTransform = toTransform;
            _transitionTime = transitionTime;
        }

        public void SetToTo(bool setToTo) {
            _reverse = !setToTo;
        }

        protected override void OnAnimate() {
            bool remove = false;
            if (_reverse) {
                _workTime -= GameDirector.Instance.DeltaTime;
                if (_workTime < 0) {
                    _workTime = 0;
                    remove = true;
                }
            } else {
                _workTime += GameDirector.Instance.DeltaTime;
                if (_workTime > _transitionTime) {
                    _workTime = _transitionTime;
                    remove = true;
                }
            }
            _currentTransform = (_toTransform - _fromTransform) * (_workTime / _transitionTime) + _fromTransform;
            GameObject.Transform = _currentTransform;

            if (remove) {
                RemoveFromParent();
            }
        }
    }
}