﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace selfiekiller_beta
{
    public class Animation
    {
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        public int FrameWidth
        {
            get { return Texture.Height; }
        }

        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }
    }
}
