using System;
using System.Reflection;
using System.Windows.Media;

namespace GFMatch3.GameCore {
    public class GameDirector {
        /// <summary>
        /// Разрешение по ширине, по которому можно рисовать и опереировать с графикой.
        /// А система сама подстроит масштаб, чтобы ANCHORED_SCREEN_WIDTH - это
        /// всегда было на всю ширину экрана
        /// </summary>
        public const int AnchoredScreenWidth = 1920;

        // здесь хардкод "GFMatch3.GameImpl.SceneStart", в это проекте не существенно
        public static readonly GameDirector Instance = new GameDirector("GFMatch3.GameImpl.SceneStart");

        private readonly String _startupStageClassRef;

        private GameScene _activeGameScene;
        private GameScene _incomingGameScene;

        private int _currentScreenHeight;

        private bool _created;

        private double _deltaTime;
        public double DeltaTime => _deltaTime;

        private GameDirector(String startupStageClassRef) {
            _startupStageClassRef = startupStageClassRef;
        }

        public void Update(long deltaTime) {
            _deltaTime = deltaTime / 1000.0;

            if (!_created) {
                _created = true;
                OnCreate();
            }

            if (_incomingGameScene != null) {
                GameScene wasGameScene = _activeGameScene;

                _activeGameScene = _incomingGameScene;
                _activeGameScene.Start();
                _incomingGameScene = null;

                if (wasGameScene != null) {
                    wasGameScene.Stop();
                }
            }

            _activeGameScene.Update();
        }

        public void Render(DrawingContext dc, int screenHeight) {
            if (_activeGameScene == null) return;

            _currentScreenHeight = screenHeight;
            _activeGameScene.Render(dc);
        }

        private void OnCreate() {
            GameScene startupGameScene = (GameScene) Assembly.GetExecutingAssembly()
                .CreateInstance(_startupStageClassRef);
            SetStage(startupGameScene);
        }

        public void SetStage(GameScene newScene) {
            // устанавливаемая сцена становится активной не сразу, а только вначале игрового цикла
            _incomingGameScene = newScene;
        }

        public T GetScene<T>() where T : GameScene {
            return _activeGameScene as T;
        }

        public int ScreenWidth => AnchoredScreenWidth;
        public int ScreenHeight => _currentScreenHeight;

    }
}