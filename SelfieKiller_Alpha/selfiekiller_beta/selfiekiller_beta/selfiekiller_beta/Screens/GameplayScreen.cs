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
        bool isPlayerHit = false;
    
        //Timeline
        Texture2D timeLineTexture;
        Texture2D timeLineBorder;
        Rectangle timeLineRectangle;
        Vector2 borderPos = new Vector2(500f,10f);
        int timeLinedec;
        bool gameEnd = false;

        //Health
        Texture2D[] arrayHealth = new Texture2D[5];

        //Camera
        TimeSpan time = TimeSpan.Zero;
        float cameraPosition = 0;

        //Enemies
        List<Enemy> jejemon = new List<Enemy>();
        float spawn = 0;
        bool isEnemyHit = false;
        Texture2D jejehit;
        bool CollisionOn = true;



        //Flag
        Flag flag;
        float endPositionX = 10000.0f;

        //timer
        int counter = 1;
        int limit = 1;
        float countDuration = .2f; //every  2s.
        float currentTime = 0f;

        InputSystem input;

        #endregion 
        #region CONSTRUCTOR
        public GameplayScreen(ContentManager content)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.content = content;

        }
        #endregion
        #region LOAD AND UNLOAD CONTENT
        public override void LoadContent()
        {
            this.input = ScreenManager.InputSystem;
            this.spriteBatch = ScreenManager.SpriteBatch;

            backgrounds = new Background[3];
            backgrounds[0] = new Background(content, "Backgrounds/Background0", 0.2f);
            backgrounds[1] = new Background(content, "Backgrounds/Background1", 0.5f);
            backgrounds[2] = new Background(content, "Backgrounds/Background2", 0.8f);

            //player
            player = new Player(new Vector2(180.0f, 500.0f), ScreenManager, content);
            playerHitSprite = content.Load<Texture2D>("hitrect");

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
            flag = new Flag(content, new Vector2(endPositionX, 240));

            //enemyhit
            jejehit = content.Load<Texture2D>("jejedead");

        }

        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion
        #region UPDATE
        public override void Update(GameTime gameTime, bool covered)
        {
            //Trace.WriteLine(player.isDestroy);
            if (!gameEnd)
            {
                cameraPosition += 1;

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

                //player
                player.Update(gameTime, input);

                //enemies
                spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (Enemy enemy in jejemon)
                {
                    enemy.Update();
                }

                if (timeLinedec < 250)
                { LoadEnemies(); }

                
                flag.Update();
                

                

                //LoseScreen
                if (player.life == 0)
                {
                    ScreenManager.AddScreen(new LoseScreen(content));
                    gameEnd = true;
                }

                //timeline
                timeLineRectangle = new Rectangle(505, 15, timeLinedec, 20);

                //collision
                HandleCollision();

                base.Update(gameTime, false);
            }
        }

        #endregion
        #region HANDLE COLLISION
        public void HandleCollision()
        {
            //Player's Attack Rect Touching and Enemy
            foreach (Enemy enemy in jejemon)
            {
                if (player.localRect.Intersects(enemy.localRect) && isPlayerHit == false
                    && player.isDestroy == true && isEnemyHit == false)
                {
                    for (int i = 0; i < jejemon.Count; i++)
                    {
                        CollisionOn = false;
                        isPlayerHit = false;
                        player.spriteColor = Color.Green;
                        isEnemyHit = true;
                        Trace.WriteLine("Enemy is hit!");
                    }

                }
            }

            //Player Rect Touching FlagRect (WINSCREEN)
            if (player.localRect.Intersects(flag.localRect) && gameEnd == false)
            {
                player.spriteColor = Color.Blue;
                gameEnd = true;
                ScreenManager.AddScreen(new WinScreen(content));
            }
            else
            {
                player.spriteColor = Color.White;
            }

            if (CollisionOn == true)
            {
                foreach (Enemy enemy in jejemon)
                {
                    if (player.localRect.Intersects(enemy.localRect) && isPlayerHit == false 
                        && player.isDestroy == false && player.isAvoid == false)
                    {
                        isPlayerHit = true;
                        player.life--;
                        player.spriteColor = Color.Red;
                    }
                    else
                    {
                        player.spriteColor = Color.White;
                    }
                }
            }

        }
        #endregion
        #region LOAD ENEMIES
        public void LoadEnemies()
        {

            if (spawn >= 1)
            {
                spawn = 0;
                if (jejemon.Count() < 5)
                {
                    jejemon.Add(new Enemy(content, new Vector2(2100, 270), "jeje"));
                }
            }

            for (int i = 0; i < jejemon.Count; i++)
            {
                
                if (!jejemon[i].isVisible || isEnemyHit == true)
                {
                    
                    isEnemyHit = false;
                    isPlayerHit = false;
                    jejemon[i].texture = jejehit;
                    if (jejemon[i].position.X < 0 - jejemon[i].texture.Width)
                    {
                        jejemon.RemoveAt(i);
                        i--;
                        CollisionOn = true;
                    }
                    
                }
            }
        }
        #endregion
        #region DRAW
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

            //flag
            flag.Draw(spriteBatch);

            //oldenemies
            foreach (Enemy enemy in jejemon)
                enemy.Draw(spriteBatch);

            //player
            player.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(timeLineTexture, timeLineRectangle, Color.White);
            spriteBatch.Draw(timeLineBorder, borderPos, Color.White);

            switch (player.life)
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
        #endregion
    }
}
