﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockadeSimulatorCollisionMechanics
{
    public class Collision
    {
        public CollisionPhase CollisionPhase { get; set; }
        public Ship WinningShip { get; set; }
    }
}
