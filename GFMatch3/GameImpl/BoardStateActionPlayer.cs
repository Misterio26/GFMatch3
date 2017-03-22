using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Простой конечный автомат для состояния доски на основе добавлени/удаления GameAction'ов из объекта доски.
    /// Активно ожидание действия игрока.
    /// Переход в состояния: BoardStateActionActivateMatches
    /// </summary>
    public class BoardStateActionPlayer : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            ((GOBoard) GameObject).TrySwapBack();

            if (GameObject.GetScene<SceneBoard>()?.GetTimeLeft() <= 0) {
                GameObject.AddAction(new GATimer(1, gameObject => {
                    GameDirector.Instance.SetScene(new SceneFinish());
                }));
                RemoveFromParent();
                return;
            }

            if (GameDirector.Instance.IsMouseClick) {
                int boardSizeCfg = BoardGameConfig.BoardCellSize * BoardGameConfig.BoardCellSize;
                Point clickPosition = GameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                if (clickPosition.X >= 0 && clickPosition.X < boardSizeCfg
                    && clickPosition.Y >= 0 && clickPosition.Y < boardSizeCfg) {
                    int cellX = (int) (clickPosition.X / BoardGameConfig.BoardCellSize);
                    int cellY = (int) (clickPosition.Y / BoardGameConfig.BoardCellSize);

                    if (((GOBoard) GameObject).SelectCell(new CellCoord(cellX, cellY))) {
                        GameObject.AddAction(new BoardStateActionActivateMatches());
                        RemoveFromParent();
                    }
                }
            }
        }
    }
}