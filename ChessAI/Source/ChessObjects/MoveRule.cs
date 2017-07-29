using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    public struct MoveRule
    {
        public List<Point> Points { get; set; }
        public MoveDirection Direction { get; set; }

        public MoveRule(MoveDirection direction, List<Point> points)
        {
            this.Direction = direction;
            this.Points = points;
        }
    }
}
