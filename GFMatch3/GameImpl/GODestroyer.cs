using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    public class GODestroyer : GameObject {
        private int _direction;

        public GODestroyer(int coloredType, int direction) {
            _direction = direction;
            Renderer = new GRDestroyer(coloredType, direction);

            AddAction(new GameActionDelegated(gameObject => {
                    double boardSize = BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize;
                    double speed = BoardGameConfig.BoardCellSize / BoardGameConfig.DestoryerTimeForCell;
                    Point directionVect = GameMath.Directions[((GODestroyer) gameObject)._direction];
                    gameObject.Transform.X += directionVect.X * GameDirector.Instance.DeltaTime * speed;
                    gameObject.Transform.Y += directionVect.Y * GameDirector.Instance.DeltaTime * speed;

                    Point nosePoint = new Point(
                        gameObject.Transform.X + directionVect.X * (BoardGameConfig.BoardCellSize - 20) / 2,
                        gameObject.Transform.Y + directionVect.Y * (BoardGameConfig.BoardCellSize - 20) / 2
                    );

                    gameObject.GetScene<SceneBoard>()?.GetBoard()?.TryActivateUnderPoint(nosePoint);

                    if (gameObject.Transform.X < -BoardGameConfig.BoardCellSize
                        || gameObject.Transform.X > boardSize + BoardGameConfig.BoardCellSize
                        || gameObject.Transform.Y < -BoardGameConfig.BoardCellSize
                        || gameObject.Transform.Y > boardSize + BoardGameConfig.BoardCellSize) {
                        gameObject.RemoveFromParent();
                    }
                }, gameObject => { AnimationsManager.Instance.BlockeableAnimationStarted(); },
                gameObject => { AnimationsManager.Instance.BlockeableAnimationStoped(); }));
        }
    }
}