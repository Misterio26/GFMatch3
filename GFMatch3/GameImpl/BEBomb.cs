using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Элемент на поле - БОНУС БОМБА.
    /// </summary>
    public class BEBomb : GOBoardElement {
        public BEBomb(int coloredType) : base(coloredType, new GRBoardElementBomb(coloredType)) {
        }

        public override void OnActivate(CellCoord cellCoord, bool fast) {
            var animationTransformToDestroy = new AnimationTransformToDestroy(
                new GameTransform(0, 0, 0, 0, 2, 2),
                BoardGameConfig.AnimationBombExplosionTime, true
            );
            animationTransformToDestroy.OnDestroy = RemoveFromParent;

            AnimatablePart.AddAction(animationTransformToDestroy);
            AnimatablePart.AddAction(new AnimationRendererFade(BoardGameConfig.AnimationBombExplosionTime, true));

            AnimatablePart.AddAction(new GATimer(BoardGameConfig.BombActivationTime, gameObject => {
                SceneBoard sceneBoard = gameObject.GetScene<SceneBoard>();
                if (sceneBoard == null) return;
                GOBoard board = sceneBoard.GetBoard();

                board.TryActivateElementInCell(new CellCoord(cellCoord.X - 1, cellCoord.Y - 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X, cellCoord.Y - 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X + 1, cellCoord.Y - 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X + 1, cellCoord.Y), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X + 1, cellCoord.Y + 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X, cellCoord.Y + 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X - 1, cellCoord.Y + 1), false, false);
                board.TryActivateElementInCell(new CellCoord(cellCoord.X - 1, cellCoord.Y), false, false);
            }));
        }
    }
}