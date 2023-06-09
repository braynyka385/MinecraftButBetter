﻿using MinecraftButBetter.Datatypes;
using MinecraftButBetter.WorldStuff.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexNoise;
using MinecraftButBetter.WorldStuff.Structures;

namespace MinecraftButBetter.WorldStuff
{
    class Chunk
    {
        Random random = new Random();
        public static int chunkSize;
        public List<Block> blocks = new List<Block>();
        public List<Structure> structures = new List<Structure>();
        public readonly int X, Z;
        public double distFromPlayerSq;
        public bool isModified = false;
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
        public bool contains(PointD3 point)
        {
            double bX = point.X;
            double bZ = point.Z;

            if (bX >= X && bZ >= Z && bX < X + chunkSize && bZ < Z + chunkSize)
            {
                return true;
            }
            return false;
        }
        public void optimizeChunk()
        {
            foreach (Block b in blocks)
            {
                for (int i = 0; i < 6; i++)
                {
                    Face f = b.getFace((FaceIndex)i);
                    b.setVisibility(f, true);

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
                case GeneratorType.Flat:
                    for (int x = 0; x < chunkSize; x++)
                    {
                        for(int y =  0; y < 3; y++)
                        {
                            for (int z = 0; z < chunkSize; z++)
                            {
                                Block b;
                                if(y == 2)
                                {
                                    b = new BlockGrass(x + X, y, z + Z);

                                }
                                else
                                {
                                    b = new BlockStone(x + X, y, z + Z);
                                }
                                blocks.Add(b);
                            }
                        }
                        
                    }
                    break;
                case GeneratorType.Simplex:
                    for (int x = 0; x < chunkSize; x++)
                    {
                        for (int z = 0; z < chunkSize; z++)
                        {
                            int f = (int)Math.Floor(Noise.CalcPixel2D(x +X,z+Z, 0.05f) / 24);
                            f++;
                            for (int y = 0; y < f; y++)
                            {
                                Block b;
                                if (y == f - 1)
                                {
                                    b = new BlockGrass(x + X, y, z + Z);

                                }
                                else
                                {
                                    b = new BlockStone(x + X, y, z + Z);
                                }
                                blocks.Add(b);

                                if(y == f-1 && Random.Shared.Next(0,100) == 5)
                                {
                                    Structure tree = new StructureTree(x + X, y + 1, z + Z);
                                    structures.Add(tree);
                                }
                            }
                            
                        }
                        

                    }
                    break;
            }
            foreach (Structure structure in structures)
            {
                foreach (Block b in structure.GetBlocks())
                {
                    if (this.contains(b.points[0]))
                    {
                        blocks.Add(b);
                    }
                    else
                    {
                        //World.world.addBlock(b);
                        blocks.Add(b); //TODO: Make this not suck
                        //Blocks for structures will currently be in the wrong chunk if they are generated near the chunk border.

                    }
                }
            }
        }
    }

    enum GeneratorType
    {
        Random,
        Simplex,
        Flat
    }
}
