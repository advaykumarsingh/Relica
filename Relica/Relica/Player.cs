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
    class Player : ControllableEntity
    {
        public ItemManager itemManager;

        public bool inTresaureRoom;
        public bool inSpecialRoom;
        //previous state variables (used when dropping an item)
        public int previousHealth;
        public int previousBulletSpeed;
        public float previousMaxSpeed;
        public double previousMaxDashSpeed;
        public double previousDashAvailable;
        public double previousDashCoolDown;
        public int rerolls;

        //num of items picked up while in a treasure room
        public int itemsPickedUp;
        public int health
        {
            get;
            set;
        }
        public int bulletSpeed;

        public MouseState mouse;
        public MouseState oldMouse;
        private Vector2 mousePos;

        Room currentRoom;

        List<Bullet> bullets;
        enum Room
        {
            Intro,
            Combat,
            Treasure,
            Special
        };

        public Player(Rectangle eRect, Texture2D eTxt) : base(eRect, eTxt)
        {
            health = previousHealth = 50;
            MaxSpeed = previousMaxSpeed = 5;
            bulletSpeed = previousBulletSpeed = 10;
            mousePos = new Vector2();
            oldMouse = Mouse.GetState();
            bullets = new List<Bullet>();
            itemManager = new ItemManager(this);
            GameItems.ItemManager = itemManager;
            //intitialize rest of previous variables
            previousMaxDashSpeed = base.maxDashSpeed;
            previousDashAvailable = base.dashesAvaliable;
            previousDashCoolDown = base.dashCoolDown;
            inTresaureRoom = false;
            inSpecialRoom = false;
            itemsPickedUp = 0;
            rerolls = 0;
            currentRoom = Room.Intro;


        }

        /// <summary>
        /// Functions - Moving; Update Shooting
        /// </summary>
        public void Update(int timer)
        {
            move(timer);
            CalcMouse();
            foreach (Bullet b in bullets)
            {
                b.Update();
            }



            if (base.curKey.IsKeyDown(Keys.NumPad1) && base.oldKey.IsKeyUp(Keys.NumPad1))
            {
                ChangeCurrentRoom(Room.Treasure);
                GameItems.GenerateNormalRoomItems();
            }
            if (base.curKey.IsKeyDown(Keys.NumPad2) && base.oldKey.IsKeyUp(Keys.NumPad2))
            {
                ChangeCurrentRoom(Room.Special);
                GameItems.GenerateSpecialRoomItems();

            }
            if (base.curKey.IsKeyDown(Keys.NumPad3) && base.oldKey.IsKeyUp(Keys.NumPad3))
            {
                ChangeCurrentRoom(Room.Intro);
            }
            if (currentRoom == Room.Special || currentRoom == Room.Treasure)
            {
                if (base.curKey.IsKeyDown(Keys.Q) && base.oldKey.IsKeyUp(Keys.Q) && itemsPickedUp == 0)
                {

                    itemManager.pickUpItems();

                }
                if (base.curKey.IsKeyDown(Keys.P) && base.oldKey.IsKeyUp(Keys.P) && itemManager.passiveItems.Count > 0)
                {
                    //remove most recently picked up passive Item
                    itemManager.RemoveItem(itemManager.passiveItems[itemManager.passiveItems.Count - 1]);
                }
                if (base.curKey.IsKeyDown(Keys.M) && base.oldKey.IsKeyUp(Keys.M) && itemManager.activeItem!=null)
                {
                    //remove most recently picked up passive Item
                    itemManager.RemoveItem(itemManager.activeItem);
                }

                //check to see if description for room item should be displayed
                itemManager.updateRoomItems();
            }
            else
            {
                UpdateVars();

            }



            updateStates(base.curKey, base.curPad);
            RectangleSync();

        }

        private void ChangeCurrentRoom(Room newRoom)
        {
            if (currentRoom != newRoom)
            {
                //room has been switched, reset itemsPicked
                UpdateVars();
            }
            currentRoom = newRoom;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.Red);
            foreach (Bullet b in bullets)
                spriteBatch.Draw(Texture, b.Rectangle, Color.Green);
            if(currentRoom == Room.Treasure || currentRoom == Room.Special)
                itemManager.DrawItems();
            spriteBatch.DrawString(Game1.font, "Passive items count: " + itemManager.passiveItems.Count + "", new Vector2(300, 300), Color.Black);
            spriteBatch.DrawString(Game1.font, "Items picked up in one room: " + itemsPickedUp + "", new Vector2(400, 350), Color.Black);
            spriteBatch.DrawString(Game1.font, "Has Active Item: " + (itemManager.activeItem != null) , new Vector2(400, 400), Color.Black);


        }

        public void CalcMouse()
        {
            mouse = Mouse.GetState();
            double xCenter = X + Width / 2;
            double yCenter = Y + Height / 2;

            if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                FireNormalBullet(mouse.X, mouse.Y, xCenter, yCenter);
            }
            
            if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released)
            {
                itemManager.UseActiveItem();
            }
            if (itemManager.activeItem?.isActive == false)
            {
                itemManager.activeItem.Update();
            }
            oldMouse = mouse;
        }

        public void FireNormalBullet(float MouseX, float MouseY, double originX, double originY)
        {

            double hyp = Math.Sqrt(Math.Pow(originX - MouseX, 2) + Math.Pow(originY - MouseY, 2));
            double numUpdates = hyp / bulletSpeed;
            Vector2 vel = new Vector2((float)((MouseX - originX) / numUpdates), (float)((MouseY - originY) / numUpdates));
            double rot = Math.Atan2(originY - MouseX, originX - MouseY);

            bullets.Add(new Bullet(new Rectangle((int)originX, (int)originY, 5, 5), vel, rot));
        }

        public void FireSplitShot(float MouseX, float MouseY, double originX, double originY)
        {

            Vector2 mouseVec = new Vector2(MouseX, MouseY);
            Vector2 playerVec = new Vector2((float)originX, (float)originY);
            Vector2 direction = mouseVec - playerVec;
            direction.Normalize();

            float spreadAngle = MathHelper.ToRadians(5); // 5 degrees cone

            Vector2[] directions = new Vector2[3]
            {
                direction,//center
                Vector2.Transform(direction, Matrix.CreateRotationZ(-spreadAngle)), //left
                Vector2.Transform(direction, Matrix.CreateRotationZ(spreadAngle)) //right
            };
            foreach (Vector2 dir in directions)
            {
                Vector2 velocity = dir * bulletSpeed;
                double rot = Math.Atan2(-dir.Y, -dir.X); 
                bullets.Add(new Bullet(new Rectangle((int)originX, (int)originY, 5, 5), velocity, rot));
            }

        }

        private void UpdateVars() //only call method when in combat, not during treasure room
        {
            previousBulletSpeed = bulletSpeed;
            previousDashAvailable = dashesAvaliable;
            previousDashCoolDown = dashCoolDown;
            previousHealth = health;
            previousMaxDashSpeed = maxDashSpeed;
            previousMaxSpeed = maxSpeed;
            itemsPickedUp = 0;

        }
    }
}
