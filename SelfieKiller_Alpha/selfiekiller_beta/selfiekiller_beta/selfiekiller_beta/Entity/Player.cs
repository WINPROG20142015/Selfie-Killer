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
        public int life = 4;
        
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

        public Color spriteColor;

        //collision
        public Rectangle localRect;

        ScreenManager screenManager;
        ContentManager content;

        float countDuration = 0.4f; //every  0.5s.
        float currentTime = 0f;

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
            idleAnimation = new Animation(content.Load<Texture2D>("Sprites/Idle"), 0.1f, true);
            avoidAnimation = new Animation(content.Load<Texture2D>("Sprites/Avoid"), 0.1f, false);

        }

        public void Reset(Vector2 position)
        {
            Position = position;
            currentAnimation = idleAnimation;
            sprite.PlayAnimation(currentAnimation);

            localRect = new Rectangle(0, 0, currentAnimation.FrameWidth / 4, currentAnimation.FrameHeight);
        }

        public void Update(GameTime gameTime, InputSystem input)
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

            PlayerKeysDown(gameTime, input);
            HandleCollision();

            if (isIdle)
            {
                if (sprite.Animation == avoidAnimation)
                {
                    isAvoid = true;
                    if (sprite.FrameIndex >= avoidAnimation.FrameCount - 1)
                    {
                        currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
                        
                        if (currentTime >= countDuration)
                        {
                            
                            currentTime = 0f;
                            sprite.PlayAnimation(idleAnimation);
                        }
                    }
                }

                if (sprite.Animation == attackAnimation)
                {
                    isDestroy = true;
                    if (sprite.FrameIndex >= attackAnimation.FrameCount - 1)
                    {
                        currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

                        if (currentTime >= countDuration)
                        {
                            
                            currentTime = 0f;
                            sprite.PlayAnimation(idleAnimation);
                        }
                    }
                }
                
            }
        }

        bool timerpress=false;

        public void PlayerKeysDown(GameTime gameTime, InputSystem input)
        {
            if (input.Avoid)
            {
                sprite.PlayAnimation(avoidAnimation);
                isIdle = false;
            }
            else if (input.Destroy)
            {
                sprite.PlayAnimation(attackAnimation);
                isIdle = false;
            }
            else
            {
                isAvoid = false;
                isDestroy = false;
                isIdle = true;
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
                //pastKey = presentKey;
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
