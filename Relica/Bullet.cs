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
    public class Bullet : Entity
    {
        static public Texture2D bulletSheet;
        public Vector2 velocity;
        public float rotation;
        public bool friendly;
        public int damage;

        public Bullet(Rectangle eRect, Vector2 vel, float rot,bool friendly,int damage) : base(eRect, bulletSheet)
        {
            velocity = vel;
            rotation = rot;
            this.friendly = friendly;
            this.damage = damage;
        }

        public override void update()
        {
            X += velocity.X;
            Y += velocity.Y;
            terminate();
        }

        public void terminate()
        {
            //if (X < Level.xLevelOffset - 800 * Player.player.level.floorGrid.GetLength(0)
            //    || X > Level.xLevelOffset + 800 * Player.player.level.floorGrid.GetLength(0)
            //    || Y < Level.yLevelOffset - 800 * Player.player.level.floorGrid.GetLength(1)
            //    || Y > Level.xLevelOffset + 800 * Player.player.level.floorGrid.GetLength(1))
            //    Player.player.playerBullets.Remove(this);
        }

        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle spriteRect = new Rectangle(1, 1, 1, 1);
            spriteBatch.Draw(bulletSheet, eRect, spriteRect, Color.White, rotation, new Vector2(spriteRect.Center.X,spriteRect.Center.Y), SpriteEffects.None, 0);
        }
    }
}
