﻿using System;
using System.Drawing;
using System.Linq;

namespace func_rocket
{
    public class ForcesTask
    {
        public static RocketForce GetThrustForce(double forceValue)
        {
            return r => new Vector(Math.Cos(r.Direction) * forceValue, Math.Sin(r.Direction) * forceValue);
        }

        public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
        {
            return r => gravity(spaceSize, r.Location);
        }

        public static RocketForce Sum(params RocketForce[] forces)
        {
            return r => forces.Aggregate(Vector.Zero, (sum, force) => sum + force(r));
        }
    }
}