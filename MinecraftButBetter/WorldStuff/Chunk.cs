using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff
{
    class Chunk
    {
        public static readonly UInt32 chunkSize = 8;
        private List<Block> blocks;
        int x, y;
        int xW, yW;
    }
}
