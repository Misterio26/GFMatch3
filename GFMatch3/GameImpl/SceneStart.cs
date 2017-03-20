using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class SceneStart : GameScene {

        private readonly GOBoard _board;

        public SceneStart() {
            _board = new GOBoard();
            AddChild(_board);
        }
    }
}