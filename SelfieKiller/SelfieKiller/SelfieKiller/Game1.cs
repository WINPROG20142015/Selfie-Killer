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
using ScrollingBackgroundSpace;
using System.Diagnostics;

namespace SelfieKiller
{
    #region MAIN
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region VARIABLES
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState; //aligabadi

        // Char Idle
        Texture2D charIdle;
        Texture2D charAvoid;
        Texture2D charAttack;
        // Set the coordinates to draw the sprite at.
        Vector2 spritePosition = new Vector2(10, 200);
        Vector2 spritePosition2 = new Vector2(10, 170);
        //BOOLEANS

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {base.Initialize();}


        private ScrollingBackground myBackground;
        protected override void LoadContent()
        {
            // ScrollingBackground
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myBackground = new ScrollingBackground();
            Texture2D background = Content.Load<Texture2D>("rizalbg");
            myBackground.Load(GraphicsDevice, background);

            // CHARACTER SPRITES
            spriteBatch = new SpriteBatch(GraphicsDevice);
            charIdle = Content.Load<Texture2D>("idle");
            charAttack = Content.Load<Texture2D>("attack");
            charAvoid = Content.Load<Texture2D>("avoid");
        }

        protected override void UnloadContent()
        {// TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            myBackground.Update(elapsed * 100);


            UpdateCharSprite(gameTime);
            base.Update(gameTime);
        }

        private void UpdateCharSprite(GameTime gameTime)
        {
            
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.A) && !currentKeyboardState.IsKeyDown(Keys.D))
            {
                Trace.WriteLine("Avoid");
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) && !currentKeyboardState.IsKeyDown(Keys.A))
            {
                Trace.WriteLine("Destroy"); 
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) && currentKeyboardState.IsKeyDown(Keys.A))
            {
                Trace.WriteLine("IDLE");
            }


        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            myBackground.Draw(spriteBatch);
            spriteBatch.End();

            if (currentKeyboardState.IsKeyDown(Keys.A) && !currentKeyboardState.IsKeyDown(Keys.D))
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(charAvoid, spritePosition, Color.White);
                spriteBatch.End();
            }
                 
            else if (currentKeyboardState.IsKeyDown(Keys.D) && !currentKeyboardState.IsKeyDown(Keys.A))
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(charAttack, spritePosition2, Color.White);
                spriteBatch.End();
            }

            else 
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(charIdle, spritePosition, Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
    #endregion
}
