using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;


namespace selfiekiller_beta
{
    public class GameplayScreen : GameScreen
    {
        //GraphicsDevice graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
        Background[] backgrounds;

        Player player;
        Texture2D playerHitSprite;
        Rectangle playerAtRect;
        bool isAttacking;
        bool isPlayerHit = false;
        int hitLimit = 1;

        //Timeline
        Texture2D timeLineTexture;
        Texture2D timeLineBorder;
        Rectangle timeLineRectangle;
        Vector2 borderPos = new Vector2(500f,10f);
        int timeLinedec;

        //Health
        int s_onePlayerHealth =4;
        Texture2D[] arrayHealth = new Texture2D[5];

        //collision
        public Color spriteColor;

        TimeSpan time = TimeSpan.Zero;

        float cameraPosition = 0;

        //oldenemies
        List<Enemies> enemies = new List<Enemies>();
        //Random random = new Random();
        Rectangle enRect;

        //flags
        Texture2D clearFlag;
        Rectangle flagRect;
        Rectangle TempRect;
        bool isFlagHit;
        bool gameEnd;

        //timer
        int counter = 1;
        int limit = 1;
        float countDuration = .001f; //every  2s.
        float currentTime = 0f;

        //charinput
        KeyboardState presentKey;
        KeyboardState pastKey;

        public GameplayScreen(ContentManager content)
        {
            isFlagHit = false;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.content = content;
        }

        public override void LoadContent()
        {

            spriteBatch = ScreenManager.SpriteBatch;
            backgrounds = new Background[3];
            backgrounds[0] = new Background(content, "Backgrounds/Background0", 0.2f);
            backgrounds[1] = new Background(content, "Backgrounds/Background1", 0.5f);
            backgrounds[2] = new Background(content, "Backgrounds/Background2", 0.8f);
            //player
            player = new Player(new Vector2(180.0f, 500.0f), ScreenManager, content);
            playerHitSprite = content.Load<Texture2D>("hitrect");
            playerAtRect = new Rectangle(190, 240, 50, 250);

            //timeline
            timeLineTexture = content.Load<Texture2D>("floor");
            timeLineBorder = content.Load<Texture2D>("ProgressBar");

            //health
            arrayHealth[0] = content.Load<Texture2D>("Battery/bat1");
            arrayHealth[1] = content.Load<Texture2D>("Battery/bat2");
            arrayHealth[2] = content.Load<Texture2D>("Battery/bat3");
            arrayHealth[3] = content.Load<Texture2D>("Battery/bat4");
            arrayHealth[4] = content.Load<Texture2D>("Battery/bat5");

            //flag
            clearFlag = content.Load<Texture2D>("flags/flag1");
            flagRect = new Rectangle(1000, 240,clearFlag.Width,clearFlag.Height);
            TempRect = new Rectangle(10, 240, 100, 100);

            //enemyrect
            enRect = new Rectangle(1100, 320,100, 100);
            
        }

        public override void UnloadContent()
        {
            content.Unload();
        }
      
        float spawn = 0;
        public override void Update(GameTime gameTime, bool covered)
        {
            if (gameEnd == false)
            {
                //Trace.WriteLine(enRect);
                //Trace.Write(isPlayerHit);
                cameraPosition += 1;

                time += gameTime.ElapsedGameTime;
                player.Update(gameTime);

                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //enemies
                spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Enemies enemy in enemies)
                {
                    enRect.X -= 15;
                    enemy.Update();
                }
                LoadEnemies();

                //timeline
                if (currentTime >= countDuration)
                {
                    counter++;
                    currentTime -= countDuration;
                }
                if (counter >= limit && timeLinedec <= 265)
                {
                    counter = 0;
                    timeLinedec += 2;
                }
                if (timeLinedec >= 250)
                {
                    //if (isFlagHit == false)
                    //{ flagRect.X -= 1; }
                }

                if (s_onePlayerHealth == 0)
                { ScreenManager.AddScreen(new LoseScreen(content)); gameEnd = true; }

                //timeline
                timeLineRectangle = new Rectangle(505, 15, timeLinedec, 20);
                //collision
                HandleCollision();
                createAttackCol();
                base.Update(gameTime, false);
            }
        }

        public void createAttackCol()
        {
            if (isAttacking == true)
            {
                playerAtRect = new Rectangle(190, 240, 50, 250);
            }
            else
            {
                playerAtRect = new Rectangle(0, 0, 0, 0);
            }

            presentKey = Keyboard.GetState();
            if (presentKey.IsKeyDown(Keys.D) && pastKey.IsKeyUp(Keys.D) && isAttacking == false)
            {
                isAttacking = true;
                Trace.Write("PRESSED D");
            }         
            pastKey = presentKey;
        }

        public void HandleCollision()
        {
            if (TempRect.Intersects(flagRect) && isFlagHit == false)
            {
                player.spriteColor = Color.Blue;
                isFlagHit = true;
                Trace.WriteLine("Intersecting");
                gameEnd = true;
                ScreenManager.AddScreen(new WinScreen(content));
            }
            else
            {
                player.spriteColor = Color.White;
            }

                if (TempRect.Intersects(enRect) && isPlayerHit == false)
                {
                    isPlayerHit = true;
                    s_onePlayerHealth --;
                    player.spriteColor = Color.Red;
                    Trace.WriteLine("Intersecting");
                }

                if (playerAtRect.Intersects(enRect) && isPlayerHit == false)
                {
                    for (int i = 0; i < enemies.Count; i++)
                        {
                            isPlayerHit = false;
                            enRect = new Rectangle(1100, 240, 100, 100);
                            enemies.RemoveAt(i);
                            i--; 
                        }  
                    Trace.WriteLine("Enemy HIT! OMG");
                }
            
           
        }

        
        public void LoadEnemies()
        {
            if (spawn >= 1)
            {
                spawn = 0;
                if (enemies.Count() < 1)
                {
                    enemies.Add(new Enemies(content.Load<Texture2D>("jeje"), new Vector2(1100,350)));
                }
            }

            for (int i = 0; i < enemies.Count; i++)
                if (!enemies[i].isVisible)
                {

                    isPlayerHit = false;
                    enRect = new Rectangle(1100, 240, 100, 100);
                    enemies.RemoveAt(i);
                    i--;
                   
                }            
        }

        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = ScreenAlpha;


            spriteBatch.Begin();
            
            for (int i = 0; i <= 2; ++i)
            {
                backgrounds[i].Draw(spriteBatch, cameraPosition);

            }
            if (isFlagHit == false)
            { spriteBatch.Draw(clearFlag, flagRect, Color.White); }
            if (isAttacking == true)
            { spriteBatch.Draw(playerHitSprite, playerAtRect, Color.White); }
            //oldenemies
            foreach (Enemies enemy in enemies)
                enemy.Draw(spriteBatch);

            //player
            player.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(timeLineTexture,timeLineRectangle,Color.White);
            spriteBatch.Draw(timeLineBorder, borderPos, Color.White);
            
            switch (s_onePlayerHealth) 
            {
                case 0: spriteBatch.Draw(arrayHealth[4], new Vector2(0f, 0f), Color.White);
                    break;
                case 1: spriteBatch.Draw(arrayHealth[3], new Vector2(0f, 0f), Color.White);
                    break;
                case 2: spriteBatch.Draw(arrayHealth[2], new Vector2(0f, 0f), Color.White);
                    break;
                case 3: spriteBatch.Draw(arrayHealth[1], new Vector2(0f, 0f), Color.White);
                    break;
                case 4: spriteBatch.Draw(arrayHealth[0], new Vector2(0f, 0f), Color.White);
                    break;

            }
            spriteBatch.End();

        }
    }
}
