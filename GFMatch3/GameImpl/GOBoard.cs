using System;
using System.Windows;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBoard : GameObject {
        private GOBoardElement[,] _cells = new GOBoardElement[BoardGameConfig.BoardSize, BoardGameConfig.BoardSize];

        private bool _hasSelectedCell;
        private int _selectedCellX;
        private int _selectedCellY;

        public GOBoard() {
            for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                    GOBoardElement boardEl = new GOBoardElement();
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

                if (GameDirector.Instance.IsMouseClick) {
                    Point clickPosition = gameObject.SceneToLocal(GameDirector.Instance.MousePosition);
                    if (clickPosition.X >= 0 && clickPosition.X < boardSizeCfg
                        && clickPosition.Y >= 0 && clickPosition.Y < boardSizeCfg) {

                        int cellX = (int) (clickPosition.X / BoardGameConfig.BoardCellSize);
                        int cellY = (int) (clickPosition.Y / BoardGameConfig.BoardCellSize);
                        ((GOBoard) gameObject).SelectCell(cellX, cellY);
                    }
                }
            }, null, null));
        }

        public void SelectCell(int x, int y) {
            if (_hasSelectedCell) {
                _cells[_selectedCellX, _selectedCellY].Selected = false;
                if (_selectedCellX == x && _selectedCellY == y) {
                    _hasSelectedCell = false;
                    return;
                }
            }
            _hasSelectedCell = true;
            _selectedCellX = x;
            _selectedCellY = y;
            _cells[x, y].Selected = true;
        }
    }
}