using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpUtils;

namespace ChessAI
{
    public class GameManager : BaseObject
    {
        public Board Board                          { get; private set; }
        public MoveManager MoveManager              { get; private set; }
        public ControllingUnit CurrentController    { get; private set; }
        public TimeSpan MatchTime                   { get; private set; }
        public TimeSpan TurnTime                    { get; private set; }

        public Dictionary<ControllingUnit, int> TurnCount { get; private set; }

        protected override void Initialize()
        {
            MatchTime = TimeSpan.Zero;
            TurnTime = TimeSpan.Zero;
            Board = new Board();
            TurnCount = new Dictionary<ControllingUnit, int>();
            TurnCount.Add(ControllingUnit.Human, 0);
            TurnCount.Add(ControllingUnit.AI, 0);

            MoveManager = new MoveManager(Board);
            ChangeTurn(ControllingUnit.Human);
        }

        protected override void Update(GameTime gameTime)
        {
            MatchTime += gameTime.ElapsedGameTime;
            TurnTime += gameTime.ElapsedGameTime;
        }

        public void ChangeTurn(ControllingUnit controllingUnit)
        {
            if(CurrentController == controllingUnit)
            {
                Log.Warning(string.Format("'{0}' is already the current controller", controllingUnit));
                return;
            }

            TurnCount[controllingUnit]++;
            MoveManager.OnTurnEnd(CurrentController);
            CurrentController = controllingUnit;
            TurnTime = TimeSpan.Zero;
            MoveManager.OnTurnBegin(controllingUnit);
        }


    }
}
