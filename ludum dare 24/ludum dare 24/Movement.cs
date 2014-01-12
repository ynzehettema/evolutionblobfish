using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ludum_dare_24
{
    class Movement
    {
        public static double GetRotation(Microsoft.Xna.Framework.Vector2 startpnt, Microsoft.Xna.Framework.Vector2 endpnt) // Partially written in dutch cause of harsh math :P
        {
            double rotation = 0;
            int[] start = { Convert.ToInt32(startpnt.X), Convert.ToInt32(startpnt.Y) };
            int[] end = { Convert.ToInt32(endpnt.X), Convert.ToInt32(endpnt.Y) };
            int[] Distance = new int[2];
            double tang = 0;
            int tang2 = 0;
            int quadrant = 0;
            if (start[0] == end[0] && start[1] > end[1]) // 0 or 360
            {
                Distance = new int[] { 0, start[1] - end[1] };
                tang2 = 0;
                tang = 0;
                rotation = 0;
                quadrant = 1;
            }
            if (start[0] < end[0] && start[1] > end[1]) // 1 - 89
            {
                double overstaandezijde = start[1] - end[1];
                double aanliggendezijde = end[0] - start[0];
                double result = 0;
                double radians = Math.Atan2(aanliggendezijde, overstaandezijde);
                double angle = radians * (180 / Math.PI);
                Distance = new int[] { end[0] - start[0], start[1] - end[1] };

                rotation = angle;
                if (rotation < 45)
                {
                    tang2 = 0;
                    tang = result;
                }
                if (rotation == 45)
                {
                    tang2 = 2;
                    tang = 1;
                }
                if (rotation > 45)
                {
                    tang2 = 1;
                    tang = result;
                }
                quadrant = 1;
            }
            if (start[0] < end[0] && start[1] == end[1]) // 90
            {
                rotation = 90;
                tang2 = 1;
                tang = 0;
                Distance = new int[] { end[0] - start[0], 0 };
                quadrant = 1;
            }
            if (start[0] < end[0] && start[1] < end[1]) // 91 - 179
            {
                double overstaandezijde = end[1] - start[1];
                double aanliggendezijde = end[0] - start[0];
                double result = overstaandezijde / aanliggendezijde;
                double radians = Math.Atan2(overstaandezijde, aanliggendezijde);
                double angle = radians * (180 / Math.PI);
                Distance = new int[] { end[0] - start[0], end[1] - start[1] };
                tang2 = 1;
                tang = result;
                rotation = angle + 90;
                quadrant = 2;
            }
            if (start[0] == end[0] && start[1] < end[1]) // 180
            {
                rotation = 180;
                tang2 = 0;
                tang = 0;
                Distance = new int[] { 0, end[1] - start[1] };
                quadrant = 2;
            }
            if (start[0] > end[0] && start[1] < end[1]) // 181 - 269
            {
                double overstaandezijde = end[1] - start[1];
                double aanliggendezijde = start[0] - end[0];
                double result = overstaandezijde / aanliggendezijde;
                double radians = Math.Atan2(aanliggendezijde, overstaandezijde);
                double angle = radians * (180 / Math.PI);
                Distance = new int[] { start[0] - end[0], start[1] - end[1] };
                tang2 = 0;
                tang = result;
                rotation = angle + 180;
                if (rotation < 225)
                {
                    tang2 = 0;
                    tang = result;
                }
                if (rotation == 225)
                {
                    tang2 = 2;
                    tang = 1;
                }
                if (rotation > 225)
                {
                    tang2 = 1;
                    tang = result;
                }
                quadrant = 3;
            }
            if (start[0] > end[0] && start[1] == end[1]) // 270
            {
                rotation = 270;
                //rotation = 180;
                tang2 = 1;
                tang = 0;
                Distance = new int[] { start[0] - end[0], 0 };
                quadrant = 3;
            }
            if (start[0] > end[0] && start[1] > end[1]) // 271 - 359
            {
                double overstaandezijde = start[1] - end[1];
                double aanliggendezijde = start[0] - end[0];
                double result = overstaandezijde / aanliggendezijde;
                double radians = Math.Atan2(overstaandezijde, aanliggendezijde);
                double angle = radians * (180 / Math.PI);
                Distance = new int[] { start[0] - end[0], start[1] - end[1] };

                rotation = angle + 270;
                if (rotation < 315)
                {
                    tang2 = 0;
                    tang = result;
                }
                if (rotation == 315)
                {
                    tang2 = 2;
                    tang = 1;
                }
                if (rotation > 315)
                {
                    tang2 = 1;
                    tang = result;
                }
                quadrant = 4;
            }
            if (rotation > 360)
            {
                rotation -= 360;
            }
            if (rotation < 0)
            {
                rotation += 360;
            }
            return Convert.ToInt32(rotation);
        }


        public static int getlength(Vector2 start, Vector2 end)
        {
            double rotation = GetRotation(start, end);
            int[] distances = new int[2];
            if (start.X > end.X)
                distances[0] = (int)start.X - (int)end.X;
            if (start.X < end.X)
                distances[0] = (int)end.X - (int)start.X;
            if (start.Y > end.Y)
                distances[1] = (int)start.Y - (int)end.Y;
            if (start.Y < end.Y)
                distances[1] = (int)end.Y - (int)start.Y;

            return (int)Math.Sqrt((Math.Pow(distances[1], 2)) + (Math.Pow(distances[0], 2)));
        }
    }
}
