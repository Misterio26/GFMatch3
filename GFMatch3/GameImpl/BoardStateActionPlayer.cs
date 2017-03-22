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
                GameObject.AddAction(new GATimer(1,
                    gameObject => { GameDirector.Instance.SetScene(new SceneFinish()); }));
                RemoveFromParent();
                return;
            }

            if (GameDirector.Instance.IsMouseClick) {
                Point clickPosition = GameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                if (((GOBoard) GameObject).IsPointInRange(clickPosition)) {
                    CellCoord cellCoord = ((GOBoard) GameObject).PointToCell(clickPosition);

                    if (((GOBoard) GameObject).SelectCell(cellCoord)) {
                        GameObject.AddAction(new BoardStateActionActivateMatches());
                        RemoveFromParent();
                    }
                }
            }
        }
    }
}