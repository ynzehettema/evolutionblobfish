using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ludum_dare_24
{
    class Player
    {
        public static Vector2 Position;
        public static double Rotation = 0;
        public static float speed = 0f;

        public static void Initialize()
        {
            Position = new Vector2(400, 300);
        }

    }
}
