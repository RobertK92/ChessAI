using System;

namespace ChessAI
{
    [Flags]
    public enum MoveDirection 
    {
        Up      = 1 << 0,
        Down    = 1 << 1,
        Left    = 1 << 2,
        Right   = 1 << 3
    }
}
