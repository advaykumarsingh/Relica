using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Relica
{
    abstract class Entity
    {
        private Rectangle entityRect;
        private Texture2D entityTxt;
        private int currTxtIdx;
        private Rectangle sourceRect;
        private double x, y;
        private int spriteWidth, spriteHeight;
        private int spritePadding, spritesPerLine;

        /// <summary>
        /// Creates a new basic Entity
        /// </summary>
        /// <param name="eRect">    Entity's World Rectangle                        </param>
        /// <param name="eTxt">     Entity's Texture                                </param>
        /// <param name="sWidth">   Width for Single Sprite within SpriteSheet      </param>
        /// <param name="sHeight">  Height for Single Sprite within SpriteSheet     </param>
        /// <param name="sPadding"> Padding between sprites in the SpriteSheet      </param>
        /// <param name="sPerLine"> Sprites per line                                </param>
        public Entity(Rectangle eRect, Texture2D eTxt, int sWidth, int sHeight, int sPadding, int sPerLine)
        {
            entityRect = eRect;
            X = eRect.X;
            Y = eRect.Y;
            if (spriteWidth == -1)
            {
                spriteWidth = eTxt.Width;
                spriteHeight = eTxt.Height;
                spritePadding = 0;
                spritesPerLine = 1;
            }
            else
            {
                spriteWidth = sWidth;
                spriteHeight = sHeight;
                spritePadding = sPadding;
                spritesPerLine = sPerLine;
            }
            entityTxt = eTxt;
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            currTxtIdx = 0;
        }
        public double X
        {
            get { return x; }
            set { x = value; entityRect.X = (int)X; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; entityRect.Y = (int)Y; }
        }
        public int Width
        {
            get { return entityRect.Width; }
            set { entityRect.Width = value; }
        }
        public int Height
        {
            get { return entityRect.Height; }
            set { entityRect.Height = value; }
        }
        public Texture2D Texture
        {
            get { return entityTxt; }
            set { entityTxt = value; }
        }
        /// <summary>
        /// Sets texture ID and source rectangle
        /// </summary>
        public int CurrTextureIDX
        {
            get { return currTxtIdx; }
            set 
            { 
                currTxtIdx = value;
                Rectangle s = SourceRect;
                s.X = spritePadding + (currTxtIdx % spritesPerLine) * (spritePadding * 2 + spriteWidth);
                s.Y = spritePadding + (currTxtIdx / spritesPerLine) * (spritePadding * 2 + spriteHeight);
                sourceRect = s;
            }
        }
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }
        public Rectangle Rectangle
        {
            get { return entityRect; }
            set { entityRect = value; }
        }
        public void RectangleSync()
        {
            entityRect.X = (int)x;
            entityRect.Y = (int)y;
        }
    }
}

