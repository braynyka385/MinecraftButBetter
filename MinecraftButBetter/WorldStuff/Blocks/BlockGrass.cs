using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff.Blocks
{
      class BlockGrass : Block
      {

            public BlockGrass(int x, int y, int z) : base(x, y, z, BlockType.GRASS)
            {
                faceColors[0] = Color.Green;
                faceColors[1] = Color.SaddleBrown;
                faceColors[2] = Color.SaddleBrown;
                faceColors[3] = Color.SaddleBrown;
                faceColors[4] = Color.SaddleBrown;
                faceColors[5] = Color.SaddleBrown;
                generateBrushes();

        }
    }
}
