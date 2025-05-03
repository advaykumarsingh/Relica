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
    class Slime : Enemy
    {
        public const int id = 0;
        public static Texture2D slimeSheet;
        public static int maxSpeed = 5;
        public static double size = 1;
        public static int radius = 30;
        public static int hp = 70;
        public static int dmg = 10;
        int moveCooldown;
        int dashTimer;
        int attackCooldown;
        int attackStartupDelay;
        bool isPreparing;
        int attackTimer;
        Vector2 velocity;
        AttackIndicator target;
        public Slime(int x, int y) : base(id, x, y)
        {
            moveCooldown = 0;
            dashTimer = 0;
            attackCooldown = 0;
            attackTimer = 0;
            attackStartupDelay = 60;
            velocity = new Vector2(0);
            target = new AttackIndicator(new Rectangle(0, 0, radius, radius), slimeSheet, 15);
        }



        public override void update()
        {
            if (distFromPlayer() > 150 && !isPreparing)
            {
                if (moveCooldown <= 0)
                {
                    velocity = distBetweenCenter(enemyController.player, this);
                    velocity.Normalize();
                    velocity *= maxSpeed;
                    dashTimer = 20;
                    moveCooldown = 120;
                }
                moveCooldown--;
            }
            else if (dashTimer < 0 && (attackCooldown <= 0 || isPreparing))
            {
                isPreparing = true;
                if (attackStartupDelay == 60)
                {
                    target.moveToPoint(enemyController.CenteredPointForRectangle(eRect.Width, eRect.Height));
                }

                if (attackStartupDelay == 0)
                {
                    velocity = distBetweenCenter(target, this);
                    velocity /= 10f;
                    isPreparing = false;
                    attackStartupDelay = 60;
                    attackCooldown = 60;
                    attackTimer = 10;
                }
                else
                    attackStartupDelay--;
            }
            if (dashTimer < 0 && attackTimer < 0 && velocity.LengthSquared() > .25f)
            {
                velocity *= .5f;
            }
            else
            {
                velocity = Vector2.Zero;
            }
            X += velocity.X;
            Y += velocity.Y;
            dashTimer--;
            attackTimer--;
            RectangleSync();
        }
        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(slimeSheet, eRect, Color.White);
            if (isPreparing)
                target.draw(gameTime, spriteBatch);
        }
    }
}

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Relica
//{
//    class Slime : Enemy
//    {
//        public static int id = 1;
//        public static Texture2D slimeSheet;
//        public static int maxDashSpeed = 5;
//        public static double size = 1;
//        
//        int moveCooldown;
//        int dashTimer;
//        int attackCooldown;
//        bool isAttacking;
//        Vector2 velocity;
//        public Slime(int x,int y):base(id,x,y)
//        {
//            moveCooldown = 0;
//            dashTimer = 0;
//            attackCooldown = 0;
//            velocity = new Vector2(0);
//        }



//        public override void update()
//        {
//            moveCooldown--;
//            if (distFromPlayer() > 150)
//            {
//                if (moveCooldown <= 0)
//                {
//                    velocity = distBetweenCenter(enemyController.player, this);
//                    velocity.Normalize();
//                    dashTimer = 20;
//                    moveCooldown = 120;
//                }
//            }

//            if (dashTimer >= 0)
//            {
//                velocity *= maxDashSpeed;
//                X += velocity.X;
//                Y += velocity.Y;

//            }
//            RectangleSync();
//        }
//        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            this.draw(gameTime, spriteBatch, new Rectangle(0, 0, 132, 132));
//        }
//    }
//}
