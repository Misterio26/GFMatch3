using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GFMatch3.GameCore {
    /// <summary>
    /// <para>Логика взаимодействия для BasicGameCanvas.xaml.</para>
    /// <para>Один из элементов для создания основного игрового цикла.</para>
    /// <para>Отвечает за отрисовку.</para>
    /// <para>Создает элементарный игровой цикл в UI потоке,
    /// в реальной жизни так, конечно, лучше не делать</para>
    /// </summary>
    public partial class BasicGameCanvas : Canvas {

        private long _lastGameLoopCallTime;
        private long _lastUpdateTime;

        private bool _isGameLoopActive;
        private DispatcherOperation _gameLoopActiveDispatcher;

        private bool _gameUpdatedAllowed;

        public BasicGameCanvas() {
            InitializeComponent();
        }

        private void OnIsVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            if (IsVisible) {
                StartGameLoop();
            } else {
                StopGameLoop();
            }
        }


        protected override void OnRender(DrawingContext dc) {
            if (_isGameLoopActive) {
                if (_gameUpdatedAllowed) {
                    GameUpdate();
                }
            }

            base.OnRender(dc);

            if (_isGameLoopActive) {
                GameRender(dc);
            }

            _gameUpdatedAllowed = false;
        }

        private void TryGameLoopOnTimer() {
            _gameLoopActiveDispatcher = null;

            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (milliseconds - _lastGameLoopCallTime < 10 && !_gameUpdatedAllowed) {
                InvokeGameLoopTry();
                return;
            }

            _lastGameLoopCallTime = milliseconds;
            _gameUpdatedAllowed = true;

            InvalidateVisual();
            InvokeGameLoopTry();
        }

        private void InvokeGameLoopTry() {
            _gameLoopActiveDispatcher = Dispatcher.InvokeAsync(TryGameLoopOnTimer, DispatcherPriority.ContextIdle);
        }

        private void StartGameLoop() {
            if (_isGameLoopActive) return;
            _isGameLoopActive = true;
            _lastGameLoopCallTime = 0;
            _lastUpdateTime = 0;
            InvokeGameLoopTry();
        }

        private void StopGameLoop() {
            if (!_isGameLoopActive) return;
            _isGameLoopActive = false;
            if (_gameLoopActiveDispatcher != null) {
                _gameLoopActiveDispatcher.Abort();
            }
        }

        private void GameUpdate() {
            long deltaTime = 0;
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (_lastUpdateTime > 0) {
                deltaTime = milliseconds - _lastUpdateTime;
            }
            _lastUpdateTime = milliseconds;
            GameDirector.Instance.Update(deltaTime);
        }

        private void GameRender(DrawingContext dc) {
            // вычисляем масштаб и актуальную высоту, чтобы подстроиться под GameDirector.ANCHORED_SCREEN_WIDTH
            double scaleX = ActualWidth / GameDirector.AnchoredScreenWidth;
            int useHeight = (int) Math.Ceiling(ActualHeight / scaleX);
            double scaleY = ActualHeight / useHeight;

            // применяем мастаб и вызываем отрисовку
            dc.PushTransform(new ScaleTransform(scaleX, scaleY));
            GameDirector.Instance.Render(dc, useHeight);
            dc.Pop();
        }
    }
}