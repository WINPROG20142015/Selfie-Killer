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
    public class Enemy
    {
        ContentManager content;
        string enemyName;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(-10, 0);

        public bool isVisible = true;

        public Rectangle localRect;

        public Enemy(ContentManager content, Vector2 position, string enemyName)
        {

            this.content = content;
            this.enemyName = enemyName;
            this.position = position;

            Load();
        }

        public void Load()
        {
            texture = content.Load<Texture2D>(enemyName);

            localRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update()
        {
            
            position += velocity;
            
            if (position.X < 0 - texture.Width)
                isVisible = false;

            localRect.X = (int)position.X;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
