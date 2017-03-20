using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class SceneBoard : GameScene {

        private readonly GOBoard _board;
        private readonly GOTimer _time;

        public SceneBoard() {
            _board = new GOBoard();
            _time = new GOTimer();
            AddChild(_board);
            AddChild(_time);
        }

    }
}