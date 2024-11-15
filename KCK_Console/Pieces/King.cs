namespace Logic
{
    public class King : Piece
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
        public King(Player Color)
        {
            Mark = "K";
            Type = PieceType.King;
            this.Color = Color;
            value = 9999;
        }
        public override Piece Copy()
        {
            var copy = new King(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (var dir in dirs)
            {
                Position to = from + dir;
                if (!Board.IsInside(to))
                    continue;
                if (board.IsEmpty(to) || board[to].Color != Color)
                    yield return to;
            }
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            foreach (var to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }
        public override bool CanCaptureOponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                var piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
