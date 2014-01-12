using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ludum_dare_24
{
    class EnemyHandling
    {
        public static string[] EnemyNames = new string[] { "fish 1", "fish2", "stage1", "stage2", "stage2" };
        public static bool[] doesfightplayer = new bool[] { false, false, false, true, true };
        public static Sprite[] EnemySprites;
        public static Vector2[] EnemyRadius;
        public static float[] EnemySpeeds = new float[] { 1.75f, 2.0f, 2.25f, 2.50f, 2.50f };
        public static float[] enemyaccelerations = new float[] { 0.05f, 0.05f, 0.05f, 0.02f, 0.02f };
        public static double[] enemyhealth = new double[] { 40, 60, 100, 150, 150 };
        public static int[] rarety = new int[] { 40, 90, 70, 80, 80 };
        public static int maxtype;

        public static List<Enemy> Enemylist;

        public static void Initialize(ContentManager content)
        {
            Enemylist = new List<Enemy>();
            maxtype = EnemyNames.Length;
            EnemySprites = new Sprite[EnemyNames.Length];
            EnemyRadius = new Vector2[EnemyNames.Length];
            for (int a = 0; a < EnemyNames.Length; a++)
            {
                EnemySprites[a] = new Sprite(content, EnemyNames[a], new Vector2(0, 0));
                EnemyRadius[a] = EnemySprites[a].GetRadius();
                if (a == 2 || a == 3)
                    EnemySprites[a].SetColor(Color.Red);
            }

        }


        static int TimetoAddNewEnemy = 120;

        public static void Update()
        {
            maxtype = PlayerHandling.currentplayerstage + 1;
            Random rand1 = new Random();
            againplease:
            foreach (Enemy E in Enemylist)
            {
                if ( E.health <= 0)
                {
                    if ( rand1.Next(0,100) > 80 )
                        Game1.starlist.Add(new Star(E.Position));
                    Enemylist.Remove(E);
                    
                    goto againplease;
                }
            }
            foreach (Enemy E in Enemylist)
            {
                if ( E.health > 0)
                    E.Update();
                if (doesfightplayer[E.type] == true && E.delay_fight == 0)
                {
                    if (Movement.getlength(E.Position, Player.Position) < 200)
                    {
                        E._moveto = Player.Position;
                        if (Movement.getlength(E.Position, E._moveto) < 32)
                        {
                            Random rand2 = new Random();
                            int damage = rand2.Next(15,50);
                            FontText.AddText("-" + damage, "ouch!!", (int)Player.Position.X, (int)Player.Position.Y, Color.Red, 60);
                            PlayerHandling.HEALTH -= damage;
                            E.delay_fight = 120;
                        }
                    }

                    
                }
            }
            if (TimetoAddNewEnemy > 0)
                TimetoAddNewEnemy--;
            if (TimetoAddNewEnemy == 0)
            {
                Random rand = new Random();
                TimetoAddNewEnemy = rand.Next(120, 300);
                Enemylist.Add(new Enemy());
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            foreach (Enemy E in Enemylist)
            {
                if (E.health > 0)
                {
                    EnemySprites[E.type].UpdatePosition(E.Position);
                    EnemySprites[E.type].SetRotation(E.Rotation);
                    EnemySprites[E.type].Draw(batch);
                }
            }
               
        }
    }
}
