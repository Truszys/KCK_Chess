namespace Logic
{
    public class Pawn : Piece
    {
        private readonly Direction forward;
        public Pawn(Player Color)
        {
            Mark = "P";
            Type = PieceType.Pawn;
            this.Color = Color;
            value = 1;
            if (Color == Player.White)
            {
                forward = Direction.North;
            }
            else if (Color == Player.Black)
            {
                forward = Direction.South;
            }
        }
        public override Piece Copy()
        {
            var copy = new Pawn(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }
        private bool CanCaptureAt(Position position, Board board)
        {
            if (!Board.IsInside(position) || board.IsEmpty(position))
                return false;
            return board[position].Color != Color;
        }
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePosition = from + forward;
            if (CanMoveTo(oneMovePosition, board))
            {
                yield return new NormalMove(from, oneMovePosition);

                Position twoMovesPosition = oneMovePosition + forward;
                if (!wasMoved && CanMoveTo(twoMovesPosition, board))
                {
                    yield return new NormalMove(from, twoMovesPosition);
                }
            }
        }
        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (var dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir;
                if (CanCaptureAt(to, board))
                {
                    yield return new NormalMove(from, to);
                }
            }
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }
        public override bool CanCaptureOponentKing(Position from, Board board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                var piece = board[move.To];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
