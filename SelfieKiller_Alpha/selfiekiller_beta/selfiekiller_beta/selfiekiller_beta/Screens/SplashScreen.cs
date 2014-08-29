using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace selfiekiller_beta
{
    public class SplashScreen : IntroScreen
    {
        ContentManager contentManager;
        GraphicsDeviceManager graphics;

        public SplashScreen(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            ScreenTime = TimeSpan.FromSeconds(0);
            FadeColor = Color.Black;
            FadeOpacity = 0.9f;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            this.contentManager = content;
            Texture = content.Load<Texture2D>("splash");

            Pixel = content.Load<Texture2D>("Spacer");
        }

        public override void Remove()
        {
            ScreenManager.AddScreen(new MainMenuScreen(contentManager, graphics));
            base.Remove();
        }
    }
}
