namespace GFMatch3.GameTools {
    /// <summary>
    /// В осоновном введен для отслеживания наличия блокирующих анимаций.
    /// </summary>
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