using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ludum_dare_24
{
    public class Enemy
    {
        public Vector2 Position;
        public Vector2 _moveto;
        public double Rotation;
        public double health;
        public float speed;
        public float currentspeed = 0.1f;  
        public float acceleration;
        public int type;
        public int Nr;
        public int delay_fight = 120;

        public Enemy()
        {
            if (Game1.HASWON == false)
            {
                Nr = EnemyHandling.Enemylist.Count - 1;
                Random rand = new Random();
            again:
                type = rand.Next(0, EnemyHandling.maxtype);
                if (rand.Next(0, 100) < EnemyHandling.rarety[type])
                    goto again;
                health = EnemyHandling.enemyhealth[type];
                speed = EnemyHandling.EnemySpeeds[type];
                acceleration = EnemyHandling.enemyaccelerations[type];
                Position = new Vector2(0, 0);
                int rand1 = rand.Next(0, 4);
                if (rand1 == 0)
                {
                    Position.X = -50;
                    Position.Y = rand.Next(0, (int)Game1.GameResolution.Y);
                    goto skip;
                }
                if (rand1 == 1)
                {
                    Position.X = (int)Game1.GameResolution.X + 50;
                    Position.Y = rand.Next(0, (int)Game1.GameResolution.Y);
                    goto skip;
                }
                if (rand1 == 2)
                {
                    Position.X = rand.Next(0, (int)Game1.GameResolution.X);
                    Position.Y = -50;
                    goto skip;
                }
                if (rand1 == 3)
                {
                    Position.X = rand.Next(0, (int)Game1.GameResolution.X);
                    Position.Y = (int)Game1.GameResolution.Y + 50;
                    goto skip;
                }
            skip:
                _moveto = new Vector2(rand.Next(0, (int)Game1.GameResolution.X), rand.Next(0, (int)Game1.GameResolution.Y));
            }
        }

        public void Update()
        {
            if (delay_fight > 0)
                delay_fight--;
            if (Movement.getlength(Position, _moveto) < 4)
            {
                Random rand = new Random();
                _moveto = new Vector2(rand.Next(0, (int)Game1.GameResolution.X), rand.Next(0, (int)Game1.GameResolution.Y));
            }
            double wannaberotation = 0.0f;

            wannaberotation = Movement.GetRotation(Position, _moveto);

            if (Rotation > 180 && (Rotation - 180) > wannaberotation)
            {
                Rotation += 4; goto skiprot;
            }
            if (Rotation < 180 && (Rotation + 180) < wannaberotation)
            {
                Rotation -= 4; goto skiprot;
            }
            if (Rotation < wannaberotation)
                Rotation += 4;
            if (Rotation > wannaberotation)
                Rotation -= 4;
        skiprot:
            if (Rotation > 360)
                Rotation -= 360;
            if (Rotation < 0)
                Rotation += 360;
            for (float a = 0; a < currentspeed; a += 0.1f)
            {
                Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Rotation - 180)));
                if (currentspeed < speed)
                    currentspeed += acceleration;
            }
            if (currentspeed > 0)
                currentspeed -= acceleration / 2;



        }

    }
}
