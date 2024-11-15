namespace Logic
{
    public abstract class Piece
    {
        public string Mark {  get; set; }
        public PieceType Type { get; set; }
        public int value { get; set; }
        public Player Color { get; set; }
        public bool wasMoved { get; set; } = false;
        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetPosibleMoves(Position from, Board board);
        protected IEnumerable<Position> MovePositionsInDir(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }
                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }
                yield break;
            }
        }
        protected IEnumerable<Position> MovePositionsInDirs(Position from, Board board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionsInDir(from, board, dir));
        }
        public virtual bool CanCaptureOponentKing(Position from, Board board)
        {
            return GetPosibleMoves(from, board).Any(move =>
            {
                var piece = board[move.To];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
