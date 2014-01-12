using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ludum_dare_24
{
    public class Star
    {
        public Vector2 Position;
        public double Rotation;
        public double movementrotation;

        public int timeleftbeforecatch = 60;
        public Star(Vector2 pos)
        {
            Position = pos;
            Random rand = new Random();
            int a = rand.Next(0, 2);
            if (a == 0)
            {
                movementrotation = rand.Next(360 - 45, 359);
            }

            if (a == 1)
            {
                movementrotation = rand.Next(0, 45);
            }
            Game1.star.Play();
        }

        public void Update()
        {
            Game1.ParticleList.Add(new particle(Position, 1));
            if (timeleftbeforecatch > 0)
                timeleftbeforecatch--;
            if (movementrotation > 180)
                movementrotation--;
            if (movementrotation < 180)
                movementrotation++;
            for ( int a = 0; a < 50; a++ )
                Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)movementrotation - 180)));
            Rotation += 2;
            if (Rotation > 360)
                Rotation -= 360;
            if ( Position.X < 0 )
                movementrotation = 180 - ((movementrotation - 180));
            if (Position.X > Game1.GameResolution.X)
                movementrotation = 180 + ((180 - movementrotation));
        }

    }
}
