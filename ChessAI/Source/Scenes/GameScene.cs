using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    public class GameScene : Scene
    {
        public Board Board { get; private set; }

        public override void Load()
        {
            Board = new Board();    
        }
    }
}
