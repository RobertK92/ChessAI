using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return "piece_pawn" + postfix;
                case PieceType.Tower:
                    return "piece_pawn" + postfix;
                case PieceType.Queen:
                    return "piece_pawn" + postfix;
                case PieceType.King:
                    return "piece_pawn" + postfix;
                default:
                    throw new ArgumentException("Invalid piece type");
            }
        }
    }
}
