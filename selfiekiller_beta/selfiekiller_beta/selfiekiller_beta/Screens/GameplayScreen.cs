using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


namespace selfiekiller_beta
{
    public class GameplayScreen : GameScreen
    {
        GraphicsDevice graphics;
        ContentManager content;
        SpriteBatch spriteBatch;
        Background[] backgrounds;

        Player player;

        //Timeline
        Texture2D timeLineTexture;
        Texture2D timeLineBorder;
        Rectangle timeLineRectangle;
        Vector2 borderPos = new Vector2(500f,10f);
        int timeLinedec;

        //Health
        int s_onePlayerHealth;
        Texture2D[] arrayHealth = new Texture2D[5];

        //collision
        public Color spriteColor;

        TimeSpan time = TimeSpan.Zero;

        float cameraPosition = 0;

        //enemies
        List<Enemies> enemies = new List<Enemies>();
        Random random = new Random();
        Rectangle enRect;


        //flags
        Texture2D clearFlag;
        Rectangle flagRect;
        Rectangle TempRect;

        //timer
        int counter = 1;
        int limit = 1;
        float countDuration = .01f; //every  2s.
        float currentTime = 0f;


        public GameplayScreen(ContentManager content)
        {
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
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        float spawn = 0;
        public override void Update(GameTime gameTime, bool covered)
        {
            cameraPosition += 1;

            time += gameTime.ElapsedGameTime;
            player.Update(gameTime);

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //enemies
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemies enemy in enemies)
                enemy.Update(graphics);
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
                    //s_onePlayerHealth++;
                }
                if (timeLinedec >= 250)
                {
                    flagRect.X -= 5;
                }
                s_onePlayerHealth = 4;
                
            
            //timeline
            timeLineRectangle = new Rectangle(505, 15, timeLinedec, 20);

            //collision
            HandleCollision();

            LoadEnemies();
            base.Update(gameTime, false);

        }
        public void HandleCollision()
        {
            if (TempRect.Intersects(flagRect))
            {
                player.spriteColor = Color.Blue;
                Trace.WriteLine("Intersecting");

            }
            else if (TempRect.Intersects(enRect))
            {
                player.spriteColor = Color.Red;
                Trace.WriteLine("Intersecting");
            }
            else
            {
                player.spriteColor = Color.White;
            }
        }

        
        public void LoadEnemies()
        {
            if (spawn >= 1)
            {
                spawn = 0;
                if (enemies.Count() < 1)
                {
                    enemies.Add(new Enemies(content.Load<Texture2D>("jeje"), new Vector2(1100f, 360f),enRect));
                }
            }

            for (int i = 0; i < enemies.Count; i++)
                if (!enemies[i].isVisible)
                {
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
            //enemysprite
            spriteBatch.Draw(clearFlag, flagRect, Color.White);
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
