using System.Timers;

namespace Logic
{
    public class GameState
    {
        public List<Move> Moves { get; } = new List<Move>();
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;
        public Settings Settings { get; private set; }
        public System.Timers.Timer Timer { get; private set; }
        public int TimeWhite { get; set; }
        public int TimeBlack { get; set; }
        private readonly int Increment;
        public GameState(Board board, Player currentPlayer, Settings settings)
        {
            Board = board;
            CurrentPlayer = currentPlayer;
            Settings = settings;
            Increment = (int)settings.time % 60;
            TimeWhite = (int)settings.time - Increment;
            TimeBlack = (int)settings.time - Increment;
            Timer = new(1000);
            Timer.Elapsed += TickTimer;
        }
        public IEnumerable<Move> LegalMovesForPiece(Position position)
        {
            if (Board.IsEmpty(position) || Board[position].Color != CurrentPlayer)
                return Enumerable.Empty<Move>();

            var piece = Board[position];
            IEnumerable<Move> moves = piece.GetPosibleMoves(position, Board);
            return moves.Where(move => move.IsLegal(Board));
        }
        public void MakeMove(Move move)
        {
            if (Board[move.To] != null)
            {
                var piece = Board[move.To];
                if (piece.Color == Player.White)
                {
                    Board.Score -= piece.value;
                }
                else
                {
                    Board.Score += piece.value;
                }
            }
            Moves.Add(move);
            move.Execute(Board);
            SwitchTimer();
            CurrentPlayer = PlayerExtentions.getOponent(CurrentPlayer);
            CheckForGameOver();
        }
        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moves = Board.PiecePositionFor(player).SelectMany(pos =>
            {
                var piece = Board[pos];
                return piece.GetPosibleMoves(pos, Board);
            });
            return moves.Where(move => move.IsLegal(Board));
        }
        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if(Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(PlayerExtentions.getOponent(CurrentPlayer));
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
        }
        public bool IsGameOver()
        {
            return Result != null;
        }


        public void StartTimer()
        {
            Timer.Start();
        }
        public void PauseTimer()
        {
            Timer.Stop();
        }
        private void TickTimer(object sender, ElapsedEventArgs e)
        {
            if (CurrentPlayer == Player.White)
            {
                TimeWhite -= 1;
                if (TimeWhite == 0)
                {
                    Result = Result.LoseTime(CurrentPlayer);
                    CurrentPlayer = Player.None;
                }
            }
            else if (CurrentPlayer == Player.Black)
            {
                TimeBlack -= 1;
                if (TimeBlack == 0)
                {
                    Result = Result.LoseTime(CurrentPlayer);
                    CurrentPlayer = Player.None;
                }
            }
        }
        public void SwitchTimer()
        {
            Timer.Stop();
            if (CurrentPlayer == Player.White)
            {
                TimeWhite += Increment;
            }
            else if(CurrentPlayer == Player.Black)
            {
                TimeBlack += Increment;
            }
            Timer.Start();
        }
    }
}
