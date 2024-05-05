﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeometricModeling.Extensions
{
    public static class VectorExtension
    {
        public static Vector3 Rotate(this Vector3 start, double angle)
        {
            float newX = (float)(start.X * Math.Cos(angle) - start.Y * Math.Sin(angle));
            float newY = (float)(start.X * Math.Sin(angle) + start.Y * Math.Cos(angle));
            return new Vector3(newX, newY, start.Z);
        }
        public static double Angle(this Vector3 first) => Math.Atan2(first.Y, first.X);

        public static bool IsSamePoint(this Vector3 point, Vector3 otherPoint, double precision)
        {
            double diffX = point.X - otherPoint.X;
            double diffY = point.Y - otherPoint.Y;
            double diffZ = point.Z - otherPoint.Z;
            Func<double, bool> IsDiffAroundZero = (diff) => diff > -precision && diff < precision;
            return IsDiffAroundZero(diffX) && IsDiffAroundZero(diffY) && IsDiffAroundZero(diffZ);
        }
    }
}
