using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Logic;

namespace UIApp
{
    public class Images
    {
        private string pieceType;
        private Dictionary<PieceType, ImageSource> whiteSources = new();
        private Dictionary<PieceType, ImageSource> blackSources = new();
        public Images(string pieceType)
        {
            this.pieceType = pieceType;
            addSources();
        }
        private void addSources()
        {
            whiteSources.Add(PieceType.Pawn, LoadImage($"Assets/{pieceType}/pawn-w.png"));
            whiteSources.Add(PieceType.Bishop, LoadImage($"Assets/{pieceType}/bishop-w.png"));
            whiteSources.Add(PieceType.Knight, LoadImage($"Assets/{pieceType}/knight-w.png"));
            whiteSources.Add(PieceType.Rook, LoadImage($"Assets/{pieceType}/rook-w.png"));
            whiteSources.Add(PieceType.Queen, LoadImage($"Assets/{pieceType}/queen-w.png"));
            whiteSources.Add(PieceType.King, LoadImage($"Assets/{pieceType}/king-w.png"));
            blackSources.Add(PieceType.Pawn, LoadImage($"Assets/{pieceType}/pawn-b.png"));
            blackSources.Add(PieceType.Bishop, LoadImage($"Assets/{pieceType}/bishop-b.png"));
            blackSources.Add(PieceType.Knight, LoadImage($"Assets/{pieceType}/knight-b.png"));
            blackSources.Add(PieceType.Rook, LoadImage($"Assets/{pieceType}/rook-b.png"));
            blackSources.Add(PieceType.Queen, LoadImage($"Assets/{pieceType}/queen-b.png"));
            blackSources.Add(PieceType.King, LoadImage($"Assets/{pieceType}/king-b.png"));
        }
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }
        public ImageSource GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.White => whiteSources[type],
                Player.Black => blackSources[type],
                _ => null
            };
        }
        public ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return GetImage(piece.Color, piece.Type);
        }
        public static ImageSource GetBoardImage(string boardType)
        {
            return LoadImage($"Assets/boards/{boardType}.jpg");
        }
    }
}
