using MinecraftButBetter.Rendering;
using MinecraftButBetter.WorldStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.Datatypes
{
     class Ray
     {
        public PointD3 start, end;
        double maxDistance;
        double maxDistSq;
        Camera player;
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
        public Ray(double maxDist, Camera playerCam)
        {
            maxDistance = maxDist;
            maxDistSq = maxDist * maxDist;
            this.player = playerCam;
        }

        public int indexOfIntersectedBlock(List<Block> blocks)
        {
            int index = -1;
            double bestDistSq = 1000000000;
            PointF midPoint = new PointF(0.5f,0.5f);
            PointF endPoint = new PointF(100f, 100f);
            for(int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].distSq < maxDistSq && blocks[i].distSq < bestDistSq)
                {
                    PointF[] pointsFs = new PointF[8];
                    for (int j = 0; j < 8; j++)
                    {
                        PointF p = player.pointToScreen(blocks[i].points[j]);
                        pointsFs[j] = p;
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        Face f = blocks[i].getFace((FaceIndex)j);
                        int[] corners = f.getCorners();
                        int intersections = 0;
                        if (f.isVisible && !f.obstructedByOwnBlock)
                        {
                            corners = f.getCorners();
                            if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[0]], pointsFs[corners[1]]))
                            {
                                intersections++;
                            }
                            if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[1]], pointsFs[corners[2]]))
                            {
                                intersections++;
                            }
                            if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[2]], pointsFs[corners[3]]))
                            {
                                intersections++;
                            }
                            if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[3]], pointsFs[corners[0]]))
                            {
                                intersections++;
                            }

                            if(intersections % 2 != 0)
                            {
                                index = i;
                                bestDistSq = blocks[i].distSq;
                            }
                        }
                    }
                }
            }
            return index;
        }
        public FaceIndex clickedFace(Block b)
        {
            PointF midPoint = new PointF(0.5f, 0.5f);
            PointF endPoint = new PointF(100f, 100f);
            PointF[] pointsFs = new PointF[8];
            for (int j = 0; j < 8; j++)
            {
                PointF p = player.pointToScreen(b.points[j]);
                pointsFs[j] = p;
            }
            for (int j = 0; j < 6; j++)
            {
                Face f = b.getFace((FaceIndex)j);
                int[] corners = f.getCorners();
                int intersections = 0;
                if (f.isVisible && !f.obstructedByOwnBlock) // So... if a face is off towards the edge of the screen, only some of the edges will be rendered (I think),
                                                            // so the evenodd test will fail... This is why sometimes blocks place perpendicular to the aiming dir. (I think) {FIX}
                {
                    corners = f.getCorners();
                    if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[0]], pointsFs[corners[1]]))
                    {
                        intersections++;
                    }
                    if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[1]], pointsFs[corners[2]]))
                    {
                        intersections++;
                    }
                    if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[2]], pointsFs[corners[3]]))
                    {
                        intersections++;
                    }
                    if (LineIntersectsLine(midPoint, endPoint, pointsFs[corners[3]], pointsFs[corners[0]]))
                    {
                        intersections++;
                    }

                    if (intersections % 2 != 0)
                    {
                        return (FaceIndex)j;
                    }
                }
            }
            return FaceIndex.TOP;
        }
        private double toRadians(double deg)
        {
            return deg * 0.01745329;
        }
        private bool LineIntersectsLine(PointF l1p1, PointF l1p2, PointF l2p1, PointF l2p2) // Thanks StackOverflow // BraydenThoughts#1: Programming-themed dating app called BabeOverflow
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }
    }
}
