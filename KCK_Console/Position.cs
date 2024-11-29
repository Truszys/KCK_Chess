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
        public static Position? GetPositionFromNotation(string str)
        {
            try
            {
                int col = str[0] - 'a';
                int row = str[1] - '1';
                var pos = new Position(row, col);
                if (Board.IsInside(pos))
                    return pos;
                return null;
            }
            catch
            {
                return null;
            }
        }
        public string GetNotation()
        {
            return ((char)('a' + Column)).ToString() + (char)('1' + Row);
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
