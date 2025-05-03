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
    class Cursor : Entity
    {
        public static Texture2D cursorTxt;
        public Cursor(Rectangle eRect, Texture2D eTxt) : base(eRect, eTxt, 13 * 4, 13 * 4, 0, 2)
        {

        }

        

        public override void update()
        {
            X = Entity.curMouse.X - eRect.Width / 2;
            Y = Entity.curMouse.Y - eRect.Height / 2;

            if (Entity.curMouse.LeftButton == ButtonState.Pressed)
            {
                CurrTextureIDX = 1;
            }
            else
                CurrTextureIDX = 0;
        }

        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(base.eTxt, base.eRect, base.sourceRect, Color.White);//cursor.SourceRect
        }

    }
}
