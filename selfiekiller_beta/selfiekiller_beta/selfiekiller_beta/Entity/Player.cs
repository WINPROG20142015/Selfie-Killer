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

        KeyboardState presentKey;
        KeyboardState pastKey;
        public Animation attackAnimation;
        public Animation idleAnimation;
        public Animation avoidAnimation;

        //int counter = 1;
        //int limit = 50;
        //float countDuration = 2f; //every  2s.
        //float currentTime = 0f;

        public bool isAvoid;
        public bool isDestroy;

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
            attackAnimation = new Animation(content.Load<Texture2D>("Sprites/Attack"), 0.1f, false);
            idleAnimation = new Animation(content.Load<Texture2D>("Sprites/Idle"), 0.1f, true);
            avoidAnimation = new Animation(content.Load<Texture2D>("Sprites/Avoid"), 0.1f, false);
        }

        public void Reset(Vector2 position)
        {
            Position = position;
            sprite.PlayAnimation(idleAnimation);
        }

        public void Update(GameTime gameTime)
        {
            //PlayerKeysUp();
            PlayerKeysDown(gameTime);
        }

        public void PlayerKeysDown(GameTime gameTime)
        {
            presentKey = Keyboard.GetState();


            if (presentKey.IsKeyDown(Keys.D) && pastKey.IsKeyUp(Keys.D))
            {
                sprite.PlayAnimation(attackAnimation);
                Trace.WriteLine("Destroy");
            }
            else if (presentKey.IsKeyDown(Keys.A) && pastKey.IsKeyUp(Keys.A))
            {
                sprite.PlayAnimation(avoidAnimation);
                Trace.WriteLine("Avoid");
            }
            else 
            {
                sprite.PlayAnimation(idleAnimation);
                Trace.WriteLine("Idle");
            }

            pastKey = presentKey;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White);
        }


    }
}
