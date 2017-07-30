using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameToolkit;
using SharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    public class ChessMouse : BaseObject
    {
        private GameManager gameManager;
        private Piece overlappingPiece;
        private Piece draggingPiece;

        public ChessMouse() : base()
        {
            EnablePhysicsCircle(FarseerPhysics.Dynamics.BodyType.Dynamic, 4.0f);
            PhysicsBody.IsSensor = true;
            PhysicsBody.GravityScale = 0.0f;
            
        }

        protected override void Initialize()
        {
            gameManager = Scene.GetObject<GameManager>();
        }

        protected override void OnContactSensor(ContactInfo contact)
        {
            Piece piece = (Piece)contact.Obj;
            if (piece == null)
                return;

            if(piece.ControllingUnit == ControllingUnit.Human && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                overlappingPiece = piece;
            }
        }

        protected override void OnSeperateSensor(ContactInfo contact)
        {
            if(overlappingPiece == (Piece)contact.Obj)
            {
                overlappingPiece = null;
            }
        }

        protected override void FixedUpdate()
        {
            PhysicsBody.Position = Mouse.GetState().Position.ToVector2() * Physics.UPP;
            PhysicsBody.Awake = true;
            
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(overlappingPiece != null)
                {
                    if (draggingPiece == null)
                    {
                        draggingPiece = overlappingPiece;
                        draggingPiece.IsDragging = true;
                        foreach (Node move in draggingPiece.PossibleMoves)
                        {
                            move.SetHighlightEnabled(true);
                        }
                    }
                }
            }
            else if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if(draggingPiece != null)
                {
                    draggingPiece.IsDragging = false;

                    foreach (Node move in draggingPiece.PossibleMoves)
                    {
                        move.SetHighlightEnabled(false);
                    }

                    Node overlappingNode = null;
                    Rectangle mouseRect = new Rectangle(Mouse.GetState().Position, new Point(2, 2));
                    foreach(Node node in Scene.GetObject<Board>().Nodes)
                    {
                        Rectangle nodeRect = new Rectangle(node.Position.ToPoint(), new Point(83, 83));
                        if(mouseRect.Intersects(nodeRect))
                        {
                            overlappingNode = node;
                            break;
                        }
                    }

                    if(overlappingNode != null)
                        draggingPiece.Move(overlappingNode);
                    draggingPiece = null;
                }
            }
        }
    }
}
