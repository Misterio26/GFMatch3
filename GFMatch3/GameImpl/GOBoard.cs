﻿using System;
using System.Collections.Generic;
using System.Windows;
using GFMatch3.GameCore;
using GFMatch3.GameTools;

namespace GFMatch3.GameImpl {
    /// <summary>
    /// Основной игровой элемент - доска.
    /// Собственно вся основная логика оп игре тут: размещение, уничтожение, заполнение элементов
    /// </summary>
    public class GOBoard : GameObject {
        private GOBoardElement[,] _cells = new GOBoardElement[BoardGameConfig.BoardSize, BoardGameConfig.BoardSize];

        private bool _hasSelectedCell;
        private CellCoord _selectedCell;

        private Random _random;

        private bool _hasSwap;
        private CellCoord _swapFromCellCoord;
        private CellCoord _swapToCellCoord;

        private List<MovedEl> _lastMovedElements = new List<MovedEl>();

        public GOBoard() {
            GameRenderer renderer = new GameRenderer();
            Renderer = renderer;
            renderer.SetClip(new Rect(0, 0, BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize,
                BoardGameConfig.BoardCellSize * BoardGameConfig.BoardSize));

            _random = new Random();

            for (int x = 0; x < BoardGameConfig.BoardSize; x++) {
                for (int y = 0; y < BoardGameConfig.BoardSize; y++) {
                    GOBoardElement boardEl = new BEGem(_random.Next(BoardGameConfig.ColoredTypes.Length));
                    boardEl.Transform = ApplyCellToTransform(boardEl.Transform, new CellCoord(x, y));
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

            // простой конечный автомат из BoardStateAction
            AddAction(new BoardStateActionPlayer());
        }

        public bool SelectCell(CellCoord cellCoord) {
            if (!IsCellInRange(cellCoord)) return false;
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

                        _lastMovedElements.Clear();
                        _lastMovedElements.Add(new MovedEl(cellCoord, _selectedCell));
                        _lastMovedElements.Add(new MovedEl(_selectedCell, cellCoord));

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

        /// <summary>
        /// Именно этот метод производит "активацию" совпавших элементов и создает бонусы.
        /// Немного перегружен.
        /// </summary>
        /// <param name="silent">Если true, то не будет инициировано никаких спец способностей элементов или анимаций,
        /// а также не будет попыток удалить объекты со сцены, все манипуляции проиcходят чисто на уровне _cell</param>
        /// <returns>Было ли хоть одно совпадение</returns>
        public bool ActivateMatches(bool silent) {
            // простая активация всех подряд
            List<ActivationEl[]> activateList = new List<ActivationEl[]>();
            ActivateMatchesByDirection(false, activateList);
            ActivateMatchesByDirection(true, activateList);


            List<ActivationEl> createBonuses = new List<ActivationEl>();

            if (!silent) {
                bool[,] bombsCheckGrid = new bool[BoardGameConfig.BoardSize, BoardGameConfig.BoardSize];

                // дополнительная логика по определению спауна бонусов
                // исходя из того, как была сделан уборка на предыдущем шаге
                foreach (ActivationEl[] cellCoords in activateList) {
                    if (cellCoords.Length >= 4) {
                        // преоритетна та точка, что была сдвинута в текущей фазе
                        ActivationEl spawnCoord = null;
                        bool vertical = true;
                        bool found = false;
                        foreach (ActivationEl cellCoord in cellCoords) {
                            foreach (MovedEl lastMovedElement in _lastMovedElements) {
                                if (cellCoord.CellCoord == lastMovedElement.ToCellCoord) {
                                    spawnCoord = cellCoord;
                                    vertical = (lastMovedElement.ToCellCoord - lastMovedElement.FromCellCoord).X == 0;
                                    found = true;
                                    break;
                                }
                            }
                            if (found) break;
                        }

                        if (!found) {
                            spawnCoord = cellCoords[0];
                        }

                        if (cellCoords.Length == 4) {
                            GOBoardElement lineElement = new BELine(spawnCoord.BoardElement.ColoredType, vertical);
                            createBonuses.Add(new ActivationEl(spawnCoord.CellCoord, lineElement));
                        } else {
                            GOBoardElement bombElement = new BEBomb(spawnCoord.BoardElement.ColoredType);
                            // бомбы преоритетнее
                            createBonuses.Insert(0, new ActivationEl(spawnCoord.CellCoord, bombElement));
                        }
                    }

                    foreach (ActivationEl cellCoord in cellCoords) {
                        if (bombsCheckGrid[cellCoord.CellCoord.X, cellCoord.CellCoord.Y]) {
                            GOBoardElement bombElement = new BEBomb(cellCoord.BoardElement.ColoredType);
                            // бомбы преоритетнее
                            createBonuses.Insert(0, new ActivationEl(cellCoord.CellCoord, bombElement));
                        }
                        bombsCheckGrid[cellCoord.CellCoord.X, cellCoord.CellCoord.Y] = true;
                    }
                }
            }

            // убираем все, что надо с проверкой на быстро или нет там, где бонусы появились
            foreach (ActivationEl[] cellCoords in activateList) {
                foreach (ActivationEl cellCoord in cellCoords) {
                    bool fast = false;
                    foreach (ActivationEl bonus in createBonuses) {
                        if (bonus.CellCoord == cellCoord.CellCoord) {
                            fast = true;
                            break;
                        }
                    }

                    ActivateElementInCell(cellCoord.CellCoord, silent, fast);
                }
            }

            // спауним бонусы
            foreach (ActivationEl bonus in createBonuses) {
                TrySpawnElement(bonus.CellCoord, bonus.BoardElement);
            }

            return activateList.Count > 0;
        }

        private void ActivateMatchesByDirection(bool swapSF, List<ActivationEl[]> activateList) {
            for (int f = 0; f < BoardGameConfig.BoardSize; f++) {
                int previousType = -1;
                int previousTypeStartPosition = -1;
                int matchesCounter = 0;
                for (int s = 0; s < BoardGameConfig.BoardSize; s++) {
                    GOBoardElement element = _cells[swapSF ? f : s, swapSF ? s : f];
                    int curType = element == null ? -1 : element.ColoredType;

                    if (previousType != curType || curType == -1) {
                        if (previousType >= 0 && matchesCounter >= BoardGameConfig.MinimalMatchesInRow) {
                            ActivationEl[] cellCoords = new ActivationEl[s - previousTypeStartPosition];
                            activateList.Add(cellCoords);
                            for (int i = previousTypeStartPosition; i < s; i++) {
                                cellCoords[i - previousTypeStartPosition] = new ActivationEl(
                                    new CellCoord(swapSF ? f : i, swapSF ? i : f),
                                    _cells[swapSF ? f : i, swapSF ? i : f]
                                );
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
                    ActivationEl[] cellCoords = new ActivationEl[BoardGameConfig.BoardSize - previousTypeStartPosition];
                    activateList.Add(cellCoords);
                    for (int i = previousTypeStartPosition; i < BoardGameConfig.BoardSize; i++) {
                        cellCoords[i - previousTypeStartPosition] = new ActivationEl(
                            new CellCoord(swapSF ? f : i, swapSF ? i : f), _cells[swapSF ? f : i, swapSF ? i : f]
                        );
                    }
                }
            }
        }

        public void TryActivateElementInCell(CellCoord cellCoord, bool silent, bool fast) {
            if (!IsCellInRange(cellCoord)) return;
            ActivateElementInCell(cellCoord, silent, fast);
        }

        private void ActivateElementInCell(CellCoord cellCoord, bool silent, bool fast) {
            GOBoardElement boardElement = _cells[cellCoord.X, cellCoord.Y];
            _cells[cellCoord.X, cellCoord.Y] = null;

            if (silent) return;

            if (boardElement != null) {
                boardElement.Activate(cellCoord, fast);
            }
        }

        public bool CondenseStep(bool silent) {
            _lastMovedElements.Clear();
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
                            boardEl.Transform = ApplyCellToTransform(boardEl.Transform, new CellCoord(x, y));

                            if (!silent) {
                                _lastMovedElements.Add(new MovedEl(new CellCoord(x, y), new CellCoord(x, y - 1)));
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

        public void SpawnDestroyers(CellCoord cellCoord, bool vertical, int coloredType) {
            GODestroyer destroyer1 = new GODestroyer(coloredType,
                vertical ? GameMath.DirectionUp : GameMath.DirectionLeft);
            destroyer1.Transform = ApplyCellToTransform(destroyer1.Transform, cellCoord);
            destroyer1.Transform.Z = 100;
            AddChild(destroyer1);

            GODestroyer destroyer2 = new GODestroyer(coloredType,
                vertical ? GameMath.DirectionDown : GameMath.DirectionRight);
            destroyer2.Transform = ApplyCellToTransform(destroyer2.Transform, cellCoord);
            destroyer2.Transform.Z = 100;
            AddChild(destroyer2);
        }

        private void TrySpawnElement(CellCoord cellCoord, GOBoardElement boardElement) {
            if (!IsCellInRange(cellCoord)) return;
            if (_cells[cellCoord.X, cellCoord.Y] != null) return;
            _cells[cellCoord.X, cellCoord.Y] = boardElement;
            AddChild(boardElement);

            boardElement.Transform = ApplyCellToTransform(boardElement.Transform, cellCoord);
        }

        public void TryActivateUnderPoint(Point point) {
            CellCoord cellCoord = PointToCell(point);
            if (!IsCellInRange(cellCoord)) return;
            ActivateElementInCell(cellCoord, false, false);
        }

        private GameTransform ApplyCellToTransform(GameTransform gameTransform, CellCoord cellCoord) {
            Point point = CellToPoint(cellCoord);
            gameTransform.X = point.X;
            gameTransform.Y = point.Y;
            gameTransform.Z = cellCoord.X + cellCoord.Y;
            return gameTransform;
        }

        public CellCoord PointToCell(Point point) {
            return new CellCoord(
                (int) Math.Floor(point.X / BoardGameConfig.BoardCellSize),
                (int) Math.Floor(point.Y / BoardGameConfig.BoardCellSize)
            );
        }

        private Point CellToPoint(CellCoord cellCoord) {
            return new Point(
                BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * cellCoord.X,
                BoardGameConfig.BoardCellSize / 2 + BoardGameConfig.BoardCellSize * cellCoord.Y
            );
        }

        private bool IsCellInRange(CellCoord cellCoord) {
            return cellCoord.X >= 0 && cellCoord.X < BoardGameConfig.BoardSize &&
                   cellCoord.Y >= 0 && cellCoord.Y < BoardGameConfig.BoardSize;
        }

        public bool IsPointInRange(Point point) {
            return point.X >= 0 && point.X < BoardGameConfig.BoardSize * BoardGameConfig.BoardCellSize &&
                   point.Y >= 0 && point.Y < BoardGameConfig.BoardSize * BoardGameConfig.BoardCellSize;
        }

        private class ActivationEl {
            public readonly CellCoord CellCoord;
            public readonly GOBoardElement BoardElement;

            public ActivationEl(CellCoord cellCoord, GOBoardElement boardElement) {
                CellCoord = cellCoord;
                BoardElement = boardElement;
            }
        }

        private class MovedEl {
            public readonly CellCoord ToCellCoord;
            public readonly CellCoord FromCellCoord;

            public MovedEl(CellCoord toCellCoord, CellCoord fromCellCoord) {
                ToCellCoord = toCellCoord;
                FromCellCoord = fromCellCoord;
            }
        }
    }
}