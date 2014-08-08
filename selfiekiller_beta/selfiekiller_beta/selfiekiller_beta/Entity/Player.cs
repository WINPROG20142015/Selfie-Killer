using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace selfiekiller_beta
{
    public class Player
    {

        KeyboardState currentKeyboardState;
        public Animation attackAnimation;
        public Animation idleAnimation;
        public Animation avoidAnimation;

        private AnimationPlayer sprite;

        public bool IsIdle
        {
            get { return isIdle; }
            set { isIdle = value; }
        }
        bool isIdle;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        ScreenManager screenManager;
        ContentManager content;

        public Player(Vector2 position, ScreenManager screenManager, ContentManager content)
        {
            this.screenManager = screenManager;
            this.content = content;
            IsIdle = true;
            LoadContent();

            sprite = new AnimationPlayer();

            Reset(position);
        }

        public void LoadContent()
        {
            attackAnimation = new Animation(content.Load<Texture2D>("Sprites/attack"), 0.1f, true);
            idleAnimation = new Animation(content.Load<Texture2D>("Sprites/Idle"), 0.1f, true);
            avoidAnimation = new Animation(content.Load<Texture2D>("Sprites/avoid"), 0.1f, true);
        }

        public void Reset(Vector2 position)
        {
            Position = position;
            sprite.PlayAnimation(idleAnimation);
        }

        public void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.A) && !currentKeyboardState.IsKeyDown(Keys.D))
            {
                sprite.PlayAnimation(avoidAnimation);
                Trace.WriteLine("Avoid");
            }
    
            if (currentKeyboardState.IsKeyDown(Keys.D) && !currentKeyboardState.IsKeyDown(Keys.A))
            {
                sprite.PlayAnimation(attackAnimation);
                Trace.WriteLine("Destroy");
            }
            if (currentKeyboardState.IsKeyDown(Keys.D) && currentKeyboardState.IsKeyDown(Keys.A))
            {
                sprite.PlayAnimation(idleAnimation);
                Trace.WriteLine("Pressing Both");
            }
            else if (!currentKeyboardState.IsKeyDown(Keys.D) && !currentKeyboardState.IsKeyDown(Keys.A))
            {
                sprite.PlayAnimation(idleAnimation);
                Trace.WriteLine("IDLE");
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White);
        }


    }
}
