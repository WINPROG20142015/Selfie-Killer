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
        #region STAGE1 VARS
        //GraphicsDevice graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
        Background[] backgrounds;

        //Player
        Player player;
        Texture2D playerHitSprite;
        Rectangle playerAtRect;
        bool isAttacking;
        bool isAvoiding;
        bool isPlayerHit = false;
    
        //Timeline
        Texture2D timeLineTexture;
        Texture2D timeLineBorder;
        Rectangle timeLineRectangle;
        Vector2 borderPos = new Vector2(500f,10f);
        int timeLinedec;

        //Health
        int s_onePlayerHealth =4;
        Texture2D[] arrayHealth = new Texture2D[5];

        //Collision
        public Color spriteColor;

        //Camera
        TimeSpan time = TimeSpan.Zero;
        float cameraPosition = 0;

        //Enemies
        List<Enemies> jejemon = new List<Enemies>();
        float spawn = 0;
        Rectangle enRect;

        //Flag
        Texture2D clearFlag;
        Rectangle flagRect;
        Rectangle TempRect;
        bool isFlagHit;
        bool gameEnd;

        //timer
        int counter = 1;
        int limit = 1;
        float countDuration = .01f; //every  2s.
        float currentTime = 0f;

        //charinput
        KeyboardState pastKey;

        #endregion 
        #region CONSTRUCTOR
        public GameplayScreen(ContentManager content)
        {
            isFlagHit = false;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.content = content;
        }
        #endregion
        #region LOAD AND UNLOAD CONTENT
        public override void LoadContent()
        {
            if (gameEnd == false)
            {
                spriteBatch = ScreenManager.SpriteBatch;
                backgrounds = new Background[3];
                backgrounds[0] = new Background(content, "Backgrounds/Background0", 0.2f);
                backgrounds[1] = new Background(content, "Backgrounds/Background1", 0.5f);
                backgrounds[2] = new Background(content, "Backgrounds/Background2", 0.8f);
                //player
                player = new Player(new Vector2(180.0f, 500.0f), ScreenManager, content);
                playerHitSprite = content.Load<Texture2D>("hitrect");
                playerAtRect = new Rectangle(10, 240, 50, 250);

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
                flagRect = new Rectangle(1000, 240, clearFlag.Width, clearFlag.Height);
                TempRect = new Rectangle(10, 240, 100, 100);

                //enemyrect
                enRect = new Rectangle(1100, 320, 100, 100);
            }
        }
        public override void UnloadContent()
        {
            if (gameEnd == true)
            {
                content.Unload();
            }
        }
        #endregion
        #region UPDATE
        public override void Update(GameTime gameTime, bool covered)
        {
            
            if (gameEnd == false)
            {
                cameraPosition += 1;

                player.Update(gameTime);

                time += gameTime.ElapsedGameTime;
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                    if (isFlagHit == false)
                    { flagRect.X -= 10; }
                }

                //enemies
                spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Enemies enemy in jejemon)
                {
                    enRect.X -= 10;
                    enemy.Update();
                }
                if (timeLinedec <= 250)
                {
                    LoadEnemies();
                }

                //LoseScreen
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
        #endregion
        #region INPUT CHECK
        public void createAttackCol()
        {
            KeyboardState presentKey = Keyboard.GetState();
            //ATTACKING INPUT-----------------------------------------------------
            if (presentKey.IsKeyDown(Keys.D) && pastKey.IsKeyUp(Keys.D))
            {
                isAttacking = true;
                playerAtRect = new Rectangle(150, 240, 50, 250);
            }
            else if (pastKey.IsKeyDown(Keys.D))
            {
                isAttacking = false;
                playerAtRect = new Rectangle(0, 0, 0, 0);
            }
            //AVOIDING INPUT---------------------------------------------------------
            if (presentKey.IsKeyDown(Keys.A) && isAvoiding == false)
            {
                isAvoiding = true;
                TempRect = new Rectangle(0, 0, 0, 0);
            }
            else if (pastKey.IsKeyDown(Keys.A))
            {
                isAvoiding = false;
                TempRect = new Rectangle(10, 240, 100, 100);
            }
            //Present > Past
            pastKey = presentKey;
        }
        #endregion
        #region HANDLE COLLISION
        public void HandleCollision()
        {
            //Player Rect Touching FlagRect (WINSCREEN)
            if (TempRect.Intersects(flagRect) && isFlagHit == false)
            {
                player.spriteColor = Color.Blue;
                isFlagHit = true;
                Trace.WriteLine("Touched Flag");
                gameEnd = true;
                ScreenManager.AddScreen(new WinScreen(content));
                Trace.WriteLine(gameEnd);
            }
            else
            {
                player.spriteColor = Color.White;
            }
            //Player Rect Touching an Enemy Rect
            if (TempRect.Intersects(enRect) && isPlayerHit == false && isAvoiding == false)
            {
                isPlayerHit = true;
                s_onePlayerHealth --;
                player.spriteColor = Color.Red;
                Trace.WriteLine("Health: " + s_onePlayerHealth);
            }
            //Player's Attack Rect Touching and Enemy
            if (playerAtRect.Intersects(enRect) && isPlayerHit == false)
            {
                for (int i = 0; i < jejemon.Count; i++)
                    {
                        isPlayerHit = false;
                        enRect = new Rectangle(1100, 240, 100, 100);
                        jejemon.RemoveAt(i);
                        i--; 
                    }  
                Trace.WriteLine("Enemy is hit!");
            }
        }
        #endregion
        #region LOAD ENEMIES
        public void LoadEnemies()
        {
            if (spawn >= 1)
            {
                spawn = 0;
                if (jejemon.Count() < 1)
                {
                    jejemon.Add(new Enemies(content.Load<Texture2D>("jeje"), new Vector2(1100,270)));
                }
            }

            for (int i = 0; i < jejemon.Count; i++)
                if (!jejemon[i].isVisible)
                {
                    isPlayerHit = false;
                    enRect = new Rectangle(1100, 240, 100, 100);
                    jejemon.RemoveAt(i);
                    i--;
                   
                }            
        }
        #endregion
        #region DRAW
        public override void Draw(GameTime gameTime)
        {
            if (gameEnd == false)
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
                foreach (Enemies enemy in jejemon)
                    enemy.Draw(spriteBatch);

                //player
                player.Draw(gameTime, spriteBatch);
                spriteBatch.Draw(timeLineTexture, timeLineRectangle, Color.White);
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
        #endregion
    }
}
