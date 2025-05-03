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
    public class ControllableEntity: Entity
    {
        public Vector2 input;
        public ControllableEntity (Rectangle eRect,Texture2D eTxt) : base(eRect,eTxt)
        {
            velocity = new Vector2(0, 0);

        }

        public override void update()
        {
        }
        public void move()
        {

            input = Vector2.Zero;
            xCollision = false;
            yCollision = false;
            if (Entity.curKey.IsKeyDown(Keys.A))
                input.X -= 1;
            if (Entity.curKey.IsKeyDown(Keys.D))
                input.X += 1;
            if (Entity.curKey.IsKeyDown(Keys.W))
                input.Y -= 1;
            if (Entity.curKey.IsKeyDown(Keys.S))
                input.Y += 1;
            RectangleSync();
        }

        public bool MoveWithCollision(bool resetVelocity)
        {

            X += velocity.X;
            Y += velocity.Y;

            bool collided = CheckCollision(resetVelocity);

            return collided;
        }



        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.draw(gameTime, spriteBatch);
        }
    }
}
