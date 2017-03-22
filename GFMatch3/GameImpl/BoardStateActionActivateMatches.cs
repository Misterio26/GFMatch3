using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Простой конечный автомат для состояния доски на основе добавлени/удаления GameAction'ов из объекта доски.
    /// Пробуем активировать совпадения. Если совпадения были, то переход в BoardStateActionCondense,
    /// если нет, то в BoardStateActionPlayer.
    /// Переход в состояния: BoardStateActionCondense или BoardStateActionPlayer.
    /// </summary>
    public class BoardStateActionActivateMatches : GameAction {
        public override void OnUpdate() {
            if (AnimationsManager.Instance.HasActiveBlockeableAnimations()) return;

            if (((GOBoard) GameObject).ActivateMatches(false)) {
                GameObject.AddAction(new BoardStateActionCondense());
            } else {
                GameObject.AddAction(new BoardStateActionPlayer());
            }
            RemoveFromParent();
        }
    }
}