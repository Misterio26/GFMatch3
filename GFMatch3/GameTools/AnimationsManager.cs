namespace GFMatch3.GameTools {
    public class AnimationsManager {
        public static readonly AnimationsManager Instance = new AnimationsManager();

        private int _activeBlockeableAnimationsCounter;

        private AnimationsManager() {
        }

        public void BlockeableAnimationStarted() {
            _activeBlockeableAnimationsCounter++;
        }

        public void BlockeableAnimationStoped() {
            _activeBlockeableAnimationsCounter--;
        }

        public bool HasActiveBlockeableAnimations() {
            return _activeBlockeableAnimationsCounter > 0;
        }

    }
}