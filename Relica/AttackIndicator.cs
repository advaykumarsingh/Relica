using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Relica
{
    class AttackIndicator : Entity
    {
        int timer;
        int maxTimer;
        Color indicator;
        static readonly Color baseColor = new Color(191, 191, 191);
        double green = baseColor.G;
        double blue = baseColor.B;
        public AttackIndicator(Rectangle r, Texture2D txt, int maxTimer) : base(r, txt)
        {
            this.timer = maxTimer;
            this.maxTimer = maxTimer;
            indicator = baseColor;
        }
        public Rectangle attackArea
        {
            get { return eRect; }
        }
        public AttackIndicator moveToPoint(int x, int y)
        {
            X = x;
            Y = y;
            return this;
        }
        public AttackIndicator moveToPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
            return this;
        }
        public AttackIndicator moveBy(int x, int y)
        {
            X += x;
            Y += y;
            return this;
        }

        public override void update()
        {
            green -= 191f / (float)maxTimer;
            indicator.G = (byte)green;
            blue -= 191f / (float)maxTimer;
            indicator.B = (byte)blue;
            timer--;
            if (timer == 0)
            {
                timer = maxTimer;
                indicator = baseColor;
            }
            RectangleSync();
        }

        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(eTxt, eRect, indicator);
        }
    }
}
