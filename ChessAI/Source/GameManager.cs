using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ChessAI
{
    public class GameManager : BaseObject
    {
        public Board Board                          { get; private set; }
        public PlayerController PlayerController    { get; private set; }
        public AIController AIController            { get; private set; }
        public TimeSpan MatchTime                   { get; private set; }
        
        public GameManager() : base()
        {
            MatchTime = TimeSpan.Zero;
            Board = new Board();
            PlayerController = new PlayerController(Board);
            AIController = new AIController(Board);
        }

        protected override void Update(GameTime gameTime)
        {
            MatchTime += gameTime.ElapsedGameTime;
            
        }

        public void SetControllingUnit(ControllingUnit controllingUnit)
        {
            switch (controllingUnit)
            {
                case ControllingUnit.Human:
                    AIController.CanMove = false;
                    PlayerController.CanMove = true;
                    PlayerController.OnTurnBegin();
                    break;
                case ControllingUnit.AI:
                    PlayerController.CanMove = false;
                    AIController.CanMove = true;
                    AIController.OnTurnBegin();
                    break;
                default:
                    throw new ArgumentException("Invalid controlling unit");
            }
        }
    }
}
