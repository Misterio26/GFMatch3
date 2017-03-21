using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class BoardStateActionPlayer : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            ((GOBoard) GameObject).TrySwapBack();

            if (GameDirector.Instance.IsMouseClick) {
                int boardSizeCfg = BoardGameConfig.BoardCellSize * BoardGameConfig.BoardCellSize;
                Point clickPosition = GameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                if (clickPosition.X >= 0 && clickPosition.X < boardSizeCfg
                    && clickPosition.Y >= 0 && clickPosition.Y < boardSizeCfg) {
                    int cellX = (int) (clickPosition.X / BoardGameConfig.BoardCellSize);
                    int cellY = (int) (clickPosition.Y / BoardGameConfig.BoardCellSize);

                    if (((GOBoard) GameObject).SelectCell(new CellCoord(cellX, cellY))) {
                        GameObject.AddAction(new BorderStateActionActivateMatches());
                        RemoveFromParent();
                    }
                }
            }
        }
    }
}