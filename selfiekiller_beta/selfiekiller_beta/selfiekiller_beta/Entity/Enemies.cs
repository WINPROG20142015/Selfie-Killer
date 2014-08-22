using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace selfiekiller_beta
{
    class Enemies
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(-2, 0);

        public Rectangle enemyRect;
        public bool isVisible = true;

        public Enemies(Texture2D newTexture, Vector2 newPosition, Rectangle newRect)
        {
            texture = newTexture;
            position = newPosition;
            enemyRect = newRect;
        }

        //new code to
        public void Update(GraphicsDevice graphics)
        {
            position += velocity;
            if (position.X < 0 - texture.Width)

                isVisible = false;
        }
        //new code to

        public void Load()
        { 
            enemyRect = new Rectangle((int)position.X,(int)position.Y,texture.Width,texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

        }
    }
}
