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
                    if(draggingPiece == null)
                        draggingPiece = overlappingPiece;
                }
            }
            else if(Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if(draggingPiece != null)
                {
                    //TODO: Move piece using MoveManager
                }

                draggingPiece = null;
            }
        }
    }
}
