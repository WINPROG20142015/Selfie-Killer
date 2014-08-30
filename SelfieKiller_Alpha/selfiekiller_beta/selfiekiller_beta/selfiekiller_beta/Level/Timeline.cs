using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace selfiekiller_beta
{
   
    class Timeline
    {

        //TIMELINE
        Texture2D timelineTexture;
        public Rectangle timelineRectangle;
        Vector2 timelinePosition;

        public int health;

        public Timeline(Texture2D newTexture, Vector2 newPosition, int newHealth)
        {
            timelineTexture = newTexture;
            timelinePosition = newPosition;
            health = newHealth;
        }
        public void Update()
        {
            timelineRectangle = new Rectangle((int)timelinePosition.X,(int)timelinePosition.Y,timelineTexture.Width,timelineTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
                spriteBatch.Draw(timelineTexture, timelineRectangle, Color.White);
                
        }

    }
}
