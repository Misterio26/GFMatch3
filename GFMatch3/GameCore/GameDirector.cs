using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace GFMatch3.GameCore {
    /// <summary>
    /// Ядро простого движка.
    /// GameDirector - содержет и взаимодействует с одной активной GameScene.
    /// GameScene - содержет и взаимодействует с иерархией GameObject.
    /// GameObject - содержет и взаимодействует сподмножеством GameObject,
    /// а также набор GameAction и один GameRenderer.
    /// Именно нужные методы GameDirector'а вызываются из системного интерфейса,
    /// чтобы активировать и оперировать игрой (<see cref="BasicGameCanvas"/>).
    /// </summary>
    public class GameDirector {
        /// <summary>
        /// Разрешение по ширине, по которому можно рисовать и опереировать с графикой.
        /// А система сама подстроит масштаб, чтобы ANCHORED_SCREEN_WIDTH - это
        /// всегда было на всю ширину экрана
        /// </summary>
        public const int AnchoredScreenWidth = 1920;

        // здесь хардкод "GFMatch3.GameImpl.SceneStart", в этом проекте несущественно
        public static readonly GameDirector Instance = new GameDirector("GFMatch3.GameImpl.SceneStart");

        private readonly String _startupStageClassRef;

        private long _fpsLastTime;
        private long _fpsCounter;
        private long _fps;
        private long _fpsMaxDeltaTime;
        private long _fpsMinDeltaTime;

        private GameScene _activeGameScene;
        private GameScene _incomingGameScene;

        private int _currentScreenHeight;

        private bool _created;

        private double _deltaTime;
        public double DeltaTime => _deltaTime;

        private Point _mousePosition;
        public Point MousePosition => _mousePosition;

        private bool _isMouseClick;
        public bool IsMouseClick => _isMouseClick;

        private bool _isMouseDown;
        public bool IsMouseDown => _isMouseDown;

        private bool _isMouseUp;
        public bool IsMouseUp => _isMouseUp;


        private GameDirector(String startupStageClassRef) {
            _startupStageClassRef = startupStageClassRef;
        }

        public void Update(long deltaTime, int screenHeight) {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (milliseconds - _fpsLastTime >= 1000) {
                _fps = _fpsCounter;

                Debug.WriteLine("_fps = " + _fps);
                Debug.WriteLine("_fpsMaxDeltaTime = " + _fpsMaxDeltaTime);
                Debug.WriteLine("_fpsMinDeltaTime " + _fpsMinDeltaTime);

                _fpsLastTime = milliseconds;
                _fpsCounter = 0;
                _fpsMaxDeltaTime = long.MinValue;
                _fpsMinDeltaTime = long.MaxValue;
            }
            _fpsCounter++;
            _fpsMaxDeltaTime = Math.Max(_fpsMaxDeltaTime, deltaTime);
            _fpsMinDeltaTime = Math.Min(_fpsMinDeltaTime, deltaTime);

            _deltaTime = deltaTime / 1000.0;
            _currentScreenHeight = screenHeight;

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

            OnResetFlagsAfterUpdate();
        }

        public void Render(DrawingContext dc, int screenHeight) {
            if (_activeGameScene == null) return;

            _currentScreenHeight = screenHeight;
            _activeGameScene.Render(dc);
        }

        private void OnResetFlagsAfterUpdate() {
            _isMouseClick = false;
            _isMouseUp = false;
            _isMouseDown = false;
        }

        private void OnCreate() {
            GameScene startupGameScene = (GameScene) Assembly.GetExecutingAssembly()
                .CreateInstance(_startupStageClassRef);
            SetScene(startupGameScene);
        }

        public void MouseClick(Point mouseClickPosition) {
            _isMouseClick = true;
            _mousePosition = mouseClickPosition;
        }

        public void MouseDown(Point mouseDownPosition) {
            _isMouseDown = true;
            _mousePosition = mouseDownPosition;
        }

        public void MouseUp(Point mouseUpPosition) {
            _isMouseUp = true;
            _mousePosition = mouseUpPosition;
        }

        public void SetScene(GameScene newScene) {
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