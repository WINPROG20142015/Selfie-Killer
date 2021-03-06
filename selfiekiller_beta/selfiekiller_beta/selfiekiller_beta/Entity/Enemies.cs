﻿using Microsoft.Xna.Framework;
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
        public Vector2 velocity = new Vector2(-10, 0);

        public bool isVisible = true;

        public Enemies(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
        }
        public void Update()
        {
            position += velocity;
            
            if (position.X < 0 - texture.Width)
                isVisible = false;
        }
        public void Load()
        { 

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
