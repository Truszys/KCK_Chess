namespace Logic
{
    public class NormalMove : Move
    {
        public override MoveType MoveType => MoveType.Normal;
        public NormalMove(Position From, Position To)
        {
            this.From = From;
            this.To = To;
        }
        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            piece.wasMoved = true;
        }
        public override string GetNotation(Board board)
        {
            var piece = board[From];
            return (piece.Type == PieceType.Pawn ? "" : piece.Mark) + To.GetPositionNotation();
        }
    }
}
