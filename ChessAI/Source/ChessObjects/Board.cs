using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ChessAI
{
    public class Board : Sprite
    {
        public const int NodeSize = 85;

        public BoardNode[,] Nodes = new BoardNode[8, 8];
               
        public Board() : base("chess_board")
        {
            float yOffset = (Chess.Instance.ScreenSize.Y - Size.Y) / 2;
            float xOffset = (Chess.Instance.ScreenSize.X - Size.X) / 2;
            Position = new Vector2(Size.X / 2 + xOffset, Size.Y / 2 + yOffset);
            
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < Nodes.GetLength(0); x++)
                {
                    Nodes[x, y] = new BoardNode(new Vector2(
                        Bounds.Left + (NodeSize / 2) + x * NodeSize, 
                        Bounds.Top  + (NodeSize / 2) + y * NodeSize), x, y);
                }
            }
        }

        protected override void DebugDraw(DebugDrawer drawer)
        {
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < Nodes.GetLength(0); x++)
                {
                    drawer.DrawCircle(Nodes[x, y].Position, 32, Color.Red, DrawingSpace.Screen);
                    drawer.DrawText(Nodes[x, y].Position, Nodes[x, y].ToString(), Color.Black, DrawingSpace.Screen);
                }
            }
        }
    }
}
