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
        public Cursor(Rectangle eRect, Texture2D eTxt) : base(eRect, eTxt, 13 * 4, 13 * 4, 0, 2)
        {
            
        }

        public void Update(int timer)
        {
            MouseState mouse = Mouse.GetState();
            X = mouse.X - Rectangle.Width / 2;
            Y = mouse.Y - Rectangle.Height / 2;

            if (mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
            {
                CurrTextureIDX = 1;
            }
            else
                CurrTextureIDX = 0;
        }

    }
}
