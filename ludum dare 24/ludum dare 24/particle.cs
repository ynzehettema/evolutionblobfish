using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ludum_dare_24
{
    public class particle
    {
        public Microsoft.Xna.Framework.Vector2 Position;
        public int time = 40;
        public int Type;
        public particle(Microsoft.Xna.Framework.Vector2 pos, int type) { Position = pos; Type = type;}
    }
}
