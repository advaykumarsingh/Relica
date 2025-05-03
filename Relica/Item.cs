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
    public abstract class Item : Entity
    {
        public static Keys pickupKey = Keys.Q;
        public static int floatDist = 10;
        public static int itemSize = 48;
        public static SpriteFont font1;
        public string itemName;
        public string itemDescription;
        public bool collected;
        public Color textColor;
        public static Treasure_room room;
        public static Vector2 textVect = new Vector2(Level.xLevelOffset + 400 / 2, Level.yLevelOffset + 400 - 40);
        public Item( string name,string desc, int x,int y,Texture2D txt) : base (new Rectangle(x - itemSize /2 ,y - itemSize / 2,itemSize,itemSize),txt, 80,80,0,4)
        {
            this.itemName = name;
            this.itemDescription = desc;
            collected = false;
            font1 = Game1.spriteFont;
        }
        private void drawDescription()
        {
            
        }
        public override void update()
        {
            //Y += floatDist * Math.Sin(Entity.time.ElapsedGameTime.TotalSeconds);
            if (distFromPlayer() < 200)
            {
                textColor.A = (Byte)(255 * Math.Pow(Math.E, -Math.Pow(distFromPlayer() / 50, 2)));
            }
            if (distFromPlayer() < 150 && wasKeyPressed(pickupKey) && room.closestItem(this)  )
            {
                collected = true;
                if (this.GetType().IsSubclassOf(typeof(PassiveItem)))
                {
                    Player.player.passiveItems.Add((PassiveItem)this);
                    Player.player.passiveItems[Player.player.passiveItems.Count - 1].OnPickup();
                    room.clear();
                }
                else
                {
                    room.switchActives((ActiveItem)this);
                }
                
            }
        }
        public override void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!collected)
            {
                spriteBatch.Draw(eTxt, eRect, sourceRect, Color.White);
                if (textColor.A > 5 && room.closestItem(this))
                {
                    spriteBatch.DrawString(font1, itemDescription, textVect, textColor);
                }
            }
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Item))
                return this == (Item)obj;
            else
                return false;
        }
        public static bool operator ==(Item i1, Item i2)
        {
            if ((i1 is null && !(i2 is null)) || (i2 is null && !(i1 is null)))
                return false;
            if (i1 is null && i2 is null)
                return true;
            return (i1.GetType() == i2.GetType());
        }
        public static bool operator != (Item i1,Item i2)
        {
            return !(i1 == i2);
        }
    }
}
