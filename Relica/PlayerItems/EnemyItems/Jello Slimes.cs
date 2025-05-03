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

namespace Relica.PlayerItems.EnemyItems
{
    class Jello_Slimes : EnemyItem
    {
        public const int id = 0;
        public static new List<Type> acceptedTypeOfEnemy;
        public Jello_Slimes(int x, int y) : base("Jello Slimes", "Bouncier Slimes have a chance to reflect bullets", x, y)
        {
            acceptedTypeOfEnemy.Add(typeof(Slime));
        }

        public override void onDashUse()
        {
        }

        public override void onFireUse()
        {
        }

        public override void onHitEnemy()
        {
        }

        public override void onMove()
        {
        }

        public override void OnPickup()
        {
            
        }

        public override void onRoomClear()
        {
        }

        public override void onSpawn()
        {
        }

        public override void onTakeDamage(Bullet b)
        {
            if (rand.Next(11) < 2)
            {
                b.friendly = false;
                b.rotation += MathHelper.ToRadians(180);
                b.velocity *= -.5f;
                if (b.velocity.Length() < 1.0f)
                {
                    b.velocity = Vector2.Normalize(b.velocity) * 1.0f;  // Set a minimum velocity
                }
            }
        }

        public override void update()
        {

        }
    }
}