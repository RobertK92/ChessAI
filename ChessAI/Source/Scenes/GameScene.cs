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
        public GameManager GameManager { get; private set; }

        public override void Load()
        {
            GameManager = new GameManager();    
        }
    }
}
