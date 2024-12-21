using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIApp
{
    public enum BoardType
    {
        Black,
        Brown,
        Wood
    }
    public enum PieceStyleType
    {
        Modern,
        Simple
    }
    public class FrontSettings
    {
        public BoardType BoardType { get; set; } = BoardType.Wood;
        public PieceStyleType PieceType { get; set; } = PieceStyleType.Simple;
        public string GetBoardType()
        {
            return BoardType switch
            {
                BoardType.Black => "black",
                BoardType.Brown => "brown",
                BoardType.Wood => "wood",
                _ => "wood"
            };
        }
        public string GetPieceType()
        {
            return PieceType switch
            {
                PieceStyleType.Simple => "simple",
                PieceStyleType.Modern => "modern",
                _ => "modern"
            };
        }
    }
}
