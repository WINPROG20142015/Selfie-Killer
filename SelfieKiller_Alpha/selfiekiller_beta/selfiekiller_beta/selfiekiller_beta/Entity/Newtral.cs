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
    public class Newtral
    {
        ContentManager content;
        string newtralName;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity = new Vector2(-13, 0);

        public bool isVisible = true;

        public Rectangle localRect;

        public Newtral(ContentManager content, Vector2 position, string newtralName)
        {

            this.content = content;
            this.newtralName = newtralName;
            this.position = position;

            Load();
        }

        public void Load()
        {
            texture = content.Load<Texture2D>(newtralName);
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
