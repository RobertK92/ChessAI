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

        public Node[,] Nodes { get; private set; }
        public Dictionary<ControllingUnit, List<Piece>> PieceDict { get; private set; }

        public Board() : base("chess_board")
        {
            DrawOrder = (int)DrawLayer.Board;

            Nodes = new Node[8, 8];
            PieceDict = new Dictionary<ControllingUnit, List<Piece>>();
            PieceDict.Add(ControllingUnit.AI, new List<Piece>());
            PieceDict.Add(ControllingUnit.Human, new List<Piece>());

            float yOffset = (Chess.Instance.ScreenSize.Y - Size.Y) / 2;
            float xOffset = (Chess.Instance.ScreenSize.X - Size.X) / 2;
            Position = new Vector2(Size.X / 2 + xOffset, Size.Y / 2 + yOffset);
            
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < Nodes.GetLength(0); x++)
                {
                    Nodes[x, y] = new Node(new Vector2(
                        Bounds.Left + (NodeSize / 2) + x * NodeSize, 
                        Bounds.Top + (NodeSize / 2) + y * NodeSize), x, y);
                }
            }

            CreatePieces(ControllingUnit.AI);
            CreatePieces(ControllingUnit.Human);
        }

        public void CreatePieces(ControllingUnit controllingUnit)
        {
            int yOffset = (controllingUnit == ControllingUnit.AI) ? 1 : 6;
            int firstColumn = (controllingUnit == ControllingUnit.AI) ? 0 : 7;

            // Create pawns
            for (int i = 0; i < 8; i++)
            {
                PieceDict[controllingUnit].Add(new Piece(PieceType.Pawn, controllingUnit, Nodes[i, yOffset]));
            }

            // Create knights
            PieceDict[controllingUnit].Add(new Piece(PieceType.Knight, controllingUnit, Nodes[1, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Knight, controllingUnit, Nodes[6, firstColumn]));
            
            //TODO: create more..
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

        protected override void OnDestroy()
        {
            foreach (var pieceKvp in PieceDict)
            {
                for (int i = 0; i < pieceKvp.Value.Count; i++)
                {
                    pieceKvp.Value[i].Destroy();
                }
            }
        }
    }
}
