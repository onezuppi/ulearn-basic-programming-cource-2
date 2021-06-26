using System;

namespace func_rocket
{
    public static class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var currentAngle = (target - rocket.Location).Angle;
            var angle = currentAngle - rocket.Direction;
            if (Math.Abs(angle) * Math.Abs(currentAngle - rocket.Velocity.Angle) < 1)
                angle += currentAngle - rocket.Velocity.Angle;
            if (angle == 0)
                return Turn.None;
                
            return angle > 0 ? Turn.Right : Turn.Left;
        }
    }
}