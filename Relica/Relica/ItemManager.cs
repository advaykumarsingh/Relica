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
    class ItemManager
    {
        public List<PassiveItem> passiveItems;
        public ActiveItem activeItem;

        public Player player;

        public ItemManager(Player player)
        {
            this.player = player;
            passiveItems = new List<PassiveItem>();
            
            activeItem = (ActiveItem) GameItems.damageItems[2];
        }

        public void AddItem(Item item)
        {

            item.OnPickup();
            if (item is PassiveItem passive)
            {
                passiveItems.Add(passive);
                item.ApplyEffect(player);
            }
            else if (item is ActiveItem active)
                activeItem = active;
            player.itemsPickedUp+=1;
            Console.WriteLine(player.itemsPickedUp);


        }

        public void RemoveItem(Item item)
        {
            if (player.itemsPickedUp <= 0)
                return;
            if (item is PassiveItem passive)
                passiveItems.Remove(passive);
            else if (item is ActiveItem active && activeItem == active)
                activeItem = null;
            item.OnRemove(player);

            Console.WriteLine(player.itemsPickedUp);



        }

        public void Update()
        {
            activeItem?.Update();
        }

        public void UseActiveItem()
        {
            activeItem?.ApplyEffect(player);
        }

        public void updateRoomItems()
        {
            for(int i = 0; i < GameItems.generatedRoomItems.Count; i++)
            {
                if (GameItems.generatedRoomItems[i].Rectangle.Intersects(player.Rectangle))
                {
                    GameItems.generatedRoomItems[i].ShowDescription = true;
                }
                else
                {
                    GameItems.generatedRoomItems[i].ShowDescription = false; ;

                }
            }
        }

        public void pickUpItems()
        {

            for (int i = GameItems.generatedRoomItems.Count - 1; i > -1; i--)
            {
                if (GameItems.generatedRoomItems[i].Rectangle.Intersects(player.Rectangle))
                {
                    
                    Item item = GameItems.generatedRoomItems[i];
                    GameItems.generatedRoomItems.Remove(GameItems.generatedRoomItems[i]);
                    AddItem(item);

                }
            }
        }



        public void DrawItems()
        {
            Game1.spriteBatch.DrawString(Game1.font, "You have one choice", new Vector2(100, 0), Color.Black);
            Console.WriteLine(passiveItems.Count);
            Game1.spriteBatch.DrawString(Game1.font, "Press P to drop item", new Vector2(100, Game1.screenHeight - 70), Color.Green);
            foreach (Item item in GameItems.generatedRoomItems)
            {
                item.Draw(Game1.spriteBatch, Game1.font);
            }
        }

        public void ClearAll()
        {
            foreach (var item in passiveItems)
                item.OnRemove(player);

            activeItem?.OnRemove(player);

            passiveItems.Clear();
            activeItem = null;
        }
    }
}
