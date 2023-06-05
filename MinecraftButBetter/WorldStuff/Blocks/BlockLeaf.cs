﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff.Blocks
{
    class BlockLeaf : Block
    {
        public BlockLeaf(int x, int y, int z) : base(x, y, z, BlockType.GRASS)
        {
            faceColors[0] = Color.Gray;
            faceColors[1] = Color.Gray;
            faceColors[2] = Color.Gray;
            faceColors[3] = Color.Gray;
            faceColors[4] = Color.Gray;
            faceColors[5] = Color.Gray;

            faceTextures = new Color[6, 2, 2];
            for (int i = 0; i < 6; i++)
            {
                int v = Color.Gray.ToArgb();
                //faceTextures[i, 0, 0] = Color.Gray;
                //faceTextures[i, 1, 0] = Color.LightGray;
                //faceTextures[i, 0, 1] = Color.DarkGray;
                //faceTextures[i, 1, 1] = Color.Gainsboro;
                //SaddleBrown: 139,69,19
                faceTextures[i, 0, 0] = Color.FromArgb(250, 12, 95, 11);
                faceTextures[i, 1, 0] = Color.FromArgb(248, 15, 90, 11);
                faceTextures[i, 0, 1] = Color.FromArgb(252, 6, 100, 11);
                faceTextures[i, 1, 1] = Color.FromArgb(251, 7, 98, 11);
            }
            hasTexture = true;
            generateBrushes();
        }
    }
}
