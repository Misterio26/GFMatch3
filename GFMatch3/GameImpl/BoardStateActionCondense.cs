using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Простой конечный автомат для состояния доски на основе добавлени/удаления GameAction'ов из объекта доски.
    /// Производим уплотнение поля (заполнение пустот), смещая макисмум на одну клетку за раз,
    /// и так пока все пустоты не уйдут.
    /// Переход в состояния: BoardStateActionActivateMatches
    /// </summary>
    public class BoardStateActionCondense : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            if (!((GOBoard) GameObject).CondenseStep(false)) {
                GameObject.AddAction(new BoardStateActionActivateMatches());
                RemoveFromParent();
            }
        }
    }
}