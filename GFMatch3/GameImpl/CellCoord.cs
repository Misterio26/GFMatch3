namespace GFMatch3.GameImpl {
    public struct CellCoord {
        public int X;
        public int Y;

        public CellCoord(int x, int y) {
            X = x;
            Y = y;
        }

        public static CellCoord operator +(CellCoord v1, CellCoord v2) {
            return new CellCoord(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static CellCoord operator -(CellCoord v1, CellCoord v2) {
            return new CellCoord(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static bool operator ==(CellCoord v1, CellCoord v2) {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        public static bool operator !=(CellCoord v1, CellCoord v2) {
            return v1.X != v2.X || v1.Y != v2.Y;
        }
    }
}