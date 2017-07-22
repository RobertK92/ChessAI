using MonoGameToolkit;
using System;

namespace ChessAI
{
    public abstract class Controller : BaseObject
    {
        public readonly Board Board;

        public bool CanMove { get; set; }

        public Action OnTurnBegin = delegate { };

        public Controller(Board board) : base()
        {
            this.Board = board;
        }
    }
}
