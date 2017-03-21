using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class SceneBoard : GameScene {

        private readonly GOBoard _board;
        private readonly GOTimer _time;
        private readonly GOScore _score;

        public SceneBoard() {
            _board = new GOBoard();
            _time = new GOTimer();
            _score = new GOScore();
            AddChild(_board);
            AddChild(_time);
            AddChild(_score);
            AddChild(new GOBackground());
        }

        public void AddScore() {
            _score.Score++;
        }

    }
}