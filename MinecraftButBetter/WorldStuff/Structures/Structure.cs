using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff.Structures
{
    internal class Structure //TODO: make structures "stay" when chunks are ungenerated and regenerated
    {
        protected List<Block> blocks = new List<Block>();
        protected int x, y, z;
        public Structure(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public List<Block> GetBlocks()
        {
            return blocks;
        }
    }
}
