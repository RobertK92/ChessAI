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
        public const int NodeCountX = 8;
        public const int NodeCountY = 8;
        public const int NodeSize = 85;

        public Node[,] Nodes { get; private set; }
        public Dictionary<ControllingUnit, List<Piece>> PieceDict { get; private set; }

        public Board() : base("chess_board") { }

        protected override void Initialize()
        {
            DrawOrder = (int)DrawLayer.Board;

            Nodes = new Node[NodeCountX, NodeCountY];
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

                    if (x == 0)
                    {
                        TextField numberLabel = new TextField((Nodes.GetLength(1) - (y)).ToString());
                        numberLabel.Position = Nodes[x, y].Position - Vector2.UnitX * (NodeSize / 1.5f);
                    }
                    if (y == Nodes.GetLength(1) - 1)
                    {
                        TextField letterLabel = new TextField(Nodes[x, y].Name[0].ToString());
                        letterLabel.Position = Nodes[x, y].Position + Vector2.UnitY * (NodeSize / 1.5f);
                    }
                }
            }

            CreatePieces(ControllingUnit.AI);
            CreatePieces(ControllingUnit.Human);
        }

        public void CreatePieces(ControllingUnit controllingUnit)
        {
            int yOffset = (controllingUnit == ControllingUnit.AI) ? 1 : 6;
            int firstColumn = (controllingUnit == ControllingUnit.AI) ? 0 : 7;
            
            for (int i = 0; i < NodeCountX; i++)
                PieceDict[controllingUnit].Add(new Piece(PieceType.Pawn, controllingUnit, Nodes[i, yOffset]));

            PieceDict[controllingUnit].Add(new Piece(PieceType.Knight, controllingUnit, Nodes[1, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Knight, controllingUnit, Nodes[6, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Bishop, controllingUnit, Nodes[2, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Bishop, controllingUnit, Nodes[5, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Rook, controllingUnit,   Nodes[0, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Rook, controllingUnit,   Nodes[7, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.Queen, controllingUnit,  Nodes[3, firstColumn]));
            PieceDict[controllingUnit].Add(new Piece(PieceType.King, controllingUnit,   Nodes[4, firstColumn]));

            PieceDict[controllingUnit].ForEach(x => x.UpdatePossibleMoves());
        }

        protected override void DebugDraw(DebugDrawer drawer)
        {
            /*for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                for (int x = 0; x < Nodes.GetLength(0); x++)
                {
                    drawer.DrawCircle(Nodes[x, y].Position, 32, Color.Red, DrawingSpace.Screen);
                    drawer.DrawText(Nodes[x, y].Position, Nodes[x, y].ToString(), Color.Black, DrawingSpace.Screen);
                }
            }*/
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
