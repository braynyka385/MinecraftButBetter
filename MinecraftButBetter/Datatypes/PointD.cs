using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.Datatypes
{
    class PointD3
    {
        public double X, Y, Z;
        public PointD3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public PointD3(int[] pos)
        {
            X = pos[0];
            Y = pos[1];
            Z = pos[2];
        }

        public void add(PointD3 point)
        {
            X += point.X;
            Y += point.Y;
            Z += point.Z;
        }
        public void subtract(PointD3 point)
        {
            X -= point.X;
            Y -= point.Y;
            Z -= point.Z;
        }
        public void multiply(PointD3 point)
        {
            X *= point.X;
            Y *= point.Y;
            Z *= point.Z;
        }
        public void divide(PointD3 point)
        {
            X /= point.X;
            Y /= point.Y;
            Z /= point.Z;
        }
        public bool equals(PointD3 point)
        {
            if(point.X == X && point.Y ==  Y && point.Z == Z) return true;
            return false;
        }
        public PointD3 delta(PointD3 other)
        {
            return new PointD3(Math.Abs(X - other.X), Math.Abs(Y - other.Y), Math.Abs(Z - other.Z));
        }
        public PointD3 deltaNoAbs(PointD3 other)
        {
            return new PointD3((X - other.X), (Y - other.Y), (Z - other.Z));
        }
        public double distance(PointD3 other)
        {
            return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z));
        }
    }
}
