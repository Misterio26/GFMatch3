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
//        private float positonDelta = 0;
//        private int fpsCounter = 0;
//        private int fpsNow = 0;
//        private long prevMilliseconds = 0;

        private long LastGameLoopCallTime;

        private bool IsGameLoopActive;
        private DispatcherOperation GameLoopActiveDispatcher;

        private bool GameUpdatedAllowed;

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
            if (IsGameLoopActive) {
                if (GameUpdatedAllowed) {
                    GameUpdate();
                }
            }

            base.OnRender(dc);

            if (IsGameLoopActive) {
                GameRender(dc);
            }

            GameUpdatedAllowed = false;
//            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            LastGameLoopCallTime = milliseconds;
//
//            if (milliseconds - prevMilliseconds >= 1000) {
//                fpsNow = fpsCounter;
//                fpsCounter = 0;
//                prevMilliseconds = milliseconds;
//
//                Debug.WriteLine("FPS = " + fpsNow);
//            }
//            fpsCounter++;
//
//            positonDelta += 1;
//
//            dc.PushTransform(new ScaleTransform(4, 4));
//            Pen pen = new Pen(new SolidColorBrush(Color.FromScRgb(1, 1, 0, 0)), 3);
//            dc.DrawEllipse(null, pen, new Point(10 + positonDelta, 10), 10, 10);
//            dc.Pop();
//
//            InvokeGameLoopTry();
        }

        private void TryGameLoopOnTimer() {
            GameLoopActiveDispatcher = null;

            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (milliseconds - LastGameLoopCallTime < 10 && !GameUpdatedAllowed) {
                InvokeGameLoopTry();
                return;
            }

            LastGameLoopCallTime = milliseconds;
            GameUpdatedAllowed = true;

            InvalidateVisual();
        }

        private void InvokeGameLoopTry() {
            GameLoopActiveDispatcher = Dispatcher.InvokeAsync(TryGameLoopOnTimer, DispatcherPriority.ContextIdle);
        }

        private void StartGameLoop() {
            if (IsGameLoopActive) return;
            IsGameLoopActive = true;
            LastGameLoopCallTime = 0;
            InvokeGameLoopTry();
        }

        private void StopGameLoop() {
            if (!IsGameLoopActive) return;
            IsGameLoopActive = false;
            if (GameLoopActiveDispatcher != null) {
                GameLoopActiveDispatcher.Abort();
            }
        }

        private void GameUpdate() {
            GameDirector.Instance.Update();
        }

        private void GameRender(DrawingContext dc) {
            // вычисляем масштаб и актуальную высоту, чтобы подстроиться под GameDirector.ANCHORED_SCREEN_WIDTH
            double scaleX = ActualWidth / GameDirector.AnchoredScreenWidth;
            int useHeight = (int) Math.Ceiling(ActualHeight / scaleX);
            double scaleY = ActualHeight / useHeight;

            // применяем мастаб и вызываем отрисовку
            dc.PushTransform(new ScaleTransform(scaleX, scaleY));
            GameDirector.Instance.Render(dc, useHeight);
//            dc.DrawRectangle(new SolidColorBrush(Color.FromScRgb(1, 1, 0, 0)), null,
//                new Rect(0, 0, 1919, useHeight - 1));
            dc.Pop();
        }
    }
}