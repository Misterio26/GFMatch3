using System.Windows.Media;

namespace GFMatch3.GameCore {
    /// <summary>
    /// См. <see cref="GameDirector"/>.
    /// Такой корневой контроллер, который можно установить текущим в GameDirector и тогда
    /// начнут работать все GameObject, которые добавлены в эту сцену.
    /// </summary>
    public class GameScene {

        private GameObject _rootGameObject = new GameObject();

        public virtual void OnStart() {
        }

        public virtual void OnStop() {
        }

        public virtual void Start() {
            _rootGameObject.StartAsRoot();
            OnStart();
        }

        public virtual void Stop() {
            OnStop();
            _rootGameObject.StopAsRoot();
        }

        public void Update() {
            _rootGameObject.Update();
        }

        public void Render(DrawingContext dc) {
            _rootGameObject.Render(dc);
        }

        public void AddChild(GameObject gameObject) {
            _rootGameObject.AddChild(gameObject);
        }

    }
}