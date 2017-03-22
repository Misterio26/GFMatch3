using System;

namespace GFMatch3.GameCore {
    /// <summary>
    /// См. <see cref="GameDirector"/>, <see cref="GameObject"/>.
    /// Вся логика непосредсвтенно ведется здесь в методе OnUpdate.
    /// GameAction должен быть обязательно приявзан к GameObject, чтобы работать.
    /// </summary>
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
            TryActivate(null);
            OnUpdate();
        }

        public void SetParent(GameObject parent) {
            if (_parent != null) {
                throw new Exception("GameAction allready added to GameObject");
            }
            _parent = new WeakReference(parent);
            if (parent.InScene) {
                TryActivate(parent);
            }
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

        public void TryActivate(GameObject parent) {
            if (!_activated) {
                _activated = true;

                GameActionSavedState savedState = new GameActionSavedState(null);
                if (parent != null) {
                    savedState = Prepare(parent);
                }
                OnStart();
                if (parent != null) {
                    Free(savedState);
                }
            }
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