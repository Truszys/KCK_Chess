namespace Logic
{
    public class Queen : Piece
    {
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };
        public Queen(Player Color)
        {
            Mark = "Q";
            Type = PieceType.Queen;
            this.Color = Color;
            value = 9;
        }
        public override Piece Copy()
        {
            var copy = new Queen(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
