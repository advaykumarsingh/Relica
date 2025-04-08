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
    class Bullet : Entity
    {
        Vector2 velocity;
        double rotation;
        Boolean isAlive;

        public Bullet(Rectangle eRect, Vector2 vel, double rot) : base (eRect, null, -1, -1, -1, 1)
        {
            velocity = vel;
            rotation = rot;
            isAlive = true;
        }

        public void Update()
        {
            X += velocity.X;
            Y += velocity.Y;
            if (ToDestroy())
                isAlive = false;
        }

        public Boolean ToDestroy()
        {
            Boolean destroys = false;
            //TODO - Manage out-of-bounds with level width and height
            //TODO - Collisions with enemies and walls
            return destroys;
        }
    }
}
