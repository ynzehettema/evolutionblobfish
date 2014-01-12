using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ludum_dare_24
{
    class PlayerHandling
    {
        public static string[] CreatureNames = new String[] { "Blobfish", "Better than blobfish fish", "Slightly bigger fish", "Almost Sharkfish", "Realy-close-to-sharkfish fish", "So-Close-to-sharkfish fish" };
        public static Sprite[] StageSprites;
        public static string[] StageSpriteNames = new string[] { "stage1", "stage2", "stage2", "stage3", "stage4", "stage4" };
        public static Sprite[] StageAttackSprites;
        public static string[] AttackSpriteNames = new string[] { "stage1attack", "stage2attack", "stage2attack", "stage3attack", "stage4attack"};
        public static Vector2[] PlayerRadius;
        public static float[] PlayerSpeedperStage = new float[] { 3.0f, 4.0f, 4.0f, 4.2f, 4.5f, 4.5f};
        public static float[] PlayerAcceleration = new float[] { 0.05f, 0.02f, 0.03f, 0.04f, 0.05f, 0.05f };
        public static double[] PlayerMaxDamage = new double[] { 15, 30, 35, 38, 37, 38 };
        public static double[] ScoreTonextEvolution = new double[] { 250, 600, 1400, 2500, 5000, 9999 };
        public static double[] turningspeed = new double[] { 6, 4, 5, 5, 6, 6 };
        public static double[] HealthperStage = new double[] { 100, 120, 140, 160, 180, 200 };
        public static double HEALTH = 100;
        public static double raisehealth = 0;

        public static double PlayerScore = 0;
        public static double Addscore = 0;

        public static int currentplayerstage = 0;
        public static HealthBar PowerUpBar;
        public static double POWA = 100;
        public static double POWAMAX = 100;

        public static HealthBar scorebar;
        public static HealthBar healthbar;
        public static Sprite mount;
        

        public static void Initialize(ContentManager content)
        {
            mount = new Sprite(content, "stage2mountedgun", new Vector2(0,0));
            scorebar = new HealthBar(content);
            healthbar = new HealthBar(content);
            PowerUpBar = new HealthBar(content);
            StageAttackSprites = new Sprite[AttackSpriteNames.Length];
            for (int a = 0; a < AttackSpriteNames.Length; a++)
            {
                StageAttackSprites[a] = new Sprite(content, AttackSpriteNames[a], new Vector2(0, 0));
            }


            StageSprites = new Sprite[StageSpriteNames.Length];
            PlayerRadius = new Vector2[StageSpriteNames.Length];
            for (int a = 0; a < StageSpriteNames.Length; a++)
            {
                StageSprites[a] = new Sprite(content, StageSpriteNames[a], new Vector2(0, 0));
                PlayerRadius[a] = StageSprites[a].GetRadius();
            }
            Player.Initialize();
        }

        static int delayattack = 60;

        static float AttackPOWA = 10.0f;
        static float AttackSpeed = 0.5f;
        static float AttackBuffer = 0.0f;
        static bool ISATTACKING = false;
        static bool ISATTACKING1 = false;
        static int attack1delay = 20;
        public static int enemieshit = 0;
        public static List<int> Nrenemieshit;

        static bool alreadyhit(int nr)
        {
            foreach (int I in Nrenemieshit)
                if (nr == I)
                    return true;
            return false;

        }
       
        static int addtoscore = 30;
        static int mountfirespeed = 180;

        public static void UpdatePlayerAttack()
        {
            if (POWA < POWAMAX)
                POWA += 0.2;
            if (currentplayerstage == 2)
            {
                
                mount.UpdatePosition(Player.Position);
                int closest = 9999999;
                int closestnr = -1;
                foreach (Enemy E in EnemyHandling.Enemylist)
                {
                    int l = Movement.getlength(Player.Position, E.Position);
                    if (l < closest)
                    {
                        closest = l;
                        closestnr = E.Nr;
                    }
                }
                if (closest < 350 && closestnr >= 0 && closestnr < EnemyHandling.Enemylist.Count)
                {
                    mount.SetRotation(Movement.GetRotation(Player.Position, EnemyHandling.Enemylist[closestnr].Position));
                    if (mountfirespeed == 0)
                    {
                        mountfirespeed = 120;
                        Game1.Projectilelist.Add(new Projectile(Player.Position + PlayerRadius[currentplayerstage] / 2, EnemyHandling.Enemylist[closestnr].Position, 0));
                        Game1.star.Play();
                    }
                }
            }

            Random rand = new Random();
            if (addtoscore > 0)
                addtoscore--;
            if (Addscore > 0 && addtoscore == 0)
            {
                addtoscore = 4;
                FontText.AddText("+" + Addscore + " xp", "scoreadded", 100, 20, Color.Blue, addtoscore + 2);
                Addscore--;
                PlayerScore++;
                
            }
            if (raisehealth > 0)
            {
                raisehealth--;
                HEALTH++;
            }
            if (Addscore > 80)
            {
                if (HEALTH < HealthperStage[currentplayerstage])
                    HEALTH++;
                FontText.AddText(" Regeneration!", "health", 660, 18, Color.Blue, 0);
            }
            if (delayattack > 0)
                delayattack--;
            if (Game1.MouseCurrent.LeftButton == ButtonState.Pressed && Game1.MousePrevious.LeftButton != ButtonState.Pressed && delayattack == 0 && ISATTACKING1 == false)
            {
                Nrenemieshit = new List<int>();
                enemieshit = 0;
                ISATTACKING1 = true;
                attack1delay = 20;
            }
            if (attack1delay == 0)
                ISATTACKING1 = false;
            if (attack1delay > 0)
                attack1delay--;
            if (ISATTACKING1 == true)
            {
                Vector2 attackpos = (Player.Position + (PlayerRadius[currentplayerstage] / 2)) + Vector2.Transform(new Vector2(0, 32f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Player.Rotation - 180)));
                foreach (Enemy E in EnemyHandling.Enemylist)
                {

                    if (alreadyhit(E.Nr) == false && Movement.getlength(attackpos, E.Position) < 32)
                    {
                        if (E.type == 1)
                        {
                            Game1.zap.Play();
                            double rand1 = rand.Next(10, 40);
                            FontText.AddText("health - " + rand1, "ouch!", (int)E.Position.X, (int)E.Position.Y, Color.DarkRed, 60);
                            HEALTH -= rand1;
                            ISATTACKING1 = false;
                        }
                        double damage = (double)rand.Next((int)PlayerMaxDamage[currentplayerstage] - 20, (int)PlayerMaxDamage[currentplayerstage]);
                        FontText.AddText("-" + damage, "Damageto" + E.Nr, (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y, Color.Green, 120);
                        double score = rand.Next(5, 20);
                        Game1.eat.Play();
                        Addscore += score;
                        E.health -= damage;
                        Nrenemieshit.Add(E.Nr);
                        enemieshit++;
                    }

                }

            }
            if (Game1.MouseCurrent.RightButton == ButtonState.Pressed && delayattack == 0 && POWA > 60)
            {
                POWA -= 60;
                Nrenemieshit = new List<int>();
                ISATTACKING = true;
                delayattack = 60;
                enemieshit = 0;
                AttackBuffer = 0.0f;
            }
            if (ISATTACKING == true)
            {
                Game1.ParticleList.Add(new particle(Player.Position + new Vector2(16,16),0));
                Player.Rotation = Movement.GetRotation(Player.Position, Game1.gamemousepos);
                if (AttackBuffer < AttackPOWA)
                {
                    AttackBuffer += AttackSpeed;
                }
                for (float a = 0; a < AttackBuffer; a += 0.1f)
                    Player.Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Player.Rotation - 180)));
                foreach (Enemy E in EnemyHandling.Enemylist)
                {
                    if (alreadyhit(E.Nr) == false && Movement.getlength(Player.Position, E.Position) < 32)
                    {
                        if (E.type == 1)
                        {
                            Game1.zap.Play();
                            double rand1 = rand.Next(10,40);
                            FontText.AddText("health - " + rand1,"ouch!",(int)E.Position.X,(int)E.Position.Y,Color.DarkRed,60);
                            HEALTH -= rand1;
                        }
                        double damage = (double)rand.Next((int)PlayerMaxDamage[currentplayerstage] - 20, (int)PlayerMaxDamage[currentplayerstage]);
                        FontText.AddText("-" + damage, "Damageto" + E.Nr, (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y, Color.Green, 120);
                        double score = rand.Next(5, 20);
                        Game1.eat.Play();
                        Addscore += score;
                        E.health -= damage;
                        Nrenemieshit.Add(E.Nr);
                        enemieshit++;
                    }
                }
                if ( Movement.getlength(Player.Position,Game1.mouseposition) < 4 )
                {
                    ISATTACKING = false;
                }
                if (AttackBuffer >= AttackPOWA)
                {
                    ISATTACKING = false;
                    if (enemieshit == 2)
                    {
                        FontText.AddText("Double Hit! + 20 xp", "combohit", (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y - 40, Color.Green, 120);
                        Addscore += 20;
                        Game1.combo1.Play();
                    }
                    if (enemieshit == 3)
                    {
                        FontText.AddText("Triple Hit! + 30 xp", "combohit", (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y - 40, Color.Green, 120);
                        Addscore += 30;
                        Game1.combo1.Play();
                    }
                    if (enemieshit == 4)
                    {
                        FontText.AddText("Multi Hit! + 40 xp", "combohit", (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y - 40, Color.Green, 120);
                        Addscore += 40;
                        Game1.combo2.Play();
                    }
                    if (enemieshit > 4)
                    {
                        FontText.AddText("Super Awesome Hit! + 50 xp", "combohit", (int)Game1.mouseposition.X, (int)Game1.mouseposition.Y - 40, Color.Green, 120);
                        Addscore += 50;
                        Game1.combo2.Play();
                    }
                }
            }

        }
        
        public static bool IsEvolving = false;
        public static bool hasleveledup = false;
        public static bool hasevolvedonce = false;

        public static void UpdateEvolution()
        {
            if (IsEvolving == false && PlayerScore > ScoreTonextEvolution[currentplayerstage] && currentplayerstage < ScoreTonextEvolution.Length - 1)
            {
                IsEvolving = true;
                hasleveledup = false;
                Game1.Evolved.Position = new Vector2( -Game1.Evolved.GetRadius().X, 300 - (Game1.Evolved.GetRadius().Y / 2));
                Game1.Evolved.SetScale(0.0f);
            }
            if (IsEvolving == true)
            {
                if ( Game1.Evolved.Position.X < 400 - (Game1.Evolved.GetRadius().X / 2 ))
                {
                    Game1.Evolved.Position.X += 2;
                    if ( Game1.Evolved.GetScale() < 1.0f)
                        Game1.Evolved.SetScale(Game1.Evolved.GetScale() + 0.003f);
                }
                if (hasleveledup == false && Game1.Evolved.Position.X > (400 - (Game1.Evolved.GetRadius().X / 2)) - 2 && Game1.Evolved.Position.X < (400 - (Game1.Evolved.GetRadius().X / 2)) + 2)
                {
                    Random rand = new Random();
                    hasleveledup = true;
                    Game1.evolve.Play();
                    Game1.Levelup.Play();
                    for (int a = 0; a < rand.Next(0, rand.Next(2, 5)); a++)
                        Game1.starlist.Add(new Star(new Vector2(rand.Next(0, (int)Game1.GameResolution.X), 40)));
                    AttackPOWA *= 1.5f;
                    currentplayerstage++;
                    raisehealth = HealthperStage[currentplayerstage] - HEALTH;
                    if (currentplayerstage == CreatureNames.Length - 1)
                    {
                        Game1.HASWON = true;
                        MediaPlayer.Stop();
                        MediaPlayer.IsRepeating = false;
                        MediaPlayer.Play(Game1.Win);
                    }
                }
                if (Game1.Evolved.Position.X > 400 - (Game1.Evolved.GetRadius().X / 2))
                {
                    Game1.Evolved.Position.X += 2; 
                    if (Game1.Evolved.GetScale() > 0.0f)
                        Game1.Evolved.SetScale(Game1.Evolved.GetScale() - 0.01f);
                }
                if (Game1.Evolved.Position.X > 800)
                {
                    IsEvolving = false;
                    if (currentplayerstage == CreatureNames.Length - 1)
                    {
                        Game1.ShowMessage("Congratiulations! \nYou've done it!\nIf you would be a blobfish\nYou'd be evolved to a shark by now!");
                        
                        return;
                    }
                }
            }

            
        }

        public static void Update()
        {
            if (HEALTH <= 0)
            {
                Game1.HASLOST = true;
                Game1.ShowMessage("You have lost! :(\nYou have reached fish level:\n\n" + CreatureNames[currentplayerstage]);
                MediaPlayer.Stop();
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Play(Game1.Lost);
            }
            if (Game1.HASWON == false && Game1.HASLOST == false)
            {
                StageSprites[currentplayerstage].SetScale(1.0f / Game1.GameCamera.Zoom);
                StageAttackSprites[currentplayerstage].SetScale(1.0f / Game1.GameCamera.Zoom);
                mount.SetScale(1.0f / Game1.GameCamera.Zoom);
            }
            UpdateEvolution();
            if (Game1.HASWON == false && Game1.HASLOST == false)
            {
                UpdatePlayerAttack();
                double wannaberotation = 0.0f;
                if (ISATTACKING == false)
                {
                    wannaberotation = Movement.GetRotation(Player.Position, Game1.gamemousepos);

                    if (Player.Rotation > 180 && (Player.Rotation - 180) > wannaberotation)
                    {
                        Player.Rotation += turningspeed[currentplayerstage]; goto skiprot;
                    }
                    if (Player.Rotation < 180 && (Player.Rotation + 180) < wannaberotation)
                    {
                        Player.Rotation -= turningspeed[currentplayerstage]; goto skiprot;
                    }
                    if (Player.Rotation < wannaberotation)
                        Player.Rotation += turningspeed[currentplayerstage];
                    if (Player.Rotation > wannaberotation)
                        Player.Rotation -= turningspeed[currentplayerstage];
                skiprot:
                    if (Player.Rotation > 360)
                        Player.Rotation -= 360;
                    if (Player.Rotation < 0)
                        Player.Rotation += 360;
                    Game1.MouseSprite.SetRotation(Movement.GetRotation(Player.Position, Game1.gamemousepos));
                    KeyboardState key = Keyboard.GetState();
                    if (key.IsKeyDown(Keys.W))
                    {
                        for (float a = 0; a < Player.speed; a += 0.1f)
                            Player.Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Player.Rotation - 180)));
                        if (Player.speed < PlayerSpeedperStage[currentplayerstage])
                            Player.speed += PlayerAcceleration[currentplayerstage];
                        if (mountfirespeed > 0)
                            mountfirespeed--;
                        return;
                    }
                    if (key.IsKeyDown(Keys.S))
                    {
                        for (float a = 0; a < Player.speed; a += 0.1f)
                            Player.Position += Vector2.Transform(new Vector2(0, 0.1f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Player.Rotation)));
                        if (Player.speed < PlayerSpeedperStage[currentplayerstage] / 2)
                            Player.speed += PlayerAcceleration[currentplayerstage] / 2;
                        if (mountfirespeed > 0)
                            mountfirespeed--;
                        return;
                    }
                    if (Player.speed > 0)
                        Player.speed -= PlayerAcceleration[currentplayerstage];
                }
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            if (ISATTACKING == false)
            {
                StageSprites[currentplayerstage].UpdatePosition(Player.Position);
                StageSprites[currentplayerstage].SetRotation(Player.Rotation);
                StageSprites[currentplayerstage].Draw(batch);
            }
            if (ISATTACKING == true || ISATTACKING1 == true)
            {
                StageAttackSprites[currentplayerstage].UpdatePosition(Player.Position);
                StageAttackSprites[currentplayerstage].SetRotation(Player.Rotation);
                StageAttackSprites[currentplayerstage].Draw(batch);
            }
            if (currentplayerstage == 2)
                mount.Draw(batch);
        }


    }
}
