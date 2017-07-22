using System;
using System.Collections.Generic;
using System.Linq;
using SharpUtils;
using MonoGameToolkit;
using Microsoft.Xna.Framework;

namespace ChessAI
{
    public class Piece : Sprite 
    {
        public readonly ControllingUnit ControllingUnit;
        public readonly PieceType PieceType;
        public readonly Node StartNode;

        public Piece(PieceType pieceType, ControllingUnit controllingUnit, Node startNode) : base()
        {
            this.StartNode = startNode;
            this.ControllingUnit = controllingUnit;
            this.PieceType = pieceType;

            DrawOrder = (int)DrawLayer.Piece;
            ChangeTexture(pieceType.GetTextureName(controllingUnit), true, true);
            Position = startNode.Position;
        }
    }
}
