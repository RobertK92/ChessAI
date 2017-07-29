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
        
        public List<Point> MoveRules { get; private set; }
        public List<Point> CaptureRules { get; private set; }
        public List<Node> PossibleMoves { get; private set; }
        public bool HasEverMoved { get; private set; }

        public Node Node { get; set; }

        private Vector2 previousNodePosition;
        private float targetAlpha;

        public Piece(PieceType pieceType, ControllingUnit controllingUnit, Node startNode) : base()
        {
            this.StartNode = startNode;
            this.ControllingUnit = controllingUnit;
            this.PieceType = pieceType;
            this.Node = startNode;
        }

        protected override void Initialize()
        {
            DrawOrder = (int)DrawLayer.Piece;
            ChangeTexture(PieceType.GetTextureName(ControllingUnit), true, true);
            Position = StartNode.Position;
            EnablePhysicsRectangle(FarseerPhysics.Dynamics.BodyType.Dynamic, Bounds);
            PhysicsBody.GravityScale = 0.0f;
            PhysicsBody.IsSensor = true;
            UpdateAllRules();
        }

        protected override void Update(GameTime gameTime)
        {
            if(Position != Node.Position)
            {
                targetAlpha += Chess.Instance.DeltaTime * 2.0f;
                targetAlpha = MathHelper.Clamp(targetAlpha, 0, 1);
                Position = Vector2.Lerp(previousNodePosition, Node.Position, targetAlpha);
            }
        }

        public void Move(Node node)
        {
            previousNodePosition = Node.Position;
            this.Node = node;
            HasEverMoved = true;
            targetAlpha = 0.0f;
        }

        public void UpdateAllRules()
        {
            Board board = Scene.GetObject<Board>();
            if (board == null)
            {
                Log.Error("No board found");
                return;
            }

            UpdateMoveRules();
            UpdateCaptureRules();
        }

        public void UpdatePossibleMoves()
        {
            
        }

        private void UpdateCaptureRules()
        {
            CaptureRules = MoveRules;
            if(PieceType == PieceType.Pawn)
            {
                CaptureRules = new List<Point>()
                {
                    new Point( 1, 1),
                    new Point(-1, 1),
                };
            }
        }

        private void UpdateMoveRules()
        {
            MoveRules.Clear();
            switch (PieceType)
            {
                case PieceType.Pawn:
                    {
                        MoveRules.Add(new MoveRule()
                        {
                            Direction = MoveDirection.Up,
                            Points = new List<Point>()
                                {
                                    new Point(0, 1)
                                }
                        });

                        if (!HasEverMoved)
                        {
                            MoveRules.Add(new MoveRule()
                            {
                                Direction = MoveDirection.Up,
                                Points = new List<Point>()
                                {
                                    new Point(0, 2)
                                }
                            });
                        }
                        break;
                    }
                case PieceType.Knight:
                    MoveRules = new List<Point>()
                        {
                            new Point( 1,  2),
                            new Point( 2,  1),
                            new Point( 2, -1),
                            new Point( 1, -2),
                            new Point(-1, -2),
                            new Point(-2, -1),
                            new Point(-2,  1),
                            new Point(-1,  2),
                        };


                    break;
                case PieceType.Bishop:
                    MoveRules.Clear();
                    for (int i = 1; i < 8; i++)
                    {
                        MoveRules.Add(new Point( i,  i));
                        MoveRules.Add(new Point(-i, -i));
                        MoveRules.Add(new Point(-i,  i));
                        MoveRules.Add(new Point( i, -i));
                    }
                    break;
                case PieceType.Rook:
                    MoveRules.Clear();
                    for (int i = 1; i < 8; i++)
                    {
                        MoveRules.Add(new Point(i, 0));
                        MoveRules.Add(new Point(0, i));
                    }
                    break;
                case PieceType.Queen:
                    MoveRules.Clear();
                    for (int i = 1; i < 8; i++)
                    {
                        MoveRules.Add(new Point( i,  i));
                        MoveRules.Add(new Point(-i, -i));
                        MoveRules.Add(new Point(-i,  i));
                        MoveRules.Add(new Point( i, -i));
                        MoveRules.Add(new Point( i,  0));
                        MoveRules.Add(new Point( 0,  i));
                    }
                    break;
                case PieceType.King:
                    MoveRules.Add(new MoveRule()
                    {
                        Direction = MoveDirection.Right | MoveDirection.Down,
                        Points = new List<Point>()
                        {
                            new Point( 1,  1)
                        }
                    });

                    MoveRules.Add(new MoveRule()
                    {
                        Direction = MoveDirection.Up | MoveDirection.Left,
                        Points = new List<Point>()
                        {
                            new Point(-1, -1)
                        }
                    });

                    MoveRules.Add(new MoveRule()
                    {
                        Direction = MoveDirection.Left | MoveDirection.Down,
                        Points = new List<Point>()
                        {
                            new Point(-1,  1)
                        }
                    });

                    MoveRules = new List<MoveRule>() new List<Point>()
                    {
                        
                        
                        
                        new Point( 1, -1),
                        new Point( 1,  0),
                        new Point( 0,  1)
                    };
                    break;
                default:
                    break;
            }
        }
    }
}
