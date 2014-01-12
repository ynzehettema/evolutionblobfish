using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ludum_dare_24
{
    public class Projectile
    {
        public Vector2 Position;
        public Vector2 Target;
        public double Rotation;
        public int Type;

        public Projectile(Vector2 pos, Vector2 tar, int type) { Position = pos; Target = tar; Type = type; Rotation = Movement.GetRotation(Position, Target); }

        public void Update()
        {
            for ( int a = 0; a < 50; a++)
                Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Rotation - 180)));
        }

    }
}
