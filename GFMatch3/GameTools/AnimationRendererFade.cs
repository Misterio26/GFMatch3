using System.Windows.Media;
using GFMatch3.GameCore;

namespace GFMatch3.GameTools {
    public class AnimationRendererFade : GAAnimation {
        private readonly double _transitionTime;

        private double _currentAlpha;
        private double _workTime;

        public AnimationRendererFade(double transitionTime) : this(transitionTime, false) {
        }

        public AnimationRendererFade(double transitionTime, bool blockeable) : base(blockeable) {
            _transitionTime = transitionTime;
        }

        protected override void OnAnimationStart() {
        }

        protected override void OnAnimate() {
            bool remove = false;
            _workTime += GameDirector.Instance.DeltaTime;
            if (_workTime > _transitionTime) {
                _workTime = _transitionTime;
                remove = true;
            }

            _currentAlpha = (_transitionTime - _workTime) / _transitionTime;
            if (GameObject.Renderer != null && (GameObject.Renderer is IColorable)) {
                ((IColorable) GameObject.Renderer).Color = Color.FromArgb((byte) (255 * _currentAlpha), 255, 255, 255);
            }
            if (remove) {
                RemoveFromParent();
            }
        }
    }
}