using MinecraftButBetter.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexNoise;
using System.ComponentModel;

namespace MinecraftButBetter.WorldStuff
{
    class World
    {
        public static World world;
        List<Chunk> loadedChunks = new List<Chunk>();
        List<Chunk> unloadedChunks = new List<Chunk>();
        List<int[]> chunkPositions = new List<int[]>();
        int renderDistanceSquared;
        int renderDist;
        public World(int chunkSize, int renderDistance, int seed)
        {
            Noise.Seed = seed;
            this.renderDistanceSquared = renderDistance * renderDistance;
            renderDist = renderDistance;
            Chunk.chunkSize = chunkSize;
            world = this;
        }
        public List<Chunk> getLoadedChunks() { return loadedChunks; }
        public List<Chunk> loadChunks(PointD3 playerPos)
        {
            for(int i = unloadedChunks.Count - 1; i >= 0; i--)
            {
                double dSq = chunkDistance(playerPos, unloadedChunks[i]);//
                unloadedChunks[i].distFromPlayerSq = dSq;
                if (dSq > 0)
                {
                    loadedChunks.Add(unloadedChunks[i]);
                    unloadedChunks.RemoveAt(i);
                }

            }
            for (int i = loadedChunks.Count - 1; i >= 0; i--)
            {
                double dSq = chunkDistance(playerPos, loadedChunks[i]);
                if (loadedChunks[i].contains(playerPos))
                {
                    dSq = 0;
                }
                loadedChunks[i].distFromPlayerSq = dSq;
                if (dSq < 0)
                {
                    if (loadedChunks[i].isModified) //temp override  || true==true
                    {
                        unloadedChunks.Add(loadedChunks[i]);

                    }

                    else
                    {
                        int[] pos = new int[]
                        {
                            loadedChunks[i].X,  loadedChunks[i].Z
                        };
                        for (int cp = chunkPositions.Count - 1; cp >= 0; cp--)
                        {
                            if (chunkPositions[cp][0] == pos[0] && chunkPositions[cp][1] == pos[1])
                            {
                                chunkPositions.RemoveAt(cp);
                            }
                        }

                    }
                    loadedChunks.RemoveAt(i);
                }

            }
            loadedChunks = loadedChunks.OrderByDescending(b => b.distFromPlayerSq).ToList();

            return loadedChunks;
        }
        public void generateChunks(PointD3 player)
        {
            const int mult = 3;
            Point roundedPlayerPos = new Point((int)Math.Round(player.X / Chunk.chunkSize) * Chunk.chunkSize, (int)Math.Round(player.Z / Chunk.chunkSize) * Chunk.chunkSize);
            for(int x = -renderDist * Chunk.chunkSize + Chunk.chunkSize* mult; x <= renderDist * Chunk.chunkSize - Chunk.chunkSize * mult; x += Chunk.chunkSize)
            {
                for (int z = -renderDist * Chunk.chunkSize + Chunk.chunkSize * mult; z <= renderDist * Chunk.chunkSize - Chunk.chunkSize * mult; z += Chunk.chunkSize)
                {
                    //double dSq = chunkDistance(playerPos, new Chunk(GeneratorType.Flat, x + roundedPlayerPos.X, z + roundedPlayerPos.Y));//
                    //unloadedChunks[i].distFromPlayerSq = dSq;
                    //if (dSq > 0)
                    //{
                    //    loadedChunks.Add(unloadedChunks[i]);
                    //    unloadedChunks.RemoveAt(i);
                    //}

                    //TODO: make chunk gen work with the same distance function as the loader, rather than being a square (or vice-versa)
                    int[] pos = new int[]
                    {
                         x + roundedPlayerPos.X, z + roundedPlayerPos.Y
                    };
                    bool contains = false;
                    foreach (int[] array in chunkPositions)
                    {
                        if (array[0] == pos[0] && array[1] == pos[1])
                        {
                            contains = true;
                        }
                    }
                    if (!contains)
                    {
                        int cg = 0;
                        chunkPositions.Add(new int[] { x + roundedPlayerPos.X, z + roundedPlayerPos.Y });
                        Chunk c = new Chunk(GeneratorType.Simplex, x + roundedPlayerPos.X, z + roundedPlayerPos.Y);
                        c.optimizeChunk();
                        loadedChunks.Add(c);
                    }
                }
            }
        }
        public void optimizeChunks()
        {
            foreach (Chunk c in loadedChunks)
            {
                c.optimizeChunk();
            }
        }
        public void removeBlock(int chunkIndex, int blockIndex)
        {
            loadedChunks[chunkIndex].blocks.RemoveAt(blockIndex);
            loadedChunks[chunkIndex].isModified = true;
            loadedChunks[chunkIndex].optimizeChunk();
        }
        public void addBlock(Block block)
        {
            int chunkSize = Chunk.chunkSize;
            
            int bX = (int)block.points[0].X;
            int bZ = (int)block.points[0].Z;

            for (int i = 0; i <  loadedChunks.Count; i++) 
            {
                int cX = loadedChunks[i].X;
                int cZ = loadedChunks[i].Z;
                if (loadedChunks[i].contains(block.points[0]))
                {
                    loadedChunks[i].blocks.Add(block);
                    loadedChunks[i].isModified = true;
                    return;
                }
            }

           // throw new Exception();
        }
        double chunkDistance(PointD3 to, Chunk c)
        {
            int chunkSize = Chunk.chunkSize;
            double distSq = (to.X - c.X) * (to.X - c.X) + (to.Z - c.Z) * (to.Z - c.Z);
            double d2 = (to.X - (c.X + chunkSize)) * (to.X - (c.X + chunkSize)) + (to.Z - c.Z) * (to.Z - c.Z);
            double d3 = (to.X - c.X) * (to.X - c.X) + (to.Z - (c.Z + chunkSize)) * (to.Z - (c.Z + chunkSize));
            double d4 = (to.X - (c.X + chunkSize)) * (to.X - (c.X + chunkSize)) + (to.Z - (c.Z + chunkSize)) * (to.Z - (c.Z + chunkSize));


            double finalDistTimesFour = (distSq + d2 + d3 + d4);
            if (distSq < renderDistanceSquared * Chunk.chunkSize*Chunk.chunkSize)
            {
                return finalDistTimesFour;
            }
            return -1;
        }
    }
}
