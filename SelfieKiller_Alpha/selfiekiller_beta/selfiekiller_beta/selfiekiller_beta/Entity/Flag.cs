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
    public class Flag
    {
        ContentManager content;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(-10, 0);
        public Rectangle localRect;

        public Flag(ContentManager content, Vector2 position)
        {
            this.content = content;
            this.position = position;

            Load();
        }

        public void Load()
        {
            texture = content.Load<Texture2D>("flags/flag1");
            localRect = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public void Update()
        {
            position += velocity;
            localRect.X = (int)position.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
