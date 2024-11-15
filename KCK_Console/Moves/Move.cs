namespace Logic
{
    public abstract class Move
    {
        public abstract MoveType MoveType { get; }
        public Position From { get; set; }
        public Position To { get; set; }
        public abstract void Execute(Board board);
        public abstract string GetNotation(Board board);
        public virtual bool IsLegal(Board board)
        {
            var player = board[From].Color;
            var copy = board.Copy();
            Execute(copy);
            return !copy.IsInCheck(player);
        }
    }
}
