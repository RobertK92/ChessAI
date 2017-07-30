using MonoGameToolkit;
using SharpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessAI
{
    public class MoveManager : BaseObject
    {
        public readonly Board Board;
        
        public List<Piece> MyPieces { get; protected set; }
        public bool CanMove { get; set; }

        private GameManager gameManager;
        private ChessMouse mouse;

        public MoveManager(Board board) : base()
        { 
            this.Board = board;
            mouse = new ChessMouse();
            
        }

        protected override void Initialize()
        {
            gameManager = Scene.GetObject<GameManager>();   
        }

        public void OnTurnBegin(ControllingUnit controllingUnit)
        {
            Log.Message(string.Format("{0}'s turn", controllingUnit));

            if(controllingUnit == ControllingUnit.AI)
            {

            }
            else if(controllingUnit == ControllingUnit.Human)
            {
                mouse.Enabled = true;
                 
            }
        }

        public void OnTurnEnd(ControllingUnit controllingUnit)
        {
            if(controllingUnit == ControllingUnit.Human)
            {
                mouse.Enabled = false;
            }
        }

        private void UpdateHuman()
        {
            if(Keyboard.GetState().IsKeyPressedOnce(Keys.F))
            {
                Board.PieceDict[ControllingUnit.Human].First(x => x.PieceType == PieceType.Pawn).Move(
                    Board.Nodes[4, 4]);
            }
        }

        private void UpdateAI()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameManager.CurrentController)
            {
                case ControllingUnit.None:
                    break;
                case ControllingUnit.Human:
                    UpdateHuman();
                    break;
                case ControllingUnit.AI:
                    UpdateAI();
                    break;
                default:
                    break;
            }
        }
    }
}
