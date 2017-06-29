using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockadeSimulatorCollisionMechanics
{
    public class Board
    {
        public Ship PlayerOneShip { get; set; }
        public Ship PlayerTwoShip { get; set; }
        public Ship PlayerThreeShip { get; set; }
        public Rock Rock { get; set; }
    }
}
