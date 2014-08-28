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
        public Animation currentAnimation;
        public string mCurrentState;

        public bool timerEnabled = false;
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

        //collision
        public Color spriteColor;
        public Rectangle localRect;
        public Rectangle BoundingRect
        {
            get
            {
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + (int)currentAnimation.Texture.Bounds.Y;
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + (int)sprite.Origin.X;
                return new Rectangle(left, top,
                    localRect.Width,
                    localRect.Height);
            }
        }

        ScreenManager screenManager;
        ContentManager content;

        public Player(Vector2 position, ScreenManager screenManager, ContentManager content)
        {
            this.screenManager = screenManager;
            this.content = content;
            IsIdle = true;
            LoadContent();
            spriteColor = Color.White;
            sprite = new AnimationPlayer();
            Reset(position);
        }

        public void LoadContent()
        {
            attackAnimation = new Animation(content.Load<Texture2D>("Sprites/Attack"), 0.01f, false);
            idleAnimation = new Animation(content.Load<Texture2D>("Sprites/Idle3"), 0.1f, true);
            avoidAnimation = new Animation(content.Load<Texture2D>("Sprites/Avoid"), 0.1f, false);
        }

        public void Reset(Vector2 position)
        {
            Position = position;
            currentAnimation = idleAnimation;
            sprite.PlayAnimation(currentAnimation);
        }

        public void Update(GameTime gameTime)
        {
            timerpress = true;
            // clock start and update  
            if (clockIsRunning == false)
            {
                //count 10 seconds down 
                start(2);
                mCurrentState = "Walking";
            }
            else
            {
                checkTime(gameTime);
            }

            if (currentAnimation == idleAnimation) { sprite.PlayAnimation(idleAnimation); }
            if (currentAnimation == attackAnimation) { sprite.PlayAnimation(attackAnimation); }
            if (currentAnimation == avoidAnimation) { sprite.PlayAnimation(avoidAnimation); }

            PlayerKeysDown(gameTime);
            HandleCollision();
        }
        bool timerpress=false;
        public void PlayerKeysDown(GameTime gameTime)
        {
            presentKey = Keyboard.GetState();

            if (presentKey.IsKeyDown(Keys.D) && pastKey.IsKeyUp(Keys.D))
            {
                mCurrentState = "Destroying"; 
            }
            if (presentKey.IsKeyDown(Keys.A) && pastKey.IsKeyUp(Keys.A))
            {
                mCurrentState = "Avoiding";
            }
            //-----------------------------------------------------------------------

            switch (mCurrentState) 
            { 
                case "Walking":
                    currentAnimation = idleAnimation;
                    break;
                case "Destroying":
                    isDestroy = true;
                    if (isDestroy)
                    {
                        currentAnimation = attackAnimation;
                        //Trace.WriteLine("Destroy");
                    }
                    break;
                case "Avoiding":
                    isAvoid = true;
                    if (isAvoid)
                    {
                        currentAnimation = avoidAnimation;
                        //Trace.WriteLine("Avoid");
                    }
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, spriteColor);
        }

        public void HandleCollision()
        {}

        #region TIMER
        //-------------------------TIMER------------------------------------------------------------
        private int endTimer; 
        private int countTimerRef;
        public bool clockIsRunning;
        public bool clockIsFinished;
         
 
        public void ClockTimer() 
        { 
 
            endTimer = 0; 
            countTimerRef = 0;
            clockIsRunning = false;
            clockIsFinished = false; 
 
        } 
        public void start(int seconds) 
        { 
            endTimer = seconds;
            clockIsRunning = true; 
        } 
 
        public Boolean checkTime(GameTime gameTime) 
        { 
            
            if (!clockIsFinished) 
            {

                if (timerpress == true)
                { countTimerRef += (int)gameTime.ElapsedGameTime.TotalMilliseconds; }

                if (countTimerRef >= 300.0f) 
                { 
                    endTimer = endTimer - 1; 
                    countTimerRef = 0; 
                    if (endTimer <= 0) 
                    { 
                        endTimer = 0;
                        clockIsFinished = true; 
                    } 
                }
                
            } 
            else 
            {
                pastKey = presentKey;
                mCurrentState = "Walking";
                reset();
            }
            return clockIsFinished; 
        } 
        public void reset() 
        {
            clockIsRunning = false;
            clockIsFinished = false; 
            countTimerRef = 0; 
            endTimer = 0;
        }
        #endregion
    }
}
