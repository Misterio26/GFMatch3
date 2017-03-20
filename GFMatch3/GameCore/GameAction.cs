using System;

namespace GFMatch3.GameCore {
    public class GameAction {
        protected GameObject GameObject => _gameObject;

        private GameObject _gameObject;

        private WeakReference _parent;
        private bool _activated;

        public GameActionSavedState Prepare(GameObject gameObject) {
            GameActionSavedState actionSavedState = new GameActionSavedState(_gameObject);
            _gameObject = gameObject;
            return actionSavedState;
        }

        public void Free(GameActionSavedState savedState) {
            _gameObject = savedState.GameObject;
        }

        public void Update() {
            if (!_activated) {
                _activated = true;
                OnStart();
            }
            OnUpdate();
        }

        public void SetParent(GameObject parent) {
            if (_parent != null) {
                throw new Exception("GameAction allready added to GameObject");
            }
            _parent = new WeakReference(parent);
        }

        public void TryRemoveParent(GameObject fromParent) {
            GameObject parent;
            if (_parent == null || (parent = (_parent.Target as GameObject)) == null) {
                return;
            }
            if (parent != fromParent) {
                throw new Exception("Trying to remove GameAction from a wrong parent");
            }

            _parent = null;

            TryDeactivate(fromParent);
        }

        public void TryDeactivate(GameObject fromParent) {
            if (_activated) {
                _activated = false;

                GameActionSavedState savedState = Prepare(fromParent);
                OnStop();
                Free(savedState);
            }
        }

        public void RemoveFromParent() {
            GameObject parent;
            if (_parent == null || (parent = (_parent.Target as GameObject)) == null) {
                return;
            }
            parent.RemoveAction(this);
        }

        public virtual void OnStart() {
        }

        public virtual void OnUpdate() {
        }

        public virtual void OnStop() {
        }
    }

    public class GameActionDelegated : GameAction {
        private readonly Action<GameObject> _onUpdateDel;
        private readonly Action<GameObject> _onStartDel;
        private readonly Action<GameObject> _onStopDel;

        public GameActionDelegated(Action<GameObject> onUpdate, Action<GameObject> onStart, Action<GameObject> onStop) {
            _onUpdateDel = onUpdate;
            _onStartDel = onStart;
            _onStopDel = onStop;
        }

        public override void OnStart() {
            _onStartDel?.Invoke(GameObject);
        }

        public override void OnUpdate() {
            _onUpdateDel?.Invoke(GameObject);
        }

        public override void OnStop() {
            _onStopDel?.Invoke(GameObject);
        }
    }
}