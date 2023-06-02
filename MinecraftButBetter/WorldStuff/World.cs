using MinecraftButBetter.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff
{
    class World
    {
        List<Chunk> loadedChunks = new List<Chunk>();
        List<Chunk> unloadedChunks = new List<Chunk>();
        List<int[]> chunkPositions = new List<int[]>();
        int renderDistanceSquared;
        int renderDist;
        public World(int chunkSize, int renderDistance)
        {
            this.renderDistanceSquared = renderDistance * renderDistance;
            renderDist = renderDistance;
            Chunk.chunkSize = chunkSize;
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
                    unloadedChunks.Add(loadedChunks[i]);
                    loadedChunks.RemoveAt(i);
                }

            }
            loadedChunks = loadedChunks.OrderByDescending(b => b.distFromPlayerSq).ToList();

            return loadedChunks;
        }
        public void generateChunks(PointD3 player)
        {
            Point roundedPlayerPos = new Point((int)Math.Round(player.X / Chunk.chunkSize) * Chunk.chunkSize, (int)Math.Round(player.Z / Chunk.chunkSize) * Chunk.chunkSize);
            for(int x = -renderDist * Chunk.chunkSize; x <= renderDist * Chunk.chunkSize; x += Chunk.chunkSize)
            {
                for (int z = -renderDist * Chunk.chunkSize; z <= renderDist * Chunk.chunkSize; z += Chunk.chunkSize)
                {
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
                        Chunk c = new Chunk(GeneratorType.Flat, x + roundedPlayerPos.X, z + roundedPlayerPos.Y);
                        c.optimizeChunk();
                        unloadedChunks.Add(c);
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
                    return;
                }
            }

            throw new Exception();
        }
        double chunkDistance(PointD3 to, Chunk c)
        {
            int chunkSize = Chunk.chunkSize;
            double distSq = (to.X - c.X) * (to.X - c.X) + (to.Z - c.Z) * (to.Z - c.Z);
            double d2 = (to.X - (c.X + chunkSize)) * (to.X - (c.X + chunkSize)) + (to.Z - c.Z) * (to.Z - c.Z);
            double d3 = (to.X - c.X) * (to.X - c.X) + (to.Z - (c.Z + chunkSize)) * (to.Z - (c.Z + chunkSize));
            double d4 = (to.X - (c.X + chunkSize)) * (to.X - (c.X + chunkSize)) + (to.Z - (c.Z + chunkSize)) * (to.Z - (c.Z + chunkSize));
            //if(d2 < distSq)
            //{
            //    distSq = d2;
            //}
            //if(d3 < distSq)
            //{
            //    distSq = d3;
            //}
            //if(d4 < distSq)
            //{
            //    distSq = d4;
            //}

            double finalDistTimesFour = (distSq + d2 + d3 + d4);
            if (distSq < renderDistanceSquared * Chunk.chunkSize)
            {
                return finalDistTimesFour;
            }
            return -1;
        }
    }
}
