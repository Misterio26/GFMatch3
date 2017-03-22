namespace GFMatch3.GameImpl {
    /// <summary>
    /// Элемент на поле - БОНУС ЛИНИЯ.
    /// </summary>
    public class BELine : GOBoardElement {

        private bool _vertical;

        public bool Vertical => _vertical;

        public BELine(int coloredType, bool vertical) : base(coloredType, new GRBoardElementLine(coloredType, vertical)) {
            _vertical = vertical;
        }

        public override void OnActivate(CellCoord cellCoord, bool fast) {
            GetScene<SceneBoard>()?.GetBoard()?.SpawnDestroyers(cellCoord, _vertical, ColoredType);
            RemoveFromParent();
        }
    }
}