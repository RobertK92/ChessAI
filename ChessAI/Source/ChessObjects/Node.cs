using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameToolkit;
using Microsoft.Xna.Framework;

namespace ChessAI
{
    public class Node 
    {
        public readonly Vector2 Position;
        public readonly int Xindex;
        public readonly int Yindex;
        public readonly string Name;

        public Piece Piece { get; set; }
        public bool IsFree { get { return Piece == null; } }

        public Node(Vector2 position, int xIndex, int yIndex)
        {
            this.Xindex = xIndex;
            this.Yindex = yIndex;
            this.Position = position;

            Name = GetNodeName(Xindex, Yindex);
        }

        public static string GetNodeName(int x, int y)
        {
            return new string(new char[] { "ABCDEFGH"[x], "12345678"[y] });
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
