using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using GFMatch3.GameCore;

namespace GFMatch3.GameImpl {
    public class GOBoard : GameObject {
        private GOBoardElement[,] _cells = new GOBoardElement[BoardGameConfig.BoardSize, BoardGameConfig.BoardSize];

        private bool _hasSelectedCell;
        private CellCoord _selectedCell;

        private Random _random;

        private bool _hasSwap;
        private CellCoord _swapFromCellCoord;
        private CellCoord _swapToCellCoord;

        public GOBoard() {
            GameRenderer renderer = new GameRenderer();
            Renderer = renderer;
            renderer.SetClip(new Rect(0, 0, BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize,
                BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize));

            _random = new Random();

            for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                    GOBoardElement boardEl = new BEGem(_random.Next(BoardGameConfig.ColoredTypes.Length));
                    boardEl.Transform.X = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * x;
                    boardEl.Transform.Y = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * y;
                    boardEl.Transform.Z = x + y;
                    _cells[x, y] = boardEl;
                }
            }
            ActivateMatchesAndCondenseSilent();
            for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                    GOBoardElement boardEl = _cells[x, y];
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

            // простой конечный автомат из BorderStateAction
            AddAction(new BoardStateActionPlayer());
        }

        public bool SelectCell(CellCoord cellCoord) {
            GOBoardElement curBoardElement = _cells[cellCoord.X, cellCoord.Y];

            if (_hasSelectedCell) {
                _hasSelectedCell = false;
                GOBoardElement selectedBoardElement = _cells[_selectedCell.X, _selectedCell.Y];
                if (selectedBoardElement != null) {
                    selectedBoardElement.Selected = false;
                }

                if (_selectedCell == cellCoord) {
                    return false;
                }

                if (selectedBoardElement != null && curBoardElement != null) {
                    CellCoord offset = cellCoord - _selectedCell;

                    // если сосед, то автивируем свап
                    if (offset.X * offset.X + offset.Y * offset.Y == 1) {
                        SwapCells(cellCoord, _selectedCell);

                        if (!HasMatches()) {
                            _hasSwap = true;
                            _swapFromCellCoord = _selectedCell;
                            _swapToCellCoord = cellCoord;

                            return false;
                        }

                        return true;
                    }
                }
            }

            if (curBoardElement != null) {
                _hasSelectedCell = true;
                _selectedCell.X = cellCoord.X;
                _selectedCell.Y = cellCoord.Y;
                curBoardElement.Selected = true;
            }

            return false;
        }

        private void SwapCells(CellCoord fromCellCoord, CellCoord toCellCoord) {
            GOBoardElement fromBoardElement = _cells[fromCellCoord.X, fromCellCoord.Y];
            GOBoardElement toBoardElement = _cells[toCellCoord.X, toCellCoord.Y];

            _cells[toCellCoord.X, toCellCoord.Y] = fromBoardElement;
            _cells[fromCellCoord.X, fromCellCoord.Y] = toBoardElement;

            if (fromBoardElement != null) {
                fromBoardElement.Transform.X = BoardGameConfig.BoardCellSize / 2 +
                                               BoardGameConfig.BoardCellSize * toCellCoord.X;
                fromBoardElement.Transform.Y = BoardGameConfig.BoardCellSize / 2 +
                                               BoardGameConfig.BoardCellSize * toCellCoord.Y;
                fromBoardElement.Transform.Z = toCellCoord.X + toCellCoord.Y;
                fromBoardElement.AnimateTransitionFromCellToCell(fromCellCoord, toCellCoord);
            }

            if (toBoardElement != null) {
                toBoardElement.Transform.X = BoardGameConfig.BoardCellSize / 2 +
                                             BoardGameConfig.BoardCellSize * fromCellCoord.X;
                toBoardElement.Transform.Y = BoardGameConfig.BoardCellSize / 2 +
                                             BoardGameConfig.BoardCellSize * fromCellCoord.Y;
                toBoardElement.Transform.Z = fromCellCoord.X + fromCellCoord.Y;

                toBoardElement.AnimateTransitionFromCellToCell(toCellCoord, fromCellCoord);
            }
        }

        public void TrySwapBack() {
            if (!_hasSwap) return;
            _hasSwap = false;
            SwapCells(_swapFromCellCoord, _swapToCellCoord);
        }

        private bool HasMatches() {
            if (HasMatchesByDirection(false)) return true;
            if (HasMatchesByDirection(true)) return true;
            return false;
        }

        private bool HasMatchesByDirection(bool swapSF) {
            for (int f = 0; f < BoardGameConfig.BoardSize; f++) {
                int previousType = -1;
                int matchesCounter = 0;
                for (int s = 0; s < BoardGameConfig.BoardSize; s++) {
                    GOBoardElement element = _cells[swapSF ? f : s, swapSF ? s : f];
                    int curType = element == null ? -1 : element.ColoredType;

                    if (previousType != curType || curType == -1) {
                        if (previousType >= 0 && matchesCounter >= BoardGameConfig.MinimalMatchesInRow) {
                            return true;
                        }
                        matchesCounter = 1;
                        previousType = curType;
                    } else {
                        matchesCounter++;
                    }
                }
                if (previousType >= 0 && matchesCounter >= BoardGameConfig.MinimalMatchesInRow) {
                    return true;
                }
            }
            return false;
        }

        public bool ActivateMatches(bool silent) {
            HashSet<CellCoord> activateList = new HashSet<CellCoord>();
            ActivateMatchesByDirection(false, activateList);
            ActivateMatchesByDirection(true, activateList);
            foreach (CellCoord cellCoord in activateList) {
                ActivateElementInCell(cellCoord, silent);
            }

            return activateList.Count > 0;
        }

        private void ActivateMatchesByDirection(bool swapSF, HashSet<CellCoord> activateList) {
            for (int f = 0; f < BoardGameConfig.BoardSize; f++) {
                int previousType = -1;
                int previousTypeStartPosition = -1;
                int matchesCounter = 0;
                for (int s = 0; s < BoardGameConfig.BoardSize; s++) {
                    GOBoardElement element = _cells[swapSF ? f : s, swapSF ? s : f];
                    int curType = element == null ? -1 : element.ColoredType;

                    if (previousType != curType || curType == -1) {
                        if (previousType >= 0 && matchesCounter >= BoardGameConfig.MinimalMatchesInRow) {
                            for (int i = previousTypeStartPosition; i < s; i++) {
                                activateList.Add(new CellCoord(swapSF ? f : i, swapSF ? i : f));
                            }
                        }
                        matchesCounter = 1;
                        previousTypeStartPosition = s;
                        previousType = curType;
                    } else {
                        matchesCounter++;
                    }
                }
                if (previousType >= 0 && matchesCounter >= BoardGameConfig.MinimalMatchesInRow) {
                    for (int i = previousTypeStartPosition; i < BoardGameConfig.BoardSize; i++) {
                        activateList.Add(new CellCoord(swapSF ? f : i, swapSF ? i : f));
                    }
                }
            }
        }

        public void ActivateElementInCell(CellCoord cellCoord, bool silent) {
            GOBoardElement boardElement = _cells[cellCoord.X, cellCoord.Y];
            _cells[cellCoord.X, cellCoord.Y] = null;

            if (silent) return;

            if (boardElement != null) {
                boardElement.Activate();
            }
        }

        public bool CondenseStep(bool silent) {
            bool hasHole = false;
            for (int y = BoardGameConfig.BoardSize - 1; y >= 0; y--) {
                for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                    if (_cells[x, y] == null) {
                        hasHole = true;
                        if (y > 0) {
                            _cells[x, y] = _cells[x, y - 1];
                            _cells[x, y - 1] = null;
                        } else {
                            _cells[x, y] = new BEGem(_random.Next(BoardGameConfig.ColoredTypes.Length));
                            if (!silent) {
                                AddChild(_cells[x, y]);
                            }
                        }
                        GOBoardElement boardEl = _cells[x, y];
                        if (boardEl != null) {
                            boardEl.Transform.X = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * x;
                            boardEl.Transform.Y = BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * y;
                            boardEl.Transform.Z = x + y;

                            if (!silent) {
                                boardEl.AnimateTransitionFromCellToCell(new CellCoord(x, y - 1), new CellCoord(x, y));
                            }
                        }
                    }
                }
            }
            return hasHole;
        }

        private void ActivateMatchesAndCondenseSilent() {
            while (ActivateMatches(true)) {
                while (CondenseStep(true)) {
                }
            }
        }

        private void PrintDebug() {
            for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                    Debug.Write(_cells[x, y] == null ? " " : _cells[x, y].ColoredType.ToString());
                    Debug.Write(" ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("");
        }
    }
}