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
                double dSq = chunkDistance(playerPos, unloadedChunks[i]);
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
                    if(!chunkPositions.Contains(new int[] { x + roundedPlayerPos.X, z + roundedPlayerPos.Y }))
                    {
                        chunkPositions.Add(new int[] { x + roundedPlayerPos.X, z + roundedPlayerPos.Y });
                        Chunk c = new Chunk(GeneratorType.Random, x + roundedPlayerPos.X, z + roundedPlayerPos.Y);
                        c.optimizeChunk();
                        unloadedChunks.Add(c);
                    }
                }
            }

        }
        double chunkDistance(PointD3 to, Chunk c)
        {
            double distSq = (to.X - c.X) * (to.X - c.X) + (to.Z - c.Z) * (to.Z - c.Z);

            if(distSq < renderDistanceSquared)
            {
                return distSq;
            }
            return -1;
        }
    }
}
