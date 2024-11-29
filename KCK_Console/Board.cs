using System.Numerics;

namespace Logic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        public int Score { get; set; }
        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }
        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }
        public Board()
        {
            Score = 0;
        }
        public static Board CreateNew()
        {
            var board = new Board();
            board.SetNewBoard();
            return board;
        }
        private void SetNewBoard()
        {
            /*pawns*/
            for (int i = 0; i < 8; i++)
            {
                /*white*/
                this[1, i] = new Pawn(Player.White);
                /*black*/
                this[6, i] = new Pawn(Player.Black);
            }
            /*white*/
            this[0, 0] = new Rook(Player.White);
            this[0, 7] = new Rook(Player.White);
            this[0, 1] = new Knight(Player.White);
            this[0, 6] = new Knight(Player.White);
            this[0, 2] = new Bishop(Player.White);
            this[0, 5] = new Bishop(Player.White);
            this[0, 3] = new Queen(Player.White);
            this[0, 4] = new King(Player.White);
            /*black*/
            this[7, 0] = new Rook(Player.Black);
            this[7, 7] = new Rook(Player.Black);
            this[7, 1] = new Knight(Player.Black);
            this[7, 6] = new Knight(Player.Black);
            this[7, 2] = new Bishop(Player.Black);
            this[7, 5] = new Bishop(Player.Black);
            this[7, 3] = new Queen(Player.Black);
            this[7, 4] = new King(Player.Black);
        }
        public static bool IsInside(Position position)
        {
            return position.Row >= 0 && position.Column >= 0 && position.Row < 8 && position.Column < 8;
        }
        public bool IsEmpty(Position position)
        {
            return this[position] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var position = new Position(i, j);
                    if (!IsEmpty(position))
                    {
                        yield return position;
                    }
                }
            }
        }
        public IEnumerable<Position> PiecePositionFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }
        public bool IsInCheck(Player player)
        {
            return PiecePositionFor(PlayerExtentions.getOponent(player)).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOponentKing(pos, this);
            });
        }
        public IEnumerable<Position> GetKings()
        {
            return PiecePositions().Where(pos => this[pos].Type == PieceType.King);
        }
        public IEnumerable<Position> GetPiecesOfType(Player color, PieceType type, Position? except = null)
        {
            if (except == null)
            {
                return PiecePositionFor(color).Where(pos => this[pos].Type == type);
            }
            return PiecePositionFor(color).Where(pos => pos != except && this[pos].Type == type);
        }
        public Board Copy()
        {
            var copy = new Board();
            foreach (var position in PiecePositions())
            {
                copy[position] = this[position].Copy();
            }
            return copy;
        }
    }
}
