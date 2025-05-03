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
    public abstract class Entity
    {
        public static KeyboardState oldKey;
        public static KeyboardState curKey;
        public static MouseState curMouse;
        public static MouseState oldMouse;
        public static Random rand = new Random();
        public static GameTime time;
        private Rectangle entityRect;
        private Texture2D entityTxt;
        public Rectangle sourceRect;

        private double x, y;
        public static Level l;
        public Vector2 velocity;
        public bool xCollision, yCollision;
        private int spriteWidth, spriteHeight;
        private int spritePadding, spritesPerLine;
        private int currTxtIdx;

        public Entity(Rectangle eRect, Texture2D eTxt, int sWidth, int sHeight, int sPadding, int sPerLine)
        {
            this.entityRect = eRect;
            this.entityTxt = eTxt;
            x = eRect.X;
            y = eRect.Y;
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
            sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            currTxtIdx = 0;
            time = Game1.totalGameTime;
        }
        public Entity(Rectangle eRect, Texture2D eTxt)
        {
            this.entityRect = eRect;
            this.entityTxt = eTxt;
            x = eRect.X;
            y = eRect.Y;


        }
        public abstract void update();

        public static void newUpdate()
        {
            curKey = Keyboard.GetState();
            curMouse = Mouse.GetState();
        }
        public static void oldUpdate()
        {
            oldKey = curKey;
            oldMouse = curMouse;
        }
        public double X
        {
            get { return x; }
            set { x = value;
                RectangleSync(); }
        }
        public double Y
        {
            get { return y; }
            set { y = value;
                RectangleSync();}
        }
        public int Width
        {
            get { return entityRect.X; }
            set { entityRect.X = value; }
        }
        public int Height
        {
            get { return entityRect.X; }
            set { entityRect.X = value; }
        }

        public Rectangle eRect
        {
            get { return entityRect; }
        }
        public Texture2D eTxt
        {
            get { return entityTxt; }
        }
        public int CurrTextureIDX
        {
            get { return currTxtIdx; }
            set
            {
                currTxtIdx = value;
                Rectangle s = sourceRect;
                s.X = spritePadding + (currTxtIdx % spritesPerLine) * (spritePadding * 2 + spriteWidth);
                s.Y = spritePadding + (currTxtIdx / spritesPerLine) * (spritePadding * 2 + spriteHeight);
                sourceRect = s;
            }
        }
        public Vector2 GetIntersectionDepth(Rectangle rectB)
        {
            float distanceX = eRect.Center.X - rectB.Center.X;
            float distanceY = eRect.Center.Y - rectB.Center.Y;
            float minDistanceX = eRect.Width/2 + rectB.Width/2;
            float minDistanceY = eRect.Height/2 + rectB.Height/2;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

            return new Vector2(depthX, depthY);
        }
        public Boolean CheckCollision(Boolean resetVelocity)
        {
            Boolean collided = false;
            int tileSize = Tile.TILE_SIZE;
            int leftTile = (eRect.Left - Level.xLevelOffset) / tileSize;
            int topTile = (eRect.Top - Level.yLevelOffset) / tileSize;

            for (int r = topTile - 1; r <= topTile + 2; r++)
            {
                for (int c = leftTile - 1; c <= leftTile + 2; c++)
                {
                    Rectangle collidingRectangle = new Rectangle(c * tileSize + 560, r * tileSize + 240, tileSize, tileSize);
                    if (l.InBounds(r, c))
                    {
                        if (l.floorGrid[r, c].Collidable)
                        {
                            if (ResolveCollision(resetVelocity,collidingRectangle))
                                collided = true;
                        }
                    }
                    else
                    {
                        if (ResolveCollision(resetVelocity,collidingRectangle))
                            collided = true;
                    }
                }
            }

            return collided;
        }

        public Boolean ResolveCollision(Boolean resetVelocity,Rectangle collidingRect)
        {
            Boolean collided = false;
            Vector2 depth = GetIntersectionDepth(collidingRect);
            if (depth != Vector2.Zero)
            {
                float absDepthX = Math.Abs(depth.X);
                float absDepthY = Math.Abs(depth.Y);

                if (absDepthY < absDepthX)
                {
                    yCollision = true;
                    Y = eRect.Y + depth.Y;
                    if (resetVelocity)
                        velocity.Y = 0;
                }
                else if (absDepthX < absDepthY)
                {
                    xCollision = true;
                    X = eRect.X + depth.X;
                    if (resetVelocity)
                        velocity.X = 0;
                }

                collided = true;
            }
            return collided;
        }

        public double FindCornerX(Rectangle rect, Vector2 vel)
        {
            return rect.Center.X + (rect.Center.X * Math.Sign(vel.X));
        }

        public double FindCornerY(Rectangle rect, Vector2 vel)
        {
            return rect.Center.Y + (rect.Center.Y * Math.Sign(vel.Y));
        }
        public Vector2 distBetweenCenter(Entity other, Entity centered)
        {
            return new Vector2((float)(other.eRect.Center.X - centered.eRect.Center.X), (float)(other.eRect.Center.Y - centered.eRect.Center.Y));
        }
        public double distFromPlayer()
        {
            return this.distBetweenCenter(Player.player, this).Length();
        }
        public static bool wasKeyPressed(Keys k)
        {
            return curKey.IsKeyDown(k) && oldKey.IsKeyUp(k);
        }
        public abstract void draw(GameTime gameTime, SpriteBatch spriteBatch);
        public void RectangleSync()
        {
            entityRect.X = (int)x;
            entityRect.Y = (int)y;
        }
        
    }
}
