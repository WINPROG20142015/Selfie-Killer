using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace selfiekiller_beta
{
    class MainMenuScreen : MenuScreen
    {
        MenuEntry newGame;
        MenuEntry quit;

        ContentManager content;
        GraphicsDeviceManager graphics;

        public MainMenuScreen(ContentManager content, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.content = content;

            newGame = new MenuEntry(this, "Start Game");
            quit = new MenuEntry(this, "Quit");

            Selected = Color.Red;
            NonSelected = Color.White;

            Removed += new EventHandler(MainMenuRemoved);
        }
        public override void Initialize()
        {
            int newGameX = graphics.GraphicsDevice.Viewport.Width - 500;
            int newGameY = graphics.GraphicsDevice.Viewport.Height / 2;
            //int newGameX = 100;
            //int newGameY = 100;
            newGame.SetPosition(new Vector2(newGameX, newGameY), true);
            newGame.Selected += new EventHandler(newGame_Selected);

            quit.SetRelativePosition(new Vector2(0, SpriteFont.LineSpacing), newGame, true);
            quit.Selected += new EventHandler(quit_Selected);

            MenuEntries.Add(newGame);
            MenuEntries.Add(quit);
        }

        void quit_Selected(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void newGame_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen(content));
            Remove();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Content;
            SpriteFont = content.Load<SpriteFont>(@"Fonts\\menuFont");
        }

        void MainMenuRemoved(object sender, EventArgs e)
        {
            MenuEntries.Clear();
        }

    }
}
