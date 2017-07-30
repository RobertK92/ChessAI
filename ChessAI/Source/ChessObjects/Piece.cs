using System;
using System.Collections.Generic;
using System.Linq;
using SharpUtils;
using MonoGameToolkit;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace ChessAI
{
    public class Piece : Sprite 
    {
        public readonly ControllingUnit ControllingUnit;
        public readonly PieceType PieceType;
        public readonly Node StartNode;
        
        public List<Node> PossibleMoves { get; private set; }
        public bool HasEverMoved { get; private set; }

        private Node currentNode;
        public Node CurrentNode
        {
            get { return currentNode; }
            set
            {
                currentNode = value;
                currentNode.Piece = this;
            }
        }

        public bool IsDragging  { get; set; }

        private Vector2 previousNodePosition;
        private float targetAlpha;

        public Piece(PieceType pieceType, ControllingUnit controllingUnit, Node startNode) : base()
        {
            this.StartNode = startNode;
            this.ControllingUnit = controllingUnit;
            this.PieceType = pieceType;
            this.CurrentNode = startNode;
            PossibleMoves = new List<Node>();
        }

        protected override void Initialize()
        {
            DrawOrder = (int)DrawLayer.Piece;
            ChangeTexture(PieceType.GetTextureName(ControllingUnit), true, true);
            Position = StartNode.Position;
            EnablePhysicsRectangle(FarseerPhysics.Dynamics.BodyType.Dynamic, Bounds);
            PhysicsBody.GravityScale = 0.0f;
            PhysicsBody.IsSensor = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if(Position != CurrentNode.Position)
            {
                targetAlpha += Chess.Instance.DeltaTime * 2.0f;
                targetAlpha = MathHelper.Clamp(targetAlpha, 0, 1);
                Position = Vector2.Lerp(previousNodePosition, CurrentNode.Position, targetAlpha);
                
            }
            
        }

        public void Move(Node node)
        {
            previousNodePosition = CurrentNode.Position;
            this.CurrentNode = node;
            HasEverMoved = true;
            targetAlpha = 0.0f;
        }

        public void UpdatePossibleMoves()
        {
            PossibleMoves.Clear();
            
            switch (PieceType)
            {
                case PieceType.Pawn:
                    CheckAndSetPossibleMoves(MoveDirection.Up, 2, 0, true);
                    break;
                case PieceType.Knight:
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Right, 1, 2, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Left,  1, 2, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Left,  2, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Right, 2, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Right, 1, 2, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Left,  1, 2, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Left,  2, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Right, 2, 1, false);
                    break;
                case PieceType.Bishop:
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Left, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Right, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Right, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Left, 8, 8, true);
                    break;
                case PieceType.Rook:
                    CheckAndSetPossibleMoves(MoveDirection.Down, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Up, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Left, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Right, 8, 8, true);
                    break;
                case PieceType.Queen:
                    CheckAndSetPossibleMoves(MoveDirection.Down, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Up, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Left, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Right, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Left, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Right, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Right, 8, 8, true);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Left, 8, 8, true);
                    break;
                case PieceType.King:
                    CheckAndSetPossibleMoves(MoveDirection.Down, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Left, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Right, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Left, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Up | MoveDirection.Right, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Right, 1, 1, false);
                    CheckAndSetPossibleMoves(MoveDirection.Down | MoveDirection.Left, 1, 1, false);
                    break;
                default:
                    break;
            }
        }

        private void CheckAndSetPossibleMoves(MoveDirection direction, int stepsX, int stepsY, bool inBetweenSteps, int iterationX = 1, int iterationY = 1)
        {
            Board board = Scene.GetObject<Board>();
            if (board == null)
            {
                throw new NullReferenceException("No board found");
            }

            if (!inBetweenSteps)
            {
                iterationX = stepsX;
                iterationY = stepsY;
            } 

            int side = ControllingUnit == ControllingUnit.Human ? -1 : 1;

            int left    = Convert.ToInt32(((direction & MoveDirection.Left) == MoveDirection.Left)) * iterationX;
            int right   = Convert.ToInt32(((direction & MoveDirection.Right) == MoveDirection.Right)) * iterationX * -1;
            int up      = Convert.ToInt32(((direction & MoveDirection.Up) == MoveDirection.Up)) * iterationY * side;
            int down    = Convert.ToInt32(((direction & MoveDirection.Down) == MoveDirection.Down)) * iterationY * side * -1;

            int x = (CurrentNode.Xindex + left + right);
            int y = (CurrentNode.Yindex + up + down);
            bool markForAdd = false;

            if (board.Nodes.IsInBounds(x, y))
            {
                Node newNode = board.Nodes[x, y];    
                if (newNode.IsFree)
                {
                    markForAdd = true;
                }
                else
                {
                    if (newNode.Piece.ControllingUnit != ControllingUnit)
                    {
                        markForAdd = true;
                    }
                }
            }

            if(markForAdd)
            {
                PossibleMoves.Add(board.Nodes[x, y]);
                if ((iterationX < stepsX || stepsX == 0) && (iterationY < stepsY || stepsY == 0))
                {
                    CheckAndSetPossibleMoves(direction, stepsX, stepsY, inBetweenSteps, ++iterationX, ++iterationY);
                }
            }
        }

    }
}
