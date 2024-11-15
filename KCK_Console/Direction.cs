namespace Logic
{
    public class Direction
    {
        public readonly static Direction North = new Direction(1, 0);
        public readonly static Direction South = new Direction(-1, 0);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East;
        public readonly static Direction NorthWest = North + West;
        public readonly static Direction SouthEast = South + East;
        public readonly static Direction SouthWest = South + West;
        public int RowD { get; }
        public int ColumnD { get; }
        public Direction(int rowD, int columnD)
        {
            RowD = rowD;
            ColumnD = columnD;
        }
        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.RowD + b.RowD, b.ColumnD + a.ColumnD);
        }
        public static Direction operator *(int scalar, Direction a)
        {
            return new Direction(scalar * a.RowD, scalar * a.ColumnD);
        }
    }
}
