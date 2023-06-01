using MinecraftButBetter.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff
{
    class Block
    {
        private int x, y, z;
        public PointD3[] points = new PointD3[8];
        private BlockType type;
        private Face[] faces;
        protected Color[] faceColors = new Color[6];
        protected Color[,,] faceTextures;
        public bool hasTexture = false;
        public SolidBrush[] faceBrush = new SolidBrush[6];
        public SolidBrush[,,] faceTextureBrush = new SolidBrush[6,2,2];
        public double distSq;
        public Block(int x, int y, int z, BlockType t)
        {
            this.type = t;
            this.x = x;
            this.y = y;
            this.z = z;
            points[0]=(new PointD3(x, y, z));
            points[1] = (new PointD3(1 + x, y, z));
            points[2] = (new PointD3(x, 1 + y, z));
            points[3] = (new PointD3(1 + x, 1 + y, z));
            points[4] = (new PointD3(x, y, 1 + z));
            points[5] = (new PointD3(1 + x, y, 1 + z));
            points[6] = (new PointD3(x, 1 + y, 1 + z));
            points[7] = (new PointD3(1 + x, 1 + y, 1 + z));

            faces = new Face[6];
            for(int i = 0; i < 6; i++)
            {
                faces[i] = new Face((FaceIndex)i, type, new int[] {x,y,z}, faceColors[i]);
            }
        }

        protected void generateBrushes()
        {
            if(!hasTexture)
            {
                for (int i = 0; i < 6; i++)
                {
                    faceBrush[i] = new SolidBrush(faceColors[i]);
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    for(int j = 0; j < 2; j++)
                    {
                        for(int k = 0; k < 2; k++)
                        {
                            faceTextureBrush[i,j,k] = new SolidBrush(faceTextures[i,j,k]);
                        }
                    }
                }
            }
        }
        public void setVisibility(Face f, bool val)
        {
            faces[(int)f.side].isVisible = val;
        }
        public PointD3 doublePos ()
        {
            return new PointD3 (x, y, z);
        }

        public Face getFace(FaceIndex side)
        {
            //return new Face(side, this.type);
            return faces[(int)side];
        }
        public void selfObstructFaces(PointD3 camera)
        {
            for(int i = 0; i < 6; i++)
            {
                faces[i].obstructedByOwnBlock = false;
            }
            int test = closestPointIndex(camera);
            //switch (test)
            //{
            //    case 0:
            //        faces[0].obstructedByOwnBlock = true;
            //        faces[2].obstructedByOwnBlock = true;
            //        faces[4].obstructedByOwnBlock = true;
            //        break;
            //    case 1:
            //        faces[0].obstructedByOwnBlock = true;
            //        faces[3].obstructedByOwnBlock = true;
            //        faces[4].obstructedByOwnBlock = true;
            //        break;
            //    case 2:
            //        faces[1].obstructedByOwnBlock = true;
            //        faces[2].obstructedByOwnBlock = true;
            //        faces[4].obstructedByOwnBlock = true;
            //        break;
            //    case 3:
            //        faces[1].obstructedByOwnBlock = true;
            //        faces[3].obstructedByOwnBlock = true;
            //        faces[4].obstructedByOwnBlock = true;
            //        break;
            //    case 4:
            //        faces[0].obstructedByOwnBlock = true;
            //        faces[2].obstructedByOwnBlock = true;
            //        faces[5].obstructedByOwnBlock = true;
            //        break;
            //    case 5:
            //        faces[0].obstructedByOwnBlock = true;
            //        faces[3].obstructedByOwnBlock = true;
            //        faces[5].obstructedByOwnBlock = true;
            //        break;
            //}
            switch (test)
            {
                case 0:
                    faces[0].obstructedByOwnBlock = true;
                    faces[2].obstructedByOwnBlock = true;
                    faces[4].obstructedByOwnBlock = true;
                    break;
                case 1:
                    faces[0].obstructedByOwnBlock = true;
                    faces[3].obstructedByOwnBlock = true;
                    faces[4].obstructedByOwnBlock = true;
                    break;
                case 2:
                    faces[1].obstructedByOwnBlock = true;
                    faces[2].obstructedByOwnBlock = true;
                    faces[4].obstructedByOwnBlock = true;
                    break;
                case 3:
                    faces[1].obstructedByOwnBlock = true;
                    faces[3].obstructedByOwnBlock = true;
                    faces[4].obstructedByOwnBlock = true;
                    break;
                case 4:
                    faces[0].obstructedByOwnBlock = true;
                    faces[2].obstructedByOwnBlock = true;
                    faces[5].obstructedByOwnBlock = true;
                    break;
                case 5:
                    faces[0].obstructedByOwnBlock = true;
                    faces[3].obstructedByOwnBlock = true;
                    faces[5].obstructedByOwnBlock = true;
                    break;
                case 6:
                    faces[1].obstructedByOwnBlock = true;
                    faces[2].obstructedByOwnBlock = true;
                    faces[5].obstructedByOwnBlock = true;
                    break;
                case 7:
                    faces[1].obstructedByOwnBlock = true;
                    faces[3].obstructedByOwnBlock = true;
                    faces[5].obstructedByOwnBlock = true;
                    break;
            }
        }
        private int closestPointIndex(PointD3 to)
        {
            int bestIndex = 0;
            double bestDistance = 100000;
            for(int i = 0; i < 8; i++)
            {
                double dist = points[i].distanceSquared(to);
                if (dist  < bestDistance)
                {
                    bestIndex = i;
                    bestDistance = dist;
                }
            }
            return bestIndex;
        }
        
    }

    class Face
    {
        public bool obstructedByOwnBlock = false;
        public bool isVisible = true;
        public FaceIndex side;
        private FaceIndex opposingFace;
        int[,] edges;
        int[] corners;
        Color color;
        double distSquared;
        public BlockType parentBlockType;
       // int[] parentLoc;
        public int[] locOfImpedingBlock;

        public Face(FaceIndex _side, BlockType _parentBlockType, int[]pos)
        {
            side = _side;
            parentBlockType = _parentBlockType;
            edges = sideToEdges(side);
            corners = sideToCorners(side);
            opposingFace = getOpposingFace();
            locOfImpedingBlock = locationOfImpedingBlock(pos);


        }
        public Face(FaceIndex _side, BlockType _parentBlockType, int[] pos, Color c)
        {
            side = _side;
            parentBlockType = _parentBlockType;
            edges = sideToEdges(side);
            corners = sideToCorners(side);
            opposingFace = getOpposingFace();
            locOfImpedingBlock = locationOfImpedingBlock(pos);
            this.color = c;

        }

        private int[] locationOfImpedingBlock(int[] parentLoc)
        {
            int[] loc = parentLoc;
            switch (side)
            {
                case FaceIndex.TOP:
                    loc[1]++;
                    break;
                case FaceIndex.BOTTOM:
                    loc[1]--;
                    break;
                case FaceIndex.LEFT:
                    loc[0]++;
                    break;
                case FaceIndex.RIGHT:
                    loc[0]--;
                    break;
                case FaceIndex.FRONT:
                    loc[2]++;
                    break;
                case FaceIndex.BACK:
                    loc[2]--; //0123
                    break;
            }
           return loc;
        }
        private FaceIndex getOpposingFace()
        {
            switch(side)
            {
                case FaceIndex.TOP:
                    return FaceIndex.BOTTOM;
                case FaceIndex.BOTTOM:
                    return FaceIndex.TOP;
                case FaceIndex.LEFT:
                    return FaceIndex.RIGHT;
                case FaceIndex.RIGHT:
                    return FaceIndex.LEFT;
                case FaceIndex.FRONT:
                    return FaceIndex.BACK;
                case FaceIndex.BACK:
                    return FaceIndex.FRONT;
            }
            return FaceIndex.TOP;
        }
        public int[,] getEdges()
        {
            return edges;
        }
        public int[] getCorners()
        {
            return corners;
        }
        private int[,] sideToEdges(FaceIndex s)
        {
            int[,] lines = new int[4, 2];
            switch (s)
            {
                case FaceIndex.TOP:
                    lines = new int[,]{
                        {2,3},
                        { 2,6},
                        { 3,7},
                        { 6,7}
                    };
                    break;
                case FaceIndex.BOTTOM:
                    lines = new int[,]{
                        {0,1},
                        {0,4},
                        {1,5},
                        {4,5}
                    };
                    break;
                case FaceIndex.LEFT:
                    lines = new int[,]{
                        {1,3},
                        {1,5},
                        {3,7},
                        {5,7}
                    };
                    break;
                case FaceIndex.RIGHT:
                    lines = new int[,]{
                        {0,2},
                        {0,4},
                        {2,6},
                        {4,6}
                    };
                    break;
                case FaceIndex.FRONT:
                    lines = new int[,]{
                        {4,5},
                        {4,6},
                        {5,7},
                        {6,7}
                    };
                    break;
                case FaceIndex.BACK:
                    lines = new int[,]{
                        {0,1},
                        {0,2},
                        {1,3},
                        {2,3}
                    };
                    break;

            }
            return lines;
        }

        private int[] sideToCorners(FaceIndex s)
        {
            int[] corners = new int[4];
            switch (s)
            {
                case FaceIndex.TOP:
                    corners = new int[]{
                        3,2,6,7
                    };
                    break;
                case FaceIndex.BOTTOM:
                    corners = new int[]{
                        1,0,4,5
                    }; 
                    break;
                case FaceIndex.LEFT:
                    corners = new int[]{
                        3,1,5,7
                    };
                    break;
                case FaceIndex.RIGHT:
                    corners = new int[]{
                        2,0,4,6
                    };
                    break;
                case FaceIndex.FRONT:
                    corners = new int[]{
                        5,4,6,7
                    };
                    break;
                case FaceIndex.BACK:
                    corners = new int[]{
                        1,0,2,3
                    };
                    break;

            }
            return corners;
        }
    }

    public enum FaceIndex : int
    {
        TOP = 0, BOTTOM = 1, LEFT = 2, RIGHT = 3, FRONT = 4, BACK = 5, NONE = 401
    }
    public enum BlockType : int
    {
        GRASS = 0, STONE = 1
    }

    
}
