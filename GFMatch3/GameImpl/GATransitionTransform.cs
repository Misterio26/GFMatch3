using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GATransitionTransform : GameAction {
        private readonly GameTransform _fromTransform;
        private readonly GameTransform _toTransform;
        private readonly double _transitionTime;

        private GameTransform _currentTransform;

        private double _workTime;
        private bool _reverse = true;
        private bool _active;

        public GATransitionTransform(GameTransform fromTransform, GameTransform toTransform, double transitionTime) {
            _fromTransform = fromTransform;
            _toTransform = toTransform;
            _transitionTime = transitionTime;
            _currentTransform = fromTransform;
        }

        public void SetState(bool setToTo) {
            if (setToTo == !_reverse) return;
            _reverse = !_reverse;
            _active = true;
        }

        public override void OnUpdate() {
            if (!_active) {
                GameObject.Transform = _currentTransform;
                return;
            }
            if (_reverse) {
                _workTime -= GameDirector.Instance.DeltaTime;
                if (_workTime < 0) {
                    _workTime = 0;
                    _active = false;
                }
            } else {
                _workTime += GameDirector.Instance.DeltaTime;
                if (_workTime > _transitionTime) {
                    _workTime = _transitionTime;
                    _active = false;
                }
            }
//            var bounceEase = new CircleEase();
//            bounceEase.EasingMode = EasingMode.EaseInOut;
//            if (!_reverse) {
//                _currentTransform = (_toTransform - _fromTransform) * bounceEase.Ease(_workTime / _transitionTime) + _fromTransform;
//            } else {
//                _currentTransform = (_fromTransform - _toTransform) * bounceEase.Ease((_transitionTime - _workTime) / _transitionTime) + _toTransform;
//            }
            _currentTransform = (_toTransform - _fromTransform) * (_workTime / _transitionTime) + _fromTransform;
            GameObject.Transform = _currentTransform;
        }
    }
}