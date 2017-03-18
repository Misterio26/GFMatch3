using System;
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

        private readonly String StartupStageClassRef;

        private GameStage ActiveGameScene;
        private GameStage IncomingGameScene;

        private GameDirector(String startupStageClassRef) {
            StartupStageClassRef = startupStageClassRef;
        }

        public void Update() {

            // todo OnCreate не всегда вызывать, тут инициализируем таймер и ФПС и первый старт проевреяем
            OnCreate();

            if (IncomingGameScene != null) {
                if (ActiveGameScene != null) {
                    ActiveGameScene.OnStop();
                }
                ActiveGameScene = IncomingGameScene;
                ActiveGameScene.OnStart();
                IncomingGameScene = null;
            }

            ActiveGameScene.Update();
        }

        public void Render(DrawingContext dc, int screenHeight) {
            if (ActiveGameScene == null) return;

            ActiveGameScene.Render(dc, screenHeight);
        }

        private void OnCreate() {
            GameStage startupGameStage = (GameStage) System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(StartupStageClassRef);
            SetStage(startupGameStage);
        }

        public void SetStage(GameStage newStage) {
            // устанавливаемая сцена становится активной не сразу, а только вначале игрового цикла
            IncomingGameScene = newStage;
        }

    }
}