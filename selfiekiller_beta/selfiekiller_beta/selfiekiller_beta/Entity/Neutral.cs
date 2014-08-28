using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selfiekiller_beta
{
    public class Neutral
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(-2, 0);

        public Rectangle neutralRect;
        public bool isVisible = true;

        public Neutral(Texture2D newTexture, Vector2 newPosition, Rectangle newRect)
        {
            texture = newTexture;
            position = newPosition;
            neutralRect = newRect;
        }

        //new code to
        public void Update()
        {
            position += velocity;
            if (position.X < 0 - texture.Width)

                isVisible = false;
        }
        //new code to

        public void Load()
        { 
            neutralRect = new Rectangle((int)position.X,(int)position.Y,texture.Width,texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

        }
    }
}
