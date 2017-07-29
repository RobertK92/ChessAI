using System;

namespace ChessAI
{
    public static class Extensions
    {
        public static string GetTextureName(this PieceType pieceType, ControllingUnit controllingUnit)
        {
            string postfix = (controllingUnit == ControllingUnit.AI) ? "_black" : "";

            switch (pieceType)
            {
                case PieceType.Pawn:
                    return "piece_pawn" + postfix;
                case PieceType.Knight:
                    return "piece_knight" + postfix;
                case PieceType.Bishop:
                    return "piece_bishop" + postfix;
                case PieceType.Rook:
                    return "piece_tower" + postfix;
                case PieceType.Queen:
                    return "piece_queen" + postfix;
                case PieceType.King:
                    return "piece_king" + postfix;
                default:
                    throw new ArgumentException("Invalid piece type");
            }
        }
    }
}
