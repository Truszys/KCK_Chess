namespace Logic
{
    public class Knight : Piece
    {
        public Knight(Player Color)
        {
            Mark = "N";
            Type = PieceType.Knight;
            this.Color = Color;
            value = 3;
        }
        public override Piece Copy()
        {
            var copy = new Knight(Color);
            copy.wasMoved = wasMoved;
            return copy;
        }
        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {

                foreach (Direction hDir in new Direction[] { Direction.East, Direction.West })
                {
                    yield return from + 2 * vDir + hDir;
                    yield return from + 2 * hDir + vDir;
                }
            }
        }
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            return PotentialToPositions(from).Where(position => Board.IsInside(position)
                && (board.IsEmpty(position) || board[position].Color != Color));
        }
        public override IEnumerable<Move> GetPosibleMoves(Position from, Board board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}
