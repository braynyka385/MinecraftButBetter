using MinecraftButBetter.WorldStuff.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftButBetter.WorldStuff.Structures
{
    internal class StructureTree : Structure
    {
        public StructureTree(int x, int y, int z) : base(x, y, z)
        {
            for(int i = 0; i < 6; i++)
            {
                this.blocks.Add(new BlockLog(x, y + i, z));
            }

            for(int xa = -2; xa < 3; xa++)
            {
                for (int ya = 1; ya < 7; ya++)
                {
                    for (int za = -2; za < 3; za++)
                    {
                        if(xa != za)
                        {
                            this.blocks.Add(new BlockLeaf(x+xa, y+ya, z+za));
                        }
                    }
                }
            }
        }
    }
}
