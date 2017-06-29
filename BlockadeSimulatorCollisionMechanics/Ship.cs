using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockadeSimulatorCollisionMechanics
{
    public class Ship:BoardObject
    {
        public FacingDirection FacingDirection { get; set; }
        public Move Moving { get; set; }
        public ShipSize Size { get; set; }
    }
}
