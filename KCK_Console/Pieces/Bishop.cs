namespace Logic
{
    public class Bishop : Piece
    {
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };
        public Bishop(Player Color)
        {
            Mark = "B";
            Type = PieceType.Bishop;
            this.Color = Color;
            value = 3;
        }
        public override Piece Copy()
        {
            var copy = new Bishop(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
