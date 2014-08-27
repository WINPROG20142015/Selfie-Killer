using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace selfiekiller_beta
{
    class Background
    {
        public Texture2D[] Textures
        {
            get;
            private set;
        }

        public float ScrollRate
        {
            get;
            private set;
        }
        public Background(ContentManager content, string basePath, float scrollRate)
        {
            Textures = new Texture2D[3];

            for (int i = 0; i < 3; i++)
            {
                Textures[i] = content.Load<Texture2D>(basePath + "_0");
            }

            ScrollRate = scrollRate;
        }

        public void Draw(SpriteBatch spritebatch, float cameraPosition)
        {
            int segmentWidth = Textures[0].Width;

            float x = cameraPosition * ScrollRate;
            int leftSegment = (int)Math.Floor(x / segmentWidth);
            int rightSegment = leftSegment + 1;
            x = (x / segmentWidth - leftSegment) * -segmentWidth;

            spritebatch.Draw(Textures[leftSegment % Textures.Length], new Vector2(x, 0.0f), Color.White);
            spritebatch.Draw(Textures[rightSegment % Textures.Length], new Vector2(x + segmentWidth, 0.0f), Color.White);
        }

    }
}
