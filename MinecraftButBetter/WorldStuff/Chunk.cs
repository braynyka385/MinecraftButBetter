﻿using MinecraftButBetter.Datatypes;
using MinecraftButBetter.WorldStuff.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff
{
    class Chunk
    {
        public static int chunkSize;
        public List<Block> blocks = new List<Block>();
        public readonly int X, Z;
        public double distFromPlayerSq;
        public Chunk(List<Block> blocks, int x, int z)
        {
            this.blocks = blocks;
            this.X = x;
            this.Z = z;
        }
        public Chunk(GeneratorType generatorType, int x, int z)
        {
            X = x;
            Z = z;
            generateChunk(generatorType);

        }

        public void optimizeChunk()
        {
            foreach (Block b in blocks)
            {
                for (int i = 0; i < 6; i++)
                {
                    Face f = b.getFace((FaceIndex)i);

                    foreach (Block b2 in blocks)
                    {
                        PointD3 p = new PointD3(f.locOfImpedingBlock);
                        if (b2.points[0].equals(p))
                        {
                            b.setVisibility(f, false);
                        }
                    }
                }
            }
        }

        private void generateChunk(GeneratorType g)
        {
            Random random = new Random();

            switch (g)
            {
                case GeneratorType.Random:
                    for(int x = 0; x < chunkSize; x++)
                    {
                        for(int z = 0; z < chunkSize; z++)
                        {
                            int y = random.Next(-1, 1);
                            Block b;
                            if (y < 0)
                            {
                                b = new BlockStone(x + X, y, z + Z);

                            }
                            else
                            {
                                b = new BlockGrass(x + X, y, z + Z);

                            }

                            blocks.Add(b);
                        }
                    }
                    break;
            }
        }
    }

    enum GeneratorType
    {
        Random,
        Perlin
    }
}
