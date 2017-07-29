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

        public void MovePiece(Piece piece, int x, int y)
        {
            if (!MyPieces.Contains(piece))
            {
                Log.Error(string.Format("Piece '{0}' cannot be controlled by me", piece.Name));
                return;
            }

            Node node = piece.PossibleMoves.FirstOrDefault(p => p.Xindex == x && p.Yindex == y);
            if(node == null)
            {
                Log.Error(string.Format("Failed to move piece '{0}' to '{1}': invalid move", piece.Name, Node.GetNodeName(x, y)));
                return;
            }

            //TODO: lerp to node and set
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
