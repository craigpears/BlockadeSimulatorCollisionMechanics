using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockadeSimulatorCollisionMechanics
{
    public class CollisionResults
    {
        public CollisionResults()
        {
            Collisions = new List<Collision>();
        }

        public List<Collision> Collisions { get; set; }
        public Board ResultingBoard { get; set; }
    }
}
