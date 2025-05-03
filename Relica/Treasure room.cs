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
using System.IO;

namespace Relica
{
    public class Treasure_room : Level
    {
        public static int passivePedestal = 50;
        public static int activePedestal = 51;
        public List<Item> itemsInRoom;
        public Treasure_room(String path) : base(path)
        {
            itemsInRoom = new List<Item>();
            for (int r = 0; r < floorGrid.GetLength(0); r++)
            {
                for (int c = 0; c < floorGrid.GetLength(1); c++)
                {
                    if (floorGrid[r, c].id == passivePedestal)
                    {
                        Item item = GameItemManager.GeneratePassiveItem(r * 32 + 16, c * 32 + 16);
                        //itemsInRoom.Add(item);
                    }

                    else if (floorGrid[r, c].id == activePedestal)
                    {
                        Item item = GameItemManager.GenerateActiveItem(r * 32 + 16, c * 32 + 16);
                        //itemsInRoom.Add(item);

                    }
                }
            }

        }
        public bool closestItem(Item i)
        {
            double distBetween = i.distFromPlayer();
            Console.WriteLine(itemsInRoom == null);
            foreach (Item temp in itemsInRoom)
            {
                if (temp != i && temp.distFromPlayer() < distBetween)
                    return false;
            }
            return true;
        }
        public new void update()
        {
            if (itemsInRoom.Count >0)
            {
                for (int i = 0; i < itemsInRoom.Count; i++)
                {
                    itemsInRoom[i].update();

                }

            }
            Player.player.update();
        }
        public void reroll()
        {
            if (itemsInRoom.Count > 0)
            {
                for (int i = 0; i < itemsInRoom.Count;i++)
                {
                    if (itemsInRoom[i].GetType().IsSubclassOf(typeof(PassiveItem)))
                        itemsInRoom[i] = GameItemManager.GeneratePassiveItem((int)itemsInRoom[i].X, (int)itemsInRoom[i].Y);
                    else
                        itemsInRoom[i] = GameItemManager.GenerateActiveItem((int)itemsInRoom[i].X, (int)itemsInRoom[i].Y);
                }
            }
        }
        public void clear()
        {
            //itemsInRoom = null;
            itemsInRoom.Clear();
        }
        public void switchActives(ActiveItem a)
        {
            ActiveItem temp = Player.player.actives[0];
            Player.player.actives[0] = a;
            foreach (Item i in itemsInRoom)
            {
                itemsInRoom.Remove(i);
            }
            temp.X = a.X;
            temp.Y = a.Y;
            temp.RectangleSync();
            temp.collected = false;
            itemsInRoom.Add(temp);
        }
        public new void draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            
            base.draw(gameTime,spriteBatch);
            if (itemsInRoom.Count > 0)
                foreach (Item i in itemsInRoom)
                {
                    i.draw(gameTime, spriteBatch);
                }
        }
    }
}
