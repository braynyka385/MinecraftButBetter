using MinecraftButBetter.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.Rendering
{
    class Camera
    {
        PointD3 loc;
        int renderDistance;
        int hFOV;
        int vFOV;
        public double headingX, headingY;
        public Camera(double x, double y, double z, int renderDistance, int hFOV, int vFOV, double headingX, double headingY)
        {
            this.loc = new PointD3(x,y,z);
            this.renderDistance = renderDistance;
            this.hFOV = hFOV;
            this.vFOV = vFOV;
            this.headingX = headingX;
            this.headingY = headingY;
        }

        public void move(PointD3 by)
        {
            double c = Math.Cos(toRadians(headingX));
            double s = Math.Sin(toRadians(headingX));
            PointD3 rotatedBy = new PointD3(0,0,0);
            rotatedBy.X = (c * by.X) + (s * by.Z);
            rotatedBy.Z = -(c * by.Z) + (s * by.X);
            rotatedBy.Y = by.Y;
            loc.add(rotatedBy);
        }
        public void rotate(int toX, int toY, int w, int h)
        {
            double cHXSP = map(headingX, 0, 360, 0, w);
            double cHYSP = map(headingY, -90, 90, 0, h);
            headingX = map(toX, 0, w, 360, 0);
            headingY = map(toY, 0, h, 0, 180);
            
        }
        public PointF pointToScreen(PointD3 point)
        {
            PointD3 delta = loc.deltaNoAbs(point);

            double headingXRad = toRadians(headingX);
            double headingYRad = toRadians(headingY);


            double XZDistFromCam = (Math.Cos(headingXRad) * delta.Z) - (Math.Sin(headingXRad) * delta.X);
            double XZOffset = ((Math.Cos(headingXRad) * delta.X) + (Math.Sin(headingXRad) * delta.Z));

            double YDistFromCam = (Math.Cos(headingYRad) * XZDistFromCam) - (Math.Sin(headingYRad) * delta.Y);
            double YOffset = (Math.Cos(headingYRad) * delta.Y) + (Math.Sin(headingYRad) * XZDistFromCam);

            XZDistFromCam -= (Math.Sin(headingYRad) * delta.Y); //Comment out if there's some weirdness with the size of blocks depending on y-axis
            double x = -1;
            double y = -1;
 
            x = map(XZOffset, 0, XZDistFromCam, 0.5, 1);
            y = map(YOffset, 0, YDistFromCam, 0.5, 1);

            // y = 0;
            if (x > 1 || y > 1 || x < 0|| y < 0 || YDistFromCam < 0)
            {
                return new PointF(-1, -1);
            }
             return new PointF((float)x, (float)y);
        }



        private double toRadians(double deg)
        {
            return deg * 0.01745329;
        }
        private double toDegrees(double rad)
        {
            return rad * 57.2957795;
        }
        private double map(double s, double a1, double a2, double b1, double b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }
}
