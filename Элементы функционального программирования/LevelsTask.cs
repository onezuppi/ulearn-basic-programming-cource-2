using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{
    public static class LevelsTask
    {
        private static readonly Physics StandardPhysics = new Physics();
        private static readonly Vector StandartTarget = new Vector(600, 200);
        private static readonly Rocket StandartRocket = new Rocket(new Vector(200, 500), Vector.Zero, Math.PI / 8);

        public static IEnumerable<Level> CreateLevels()
        {
            yield return GenerateLevel("Zero", (size, v) => Vector.Zero);
            yield return GenerateLevel("Heavy", (size, v) => new Vector(0, 0.9));
            yield return GenerateLevel("Up", (size, v) => new Vector(0, -300 / (size.Height - v.Y + 300)),
                new Vector(700, 500));
            yield return GenerateLevel("WhiteHole", GetWhiteHole);
            yield return GenerateLevel("BlackHole", GetBlackHole);
            yield return GenerateLevel("BlackAndWhite",
                (size, v) => (GetWhiteHole(size, v) + GetBlackHole(size, v)) / 2);
        }

        private static Vector GetWhiteHole(Size size, Vector v)
        {
            var whiteHole = v - StandartTarget;
            
            return 140 * whiteHole.Length * whiteHole.Normalize() / (whiteHole.Length * whiteHole.Length + 1);
        }

        private static Vector GetBlackHole(Size size, Vector v)
        {
            var blackHolePosition = (StandartTarget + StandartRocket.Location) / 2;
            var blackHole = blackHolePosition - v;

            return 300 * blackHole.Length * blackHole.Normalize() /
                   (blackHole.Length * blackHole.Length + 1);
        }

        private static Level GenerateLevel(string name, Gravity gravity, Vector target = null) =>
            new Level(name, StandartRocket, target ?? StandartTarget, gravity, StandardPhysics);
    }
}