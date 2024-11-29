using System.Linq;
using System.Text.RegularExpressions;

namespace Logic
{
    public abstract class Move
    {
        public abstract MoveType MoveType { get; }
        public Position From { get; set; }
        public Position To { get; set; }
        public abstract void Execute(Board board);
        public virtual bool IsLegal(Board board)
        {
            var player = board[From].Color;
            var copy = board.Copy();
            Execute(copy);
            return !copy.IsInCheck(player);
        }
        public string GetNotation(Board board)
        {
            Board copy;
            Position kingPosition;
            bool isCheck, isMate = false;//[toDo] mate check
            var piece = board[From];
            Player oponent = PlayerExtentions.getOponent(piece.Color);
            if (piece.Type != PieceType.King)
            {
                var otherPiecesOfType = board.GetPiecesOfType(piece.Color, piece.Type, From);
                if (otherPiecesOfType.Any())
                {
                    var otherInCol = false;
                    var otherInRow = false;
                    foreach (var pos in otherPiecesOfType)
                    {
                        if (board[pos].GetPosibleMoves(pos, board).Where(move => move.To == To).Any())
                        {
                            if (pos.Column == From.Column)
                            {
                                otherInCol = true;
                            }
                            else if (pos.Row == From.Row || piece.Type == PieceType.Knight)
                            {
                                otherInRow = true;
                            }
                        }
                    }
                    var FromNotation = From.GetNotation();
                    copy = board.Copy();
                    Execute(copy);
                    isCheck = copy.IsInCheck(oponent);
                    //if (isCheck)
                    //{
                    //    kingPosition = copy.GetKings().Where(p => copy[p].Color == oponent).First();
                    //    isMate = copy[kingPosition].GetPosibleMoves(kingPosition, copy).Any();
                    //}
                    return (piece.Type == PieceType.Pawn ? "" : piece.Mark) + 
                        (otherInRow ? FromNotation[0] : "") + (otherInCol ? FromNotation[1] : "") + 
                        (board[To] != null ? "x" : "") + 
                        To.GetNotation() +
                        (isCheck ? (isMate ? "#" : "+") : "");
                }
            }
            copy = board.Copy();
            Execute(copy);
            isCheck = copy.IsInCheck(oponent);
            //if (isCheck)
            //{
            //    kingPosition = copy.GetKings().Where(p => copy[p].Color == oponent).First();
            //    isMate = copy[kingPosition].GetPosibleMoves(kingPosition, copy).Any();
            //}
            return (piece.Type == PieceType.Pawn ? "" : piece.Mark) + (board[To] != null ? "x" : "") + To.GetNotation() + (isCheck ? (isMate ? "#" : "+") : "");
        }
        public static Move? GetMoveFromNotation(string notation, Player player, Board board)
        {
            var copy = Regex.Replace(notation, "[+#]", "");
            Position? from = null, to = null;
            PieceType piece;
            switch(copy[0])
            {
                case 'B':
                    if (Regex.Match(notation, @"^B[a-h]{0,1}[1-8]{0,1}x{0,1}[a-h][1-8]").Success)
                        piece = PieceType.Bishop;
                    else
                        return null;
                    break;
                case 'K':
                    if (Regex.Match(notation, @"^Kx{0,1}[a-h][1-8]").Success)
                        piece = PieceType.King;
                    else
                        return null;
                    break;
                case 'Q':
                    if (Regex.Match(notation, @"^Q[a-h]{0,1}[1-8]{0,1}x{0,1}[a-h][1-8]").Success)
                        piece = PieceType.Queen;
                    else
                        return null;
                    break;
                case 'N':
                    if (Regex.Match(notation, @"^N[a-h]{0,1}[1-8]{0,1}x{0,1}[a-h][1-8]").Success)
                        piece = PieceType.Knight;
                    else
                        return null;
                    break;
                case 'R':
                    if (Regex.Match(notation, @"^R[a-h]{0,1}[1-8]{0,1}x{0,1}[a-h][1-8]").Success)
                        piece = PieceType.Rook;
                    else
                        return null;
                    break;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                    if (Regex.Match(notation, @"^([a-h]x){0,1}([a-h][1-8]){1}").Success)
                        piece = PieceType.Pawn;
                    else
                        return null;
                    break;
                default:
                    if (copy == "0-0")
                    {
                        //castleQS
                    }
                    else if (copy == "0-0-0")
                    {
                        //castleKS
                    }
                    return null;
            }
            var pieces = board.GetPiecesOfType(player, piece);
            switch(piece)
            {
                case PieceType.Pawn:
                    if (copy.Contains('x'))
                    {
                        to = Position.GetPositionFromNotation(copy[2..4]);
                        if (to != null)
                        {
                            var posible = pieces.Where(p => p.Column == copy[0] - 'a');
                            if (posible.Any())
                            {
                                foreach (var p in posible)
                                {
                                    var moves = board[p].GetPosibleMoves(p, board).Where(m => m.To == to);
                                    if (moves.Any())
                                    {
                                        from = p;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        to = Position.GetPositionFromNotation(copy);
                        if (to != null)
                        {
                            var posible = pieces.Where(p => p.Column == copy[0] - 'a');
                            if (posible.Any())
                            {
                                foreach (var p in posible)
                                {
                                    var moves = board[p].GetPosibleMoves(p, board).Where(m => m.To == to);
                                    if (moves.Any())
                                    {
                                        from = p;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                default:
                    copy = copy[1..];
                    if (copy.Contains('x'))
                    {
                        var x = copy.Split('x');
                        to = Position.GetPositionFromNotation(x.Last()[..2]);
                        if (to != null)
                        {
                            if (x.Length == 1 || (x.Length == 2 && x[0] == ""))
                            {
                                foreach (var p in pieces)
                                {
                                    var moves = board[p].GetPosibleMoves(p, board).Where(m => m.To == to);
                                    if (moves.Any())
                                    {
                                        from = p;
                                        break;
                                    }
                                }
                            }
                            else if (x.Length == 2)
                            {
                                if (x[0].Length == 2)
                                {
                                    from = pieces.Where(p => p == Position.GetPositionFromNotation(x[0])).First();
                                }
                                else
                                {
                                    IEnumerable<Position> posible;
                                    if (x[0][0] >= 'a' && x[0][0] <= 'h')
                                    {
                                        posible = pieces.Where(p => p.Column == x[0][0] - 'a');
                                    }
                                    else
                                    {
                                        posible = pieces.Where(p => p.Row == x[0][0] - '1');
                                    }
                                    foreach (var p in posible)
                                    {
                                        var moves = board[p].GetPosibleMoves(p, board).Where(m => m.To == to);
                                        if (moves.Any())
                                        {
                                            from = p;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                    break;
            }
            if (to == null || from == null)
            {
                return null;
            }
            return new NormalMove(from, to);
        }
    }
}
