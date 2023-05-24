using MinecraftButBetter.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff
{
    class Block
    {
        private int x, y, z;
        public PointD3[] points = new PointD3[8];
        private BlockType type;
        private int myIndex = 0;

        public Block(int x, int y, int z)
        {
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
        }

        public PointD3 doublePos ()
        {
            return new PointD3 (x, y, z);
        }

        public Face getFace(FaceIndex side)
        {
            return new Face(side, this.type);
        }
        
    }

    class Face
    {
        public FaceIndex side;
        int[,] edges;
        Color color;
        double distSquared;
        public BlockType parentBlockType;

        public Face(FaceIndex _side, BlockType _parentBlockType)
        {
            side = _side;
            parentBlockType = _parentBlockType;
            edges = sideToEdges(side);
        }

        public int[,] getEdges()
        {
            return edges;
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
    }

    public enum FaceIndex : int
    {
        TOP = 0, BOTTOM = 1, LEFT = 2, RIGHT = 3, FRONT = 4, BACK = 5
    }
    public enum BlockType : int
    {
        GRASS = 0, STONE = 1
    }

    
}
