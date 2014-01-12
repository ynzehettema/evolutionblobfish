using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace ludum_dare_24
{
    public class HealthBar
    {
        public Sprite green;
        public Sprite red;
        public Sprite Frame;

        public HealthBar(ContentManager content)
        {
            green = new Sprite(content, "scorestuffy1", new Vector2(0, 0));
            red = new Sprite(content, "scorestuffy", new Vector2(0, 0));
            Frame = new Sprite(content, "scoreframe", new Vector2(0, 0));
        }

        public void Draw(SpriteBatch batch, Vector2 pos, double health, double maxhealth)
        {
            if (health > maxhealth)
                health = maxhealth;
            int tgreen;
            int tred;
            tgreen = (int)((health / maxhealth) * 160);
            tred = 160 - tgreen;
            Frame.UpdatePosition(pos);
            Frame.Draw(batch);
            for (int a = 0; a < tgreen; a++)
            {
                green.UpdatePosition(new Vector2(pos.X + 20 + a, pos.Y + 13));
                green.Draw(batch);
            }
            for (int a = 0; a < tred; a++)
            {
                red.UpdatePosition(new Vector2(pos.X + 20 + tgreen + a, pos.Y + 13));
                red.Draw(batch);
            }
        }

    }
}
