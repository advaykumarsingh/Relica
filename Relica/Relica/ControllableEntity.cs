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
    class ControllableEntity : Entity
    {
        public  KeyboardState oldKey;
        public GamePadState oldPad;
        public KeyboardState curKey;
        public GamePadState curPad;
        public float maxSpeed;
        public float maxDashSpeed;
        private float acceleration;
        private float deceleration;
        private Vector2 input;
        private Vector2 velocity;

        private double dashRefill;
        private double dashTime;
        public double dashesAvaliable;
        private Boolean isDashing;

        public double dashCoolDown;

        public ControllableEntity(Rectangle eRect, Texture2D eTxt) : base(eRect, eTxt, -1, -1, -1, 1)
        {
            oldKey = Keyboard.GetState();
            oldPad = GamePad.GetState(PlayerIndex.One);
            maxSpeed = 5;
            maxDashSpeed = 20;
            acceleration = 0.6f;
            deceleration = 0.3f;
            velocity = new Vector2(0, 0);

            isDashing = false;
            dashesAvaliable = 2;
            dashRefill = 0;
            dashTime = 0;
            dashCoolDown = 60 * 5;
        }
        public void move(int timer)
        {
             curKey = Keyboard.GetState();
             curPad = GamePad.GetState(PlayerIndex.One);

            input = Vector2.Zero;

            dashRefill--;
            if (dashesAvaliable > 0)
                if (curKey.IsKeyDown(Keys.Space) && !oldKey.IsKeyDown(Keys.Space) && !isDashing)
                {
                    isDashing = true;
                    dashRefill = timer + dashCoolDown;
                    dashTime = 10;
                    dashesAvaliable--;
                }

            if (dashRefill < timer)
            {
                dashesAvaliable = 3;
            }

            dashTime--;
            if (dashTime < 0)
                isDashing = false;

            if (!isDashing)
                MoveRegular(curKey);
            else
                MoveDash();

            X += velocity.X;
            Y += velocity.Y;


        }

        public void MoveRegular(KeyboardState curKey)
        {
            if (curKey.IsKeyDown(Keys.A))
                input.X -= 1;
            if (curKey.IsKeyDown(Keys.D))
                input.X += 1;
            if (curKey.IsKeyDown(Keys.W))
                input.Y -= 1;
            if (curKey.IsKeyDown(Keys.S))
                input.Y += 1;
            if (input != Vector2.Zero)
                input.Normalize();
            velocity += input * acceleration;

            if (velocity.Length() > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }

            if (input.X == 0)
            {
                if (velocity.Length() > deceleration)
                {
                    Vector2 vel = velocity;
                    vel.Normalize();
                    velocity.X -= vel.X * deceleration;
                }
                else
                    velocity.X = 0;
            }
            if (input.Y == 0)
            {
                if (velocity.Length() > deceleration)
                {
                    Vector2 vel = velocity;
                    vel.Normalize();
                    velocity.Y -= vel.Y * deceleration;
                }
                else
                    velocity.Y = 0;
            }
        }


        
        public void MoveDash()
        {
            if (velocity == Vector2.Zero)
            {
                velocity.X = -(float)(Mouse.GetState().X - (X + Width / 2));
                velocity.Y = -(float)(Mouse.GetState().Y - (Y + Height / 2));
            }

            Vector2 vel = velocity;
            vel.Normalize();
            velocity = vel * maxDashSpeed;

            if (velocity.Length() > maxDashSpeed)
            {
                velocity.Normalize();
                velocity *= maxDashSpeed;
            }

            velocity -= vel * deceleration;
                
        }

        public void updateStates(KeyboardState curKey, GamePadState curPad)
        {
            oldKey = curKey;
            oldPad = curPad;
        }
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
    }
}
