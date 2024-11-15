namespace Logic
{
    public class Position
    {
        public int Row;
        public int Column;
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public Position Copy()
        {
            return new Position(Row, Column);
        }
        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0)
                return Player.Black;
            return Player.White;
        }
        public static Position GetPositionFromNotation(string str)
        {
            return new Position(str[0] - 'a', int.Parse(str[1] + "") - 1);
        }
        public string GetPositionNotation()
        {
            return ((char)('a' + Row)).ToString() + (char)('1' + Column);
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Position? a, Position? b)
        {
            return EqualityComparer<Position>.Default.Equals(a, b);
        }

        public static bool operator !=(Position? a, Position? b)
        {
            return !(a == b);
        }
        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowD, pos.Column + dir.ColumnD);
        }
    }
}
