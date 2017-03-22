using System;
using GFMatch3.GameCore;

namespace GFMatch3.GameTools {
    public class GATimer : GameAction {
        private double _leftTime;

        private readonly Action<GameObject> _onFinish;

        public GATimer(double leftTime, Action<GameObject> onFinish) {
            _leftTime = leftTime;
            _onFinish = onFinish;
        }

        public override void OnUpdate() {
            base.OnUpdate();
            _leftTime -= GameDirector.Instance.DeltaTime;
            if (_leftTime <= 0) {
                _onFinish(GameObject);
                RemoveFromParent();
            }
        }
    }
}