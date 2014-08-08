using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace selfiekiller_beta
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        Background[] backgrounds;

        Player player;

        TimeSpan time = TimeSpan.Zero;

        float cameraPosition = 0;

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
            player = new Player(new Vector2(100.0f, 450.0f), ScreenManager, content);
            
            

        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            cameraPosition += 1;

            time += gameTime.ElapsedGameTime;
            player.Update(gameTime);
            base.Update(gameTime, false);

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
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
