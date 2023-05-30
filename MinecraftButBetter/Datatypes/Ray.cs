using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.Datatypes
{
     class Ray
    {
        public PointD3 start, end;

        //public Ray(PointD3 pos, double x2, double y2, double z2)
        //{
        //    start = pos;
        //    end = new PointD3(x2, y2, z2);
        //}
        public Ray(PointD3 pos, double xHeading, double yHeading, double maxDist)
        {
            double rX = toRadians(xHeading);
            double rY = toRadians(yHeading);
            start = pos;
            PointD3 direction = new PointD3(Math.Sin(rX), Math.Sin(rY), -Math.Cos(rX));
            double scalar = direction.magnitude();

            direction.multiply(new PointD3(maxDist / scalar, maxDist / scalar, maxDist / scalar));
            end = direction;
            end.add(start);
            double b = 3;
        }

        private double toRadians(double deg)
        {
            return deg * 0.01745329;
        }
    }
}
