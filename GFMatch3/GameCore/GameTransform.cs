namespace GFMatch3.GameCore {
    public struct GameTransform {
        public static readonly GameTransform Default = new GameTransform(0, 0, 0, 0, 1, 1);
        public static readonly GameTransform Zero = new GameTransform(0, 0, 0, 0, 0, 0);

        public double X;
        public double Y;
        public double Z;
        public double Angle;
        public double ScaleX;
        public double ScaleY;

        public GameTransform(double x, double y, double z, double angle, double scaleX, double scaleY) {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        public bool IsDefault() {
            return X == 0 && Y == 0 && Z == 0 && Angle == 0 && ScaleX == 1 && ScaleY == 1;
        }

        public static GameTransform operator +(GameTransform v1, GameTransform v2) {
            return new GameTransform(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.Angle + v2.Angle, v1.ScaleX + v2.ScaleX,
                v1.ScaleY + v2.ScaleY);
        }

        public static GameTransform operator -(GameTransform v1, GameTransform v2) {
            return new GameTransform(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.Angle - v2.Angle, v1.ScaleX - v2.ScaleX,
                v1.ScaleY - v2.ScaleY);
        }

        public static GameTransform operator *(GameTransform v1, double v2) {
            return new GameTransform(v1.X * v2, v1.Y * v2, v1.Z * v2, v1.Angle * v2, v1.ScaleX * v2,
                v1.ScaleY * v2);
        }
    }
}