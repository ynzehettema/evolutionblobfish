using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace ludum_dare_24
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static MouseState MouseCurrent;
        public static MouseState MousePrevious;
        public static Vector2 mouseposition; // for easy acces :) * lazyness strikes! *
        public static Vector2 gamemousepos; 
        public static Sprite MouseSprite;
        static Sprite Background;

        public static SoundEffect eat;
        public static SoundEffect evolve; 
        public static SoundEffect zap;
        public static SoundEffect combo1;
        public static SoundEffect combo2;
        public static SoundEffect Levelup;
        public static SoundEffect star;

        static Sprite starsprite;
        public static List<Star> starlist;
        static Sprite bubblesprite;
        static Sprite stardustsprite;
        public static List<particle> ParticleList;
        static Sprite sharkteeth;
        public static List<Projectile> Projectilelist;

        public static bool PAUSED = true;

        public static Sprite Paused;
        public static string pausedtext = "Welcome To SpectreGames \nLudum Dare's Game\nQue the Instructions!";
        public static Sprite Evolved;

        public static Vector2 Resolution = new Vector2(800, 600);
        public static Vector2 GameResolution = new Vector2(800, 600);
        public static Camera2D GameCamera = new Camera2D();

        public static bool HASWON = false;
        public static bool HASREALYWON = false;
        public static bool HASLOST = false;

        public static Song Lost;
        public static Song Win;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }


        protected override void Initialize()
        {
            starlist = new List<Star>();
            ParticleList = new List<particle>();
            Projectilelist = new List<Projectile>();
            // TODO: Add your initialization logic here
            FontText.font = Content.Load<SpriteFont>("Comic Sans MS");
            eat = Content.Load<SoundEffect>("eat");
            evolve = Content.Load<SoundEffect>("evolve");
            zap = Content.Load<SoundEffect>("zap");
            combo1 = Content.Load<SoundEffect>("combo");
            combo2 = Content.Load<SoundEffect>("combo2");
            Levelup = Content.Load<SoundEffect>("levelup");
            star = Content.Load<SoundEffect>("star");
            starsprite = new Sprite(this.Content, "xp star", new Vector2(0, 0));
            bubblesprite = new Sprite(this.Content, "bubble", new Vector2(0, 0));
            stardustsprite = new Sprite(this.Content, "stardust", new Vector2(0, 0));
            sharkteeth = new Sprite(this.Content, "sharktooth", new Vector2(0, 0));
            Paused = new Sprite(this.Content, "Message", new Vector2(400 - 140, 300 - 80));
            Evolved = new Sprite(this.Content, "Evolved", new Vector2(- 200,0));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Song song = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.6f;
            MediaPlayer.Play(song);

            GameCamera.Pos = new Vector2(0, 0);
            GameCamera.Zoom = 1f;
            GameCamera.Pos = Resolution / 2;

            Mouse.SetPosition((int)Resolution.X / 2, (int)Resolution.Y / 2);


            Lost = Content.Load<Song>("lost");
            Win = Content.Load<Song>("Win");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerHandling.Initialize(this.Content);
            EnemyHandling.Initialize(this.Content);
            MouseCurrent = Mouse.GetState();
            MousePrevious = MouseCurrent;
            mouseposition = new Vector2(MouseCurrent.X, MouseCurrent.Y);
            MouseSprite = new Sprite(this.Content, "cursor", mouseposition);
            Background = new Sprite(this.Content,"background", new Vector2(0,0));
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static void ShowMessage(string message)
        {
            pausedtext = message;
            PAUSED = true;
        }

        public void Updatepause()
        {
            if (mouseposition.X > (400 - 140) + 88 && mouseposition.X < (400 - 140) + 192 &&
                mouseposition.Y > (300 - 80) + 128 && mouseposition.Y < (300 - 80) + 146 && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton != ButtonState.Pressed)
            {
                PAUSED = false;
                if (HASLOST == true || HASREALYWON == true)
                    this.Exit();
                if (HASWON == true)
                {
                    ShowMessage("Thank you for playing!\nThis game was made by:\nMetaldemon from Spectregames");
                    HASREALYWON = true;
                }
            }
        }

        int frameruns = 0;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            

            UpdateCamera();
            MouseCurrent = Mouse.GetState();
            mouseposition = (new Vector2(Game1.MouseCurrent.X, Game1.MouseCurrent.Y) - Game1.GameResolution / 2) + Game1.GameCamera._pos;
            gamemousepos = mouseposition / GameCamera.Zoom;
            MouseSprite.UpdatePosition(mouseposition);
            if (PAUSED == false)
            {
                if (frameruns < 16)
                {
                    
                    frameruns++;
                }
                if (frameruns == 15)
                    ShowMessage("Eat other fishies to gain Xp!\nOnce the xp bar is full...\nYou'll EVOLVE");
                if (frameruns == 10)
                    ShowMessage("Attack with [Left Mouse Button]\nPreform a special attack with:\n[Right Mouse Button]");
                if (frameruns == 5)
                    ShowMessage("Steer your fish with: \nThe mouse\nMove your fish with:\n[W]");
                if (frameruns == 1)
                    ShowMessage("This game is called:\nThe evolution of the blobfish!\nIt's about... You guessed it!\nThe \"Evolution of the blobfish!\" ");
                
                PlayerHandling.Update();
                EnemyHandling.Update();
                UpdateMisc();
            }
            if (PAUSED == true)
                Updatepause();
            
            MousePrevious = MouseCurrent;



            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        public void UpdateCamera()
        {

            GameResolution = Resolution / GameCamera.Zoom;

            if (PlayerHandling.currentplayerstage == 2)
            {
                if (GameCamera.Zoom > 0.5f)
                {
                    GameCamera.Zoom -= 0.005f;
                }
            }
            GameCamera.Pos = GameResolution / 2;

            /*if (Movement.getlength(GameCamera.Pos,GameResolution / 2) > 4)
            {
                GameCamera.Pos += Vector2.Transform(new Vector2(0, 2f), Matrix.CreateRotationZ(MathHelper.ToRadians((float)Movement.GetRotation(GameCamera.Pos, Resolution / 2) - 180)));

            }*/
        }

        public void UpdateMisc()
        {
        again1:
            foreach (particle B in ParticleList)
            {
                if (B.time == 0)
                {
                    ParticleList.Remove(B); goto again1;
                }
            }
        foreach (particle B in ParticleList)
            if (B.time > 0)
                B.time--;
            again2:
        foreach (Projectile B in Projectilelist)
            if (B.Position.X < 0 || B.Position.X > 800 || B.Position.Y < 0 || B.Position.Y > 600)
            {
                Projectilelist.Remove(B);
                goto again2;
            }
            foreach (Projectile B in Projectilelist)
            {
                B.Update();
                foreach (Enemy E in EnemyHandling.Enemylist)
                {
                    if (B.Position.X + 1> E.Position.X && B.Position.X < E.Position.X + EnemyHandling.EnemyRadius[E.type].X &&
                        B.Position.Y + 1 > E.Position.Y && B.Position.Y < E.Position.Y + EnemyHandling.EnemyRadius[E.type].Y)
                    {
                        Random rand = new Random();
                        double damage = (double)rand.Next((int)PlayerHandling.PlayerMaxDamage[PlayerHandling.currentplayerstage] - 20, (int)PlayerHandling.PlayerMaxDamage[PlayerHandling.currentplayerstage]);
                        FontText.AddText("-" + damage, "Damageto" + E.Nr, (int)Player.Position.X, (int)Player.Position.Y, Color.Green, 120);
                        double score = rand.Next(5, 20);
                        PlayerHandling.Addscore += score;
                        E.health -= damage;
                        PlayerHandling.Nrenemieshit.Add(E.Nr);
                        PlayerHandling.enemieshit++;
                    }
                }
            }
        again:
            foreach (Star S in starlist)
            {
                if (S.Position.Y > GameResolution.Y)
                {
                    starlist.Remove(S); goto again;
                }
            }
            foreach (Star S in starlist)
            {
                S.Update();
                if (S.timeleftbeforecatch == 0 && Movement.getlength(Player.Position, S.Position) < 32)
                {
                    combo1.Play();
                    FontText.AddText("Star! + 40 xp!", "woot a star", (int)S.Position.X, (int)S.Position.Y, Color.Green, 60);
                    PlayerHandling.Addscore += 40;
                    S.Position.Y = GameResolution.Y + 50;
                }
            }
        }

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            Background.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        GameCamera.get_transformation(graphics.GraphicsDevice));
            EnemyHandling.Draw(spriteBatch);
            foreach (particle B in ParticleList)
                if (B.time > 0)
                {
                    if (B.Type == 0)
                    {
                        bubblesprite.UpdatePosition(B.Position);
                        bubblesprite.Draw(spriteBatch);
                    }
                    if (B.Type == 1)
                    {
                        stardustsprite.UpdatePosition(B.Position);
                        stardustsprite.Draw(spriteBatch);
                    }
                }
            foreach (Projectile B in Projectilelist)
                    if (B.Type == 0)
                    {
                        sharkteeth.UpdatePosition(B.Position);
                        sharkteeth.SetRotation(B.Rotation);
                        sharkteeth.Draw(spriteBatch);
                    }
            foreach (Star S in starlist)
            {
                starsprite.UpdatePosition(S.Position);
                starsprite.SetRotation(S.Rotation);
                starsprite.Draw(spriteBatch);
            }

            PlayerHandling.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
                PlayerHandling.scorebar.Draw(spriteBatch, new Vector2(4, 4), PlayerHandling.PlayerScore, PlayerHandling.ScoreTonextEvolution[PlayerHandling.currentplayerstage]);
            PlayerHandling.healthbar.Draw(spriteBatch, new Vector2(600,4), PlayerHandling.HEALTH, PlayerHandling.HealthperStage[PlayerHandling.currentplayerstage]);
            FontText.AddText("Health: " + PlayerHandling.HEALTH, "health", 630, 18, Color.Blue, 0);
            PlayerHandling.PowerUpBar.Draw(spriteBatch, new Vector2(230, 4), PlayerHandling.POWA, PlayerHandling.POWAMAX);
            FontText.AddText("PowerUp!", "powa", 280, 18, Color.Blue, 0);
            if (PAUSED == true)
            {
                Paused.Draw(spriteBatch);
                FontText.AddText(pausedtext, "paused", 400 - 100, 300 - 60, Color.Yellow, 5);
                MouseSprite.SetRotation(270 + 45);
            }
            if (PlayerHandling.IsEvolving == true)
                Evolved.Draw(spriteBatch);
            FontText.Draw(spriteBatch);
            MouseSprite.Draw(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
