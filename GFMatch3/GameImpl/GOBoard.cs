using System;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBoard : GameObject {
        private GameObject[,] _cells = new GameObject[BoardGameConfig.BoardSize, BoardGameConfig.BoardSize];

        public GOBoard() {
            for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                    GameObject boardEl = new GOBoardElement();
                    boardEl.Transform.X = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * x;
                    boardEl.Transform.Y = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * y;
                    _cells[x, y] = boardEl;
                    AddChild(boardEl);
                }
            }

            AddAction(new GameActionDelegated(gameObject => {
                int boardSizeCfg = BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize;

                int availableWidth = Math.Max(0, GameDirector.Instance.ScreenWidth - 20);
                int availableHeight = Math.Max(0, GameDirector.Instance.ScreenHeight - 70 - 10);

                double boardSize = Math.Min(availableWidth, availableHeight);
                double boardX = 10 + (availableWidth - boardSize) / 2;
                double boardY = 70 + (availableHeight - boardSize) / 2;

                double boardScale = boardSize / boardSizeCfg;

                gameObject.Transform.X = boardX;
                gameObject.Transform.Y = boardY;
                gameObject.Transform.ScaleX = boardScale;
                gameObject.Transform.ScaleY = boardScale;
            }, null, null));
        }
    }
}