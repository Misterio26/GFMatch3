﻿using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GFMatch3.GameCore {
    public class GameObject : IComparable<GameObject> {
        public readonly GameTransform Transform = new GameTransform();

        public GameRenderer Renderer;

        private List<GameAction> _actions = new List<GameAction>();
        private List<GameObject> _children = new List<GameObject>();

        private List<GameAction> _tmpActionsList = new List<GameAction>();
        private List<GameObject> _tmpChildrenList = new List<GameObject>();

        private WeakReference _parent;

        private bool _inScene;

        public bool InScene {
            get { return _inScene; }
            private set {
                if (_inScene != value) {
                    _inScene = value;
                    InSceneChanged();
                }
            }
        }

        public void Update() {
            if (_actions.Count > 0) {
                _tmpActionsList.AddRange(_actions);
                foreach (GameAction action in _tmpActionsList) {
                    action.Prepare(this);
                    action.Update();
                    action.Free();
                }
                _tmpActionsList.Clear();
            }

            _tmpChildrenList.AddRange(_children);
            foreach (GameObject child in _tmpChildrenList) {
                child.Update();
            }
            _tmpChildrenList.Clear();
        }

        public void Render(DrawingContext dc) {
            if (_children.Count == 0 && Renderer == null) return;
            _children.Sort();

            Matrix matrix = new Matrix();
            matrix.Rotate(Transform.Angle);
            matrix.Scale(Transform.ScaleX, Transform.ScaleY);
            matrix.Translate(Transform.X, Transform.Y);
            dc.PushTransform(new MatrixTransform(matrix));

            if (Renderer != null) {
                Renderer.OnDraw(dc);
            }

            foreach (GameObject child in _children) {
                child.Render(dc);
            }
            dc.Pop();
        }

        public int CompareTo(GameObject other) {
            return Transform.Z.CompareTo(other.Transform.Z);
        }

        public void AddChild(GameObject gameObject) {
            if (_children.Contains(gameObject)) return;
            if (gameObject._parent != null) {
                throw new Exception("GameObject allready added to Scene");
            }
            gameObject._parent = new WeakReference(this);
            gameObject.InScene = InScene;
            _children.Add(gameObject);
        }

        public void RemoveChild(GameObject gameObject) {
            GameObject parent;
            if (gameObject._parent == null || (parent = (gameObject._parent.Target as GameObject)) == null) {
                return;
            }
            if (parent != this) {
                throw new Exception("Trying to remove GameObject from a wrong parent");
            }

            gameObject._parent = null;
            gameObject.InScene = false;
            _children.Remove(gameObject);
        }

        private void InSceneChanged() {
            // уведомим действия, что деактиввирован объект,
            // т.е. удалился со сцены
            // нужно, чтобы гарантировать вызов OnStop у действий
            if (!InScene) {
                foreach (GameAction action in _actions.ToArray()) {
                    action.TryDeactivate(this);
                }
            }
            // всем детям присвоим рекурсивно то же значение "в сцене"
            foreach (GameObject child in _children) {
                child.InScene = InScene;
            }
        }

        public void StartAsRoot() {
            InScene = true;
        }

        public void StopAsRoot() {
            InScene = false;
        }

        public void RemoveFromParent() {
            GameObject parent;
            if (_parent == null || (parent = (_parent.Target as GameObject)) == null) {
                return;
            }
            parent.RemoveChild(this);
        }

        public void AddAction(GameAction gameAction) {
            if (_actions.Contains(gameAction)) return;
            gameAction.SetParent(this);
            _actions.Add(gameAction);
        }

        public void RemoveAction(GameAction gameAction) {
            if (!_actions.Remove(gameAction)) return;
            gameAction.TryRemoveParent(this);
        }
    }
}