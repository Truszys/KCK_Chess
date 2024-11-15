namespace Logic
{
    public class Rook : Piece
    {
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        };
        public Rook(Player Color)
        {
            Mark = "R";
            Type = PieceType.Rook;
            this.Color = Color;
            value = 5;
        }
        public override Piece Copy()
        {
            var copy = new Rook(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            return MovePositionsInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}
