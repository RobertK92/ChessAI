using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameToolkit;
using Microsoft.Xna.Framework;

namespace ChessAI
{
    public class BoardNode 
    {
        public readonly Vector2 Position;
        public readonly int Xindex;
        public readonly int Yindex;
        public readonly string Name;

        public BoardNode(Vector2 position, int xIndex, int yIndex)
        {
            this.Xindex = xIndex;
            this.Yindex = yIndex;
            this.Position = position;

            string xAxis = "ABCDEFGH";
            string yAxis = "12345678";
            Name = new string(new char[] { xAxis[Xindex], yAxis[yAxis.Length - (Yindex + 1)] });
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
