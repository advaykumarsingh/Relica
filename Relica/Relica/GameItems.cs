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
    /// <summary>
    /// Stores all the possible Items in the game
    /// Three main groups of items: stat-upgrades, mods, and specials
    /// Item types: Movement, Damage, Special, Miscellaneous, and Health
    /// and initialize them as well.
    /// </summary>
    class GameItems
    {
        public static ItemManager ItemManager;

        public static Dictionary<int, Item> movementItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> damageItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> specialItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> miscItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> healthItems = new Dictionary<int, Item>();
        public static List<Item> generatedRoomItems = new List<Item>();
        private static Random random = new Random();
        public static Texture2D HUDspriteSheet;
        public static Texture2D SpecialSymbolsSpriteSheet;
        public static Texture2D TreasureRoomUpgradesSpriteSheet;

        public GameItems(Texture2D hudSheet, Texture2D ssSheet, Texture2D tRoomSheet)
        {
            movementItems = new Dictionary<int, Item>();
            damageItems = new Dictionary<int, Item>();
            specialItems = new Dictionary<int, Item>();
            miscItems = new Dictionary<int, Item>();
            healthItems = new Dictionary<int, Item>();
            HUDspriteSheet = hudSheet;
            SpecialSymbolsSpriteSheet = ssSheet;
            TreasureRoomUpgradesSpriteSheet = tRoomSheet;
            generatedRoomItems = new List<Item>();
            random = new Random();
        }

        /// <summary>
        /// creates all game items and assigns each an ID

        /// </summary>
        public static void createItems()
        {

            healthItems.Add(1, new PassiveItem(
                p => p.health =100, 
                p => p.health = p.previousHealth,
                1, // ID of category, use to determine what image to display on HUD (1: health/miscalennous, 2: movement, 3: Damage, 4: Special)
                "Health Refill", // Name
                "Restore full health", // Description
                new Rectangle(100, 100, 50, 50), // Rectangle
                TreasureRoomUpgradesSpriteSheet, // Corresponsing Sprite sheet
                80, //sprite width 
                80, //sprite height
                0,  //sprite padding
                4  //sprites per line
            ));
            healthItems[1].CurrTextureIDX = 2;

            movementItems.Add(1, new PassiveItem(
                p => p.MaxSpeed += p.MaxSpeed/2, 
                p => p.MaxSpeed = p.previousMaxSpeed,
                2,
                "Speed up", 
                "Move faster by 50%", 
                new Rectangle(200, 100, 50, 50), 
                TreasureRoomUpgradesSpriteSheet, 
                80, 
                80, 
                0,  
                4  
            ));
            movementItems[1].CurrTextureIDX = 1;

            movementItems.Add(2, new PassiveItem(
                p => p.dashesAvaliable += 1,
                p => p.dashesAvaliable = p.previousDashAvailable,
                2,
                "Max dashes", 
                "Increase Max Dashes by 1",
                new Rectangle(300, 100, 50, 50), 
                TreasureRoomUpgradesSpriteSheet, 
                80, 
                80, 
                0, 
                4  
            ));
            movementItems[2].CurrTextureIDX = 1;

            movementItems.Add(3, new PassiveItem(
                p => p.dashCoolDown -= 60,
                p => p.dashCoolDown = p.previousDashCoolDown,
                2, 
                "Dash Cool Down", 
                "Decrease Dash Cooldown by 1 second", 
                new Rectangle(400, 100, 50, 50), 
                TreasureRoomUpgradesSpriteSheet, 
                80, 
                80, 
                0,  
                4  
            ));
            movementItems[3].CurrTextureIDX = 1;


            damageItems.Add(1, new PassiveItem(
                p => p.bulletSpeed *= 2, 
                p => p.bulletSpeed = p.previousBulletSpeed,
                3, 
                "bullet speed", 
                "Increase bullet speed", 
                new Rectangle(500, 100, 50, 50), 
                TreasureRoomUpgradesSpriteSheet, 
                80, 
                80, 
                0,  
                4  
            ));
            damageItems[1].CurrTextureIDX = 0;

            damageItems.Add(2, new ActiveItem(
                p => p.FireSplitShot(p.mouse.X, p.mouse.Y, p.Rectangle.X + p.Rectangle.Width / 2, p.Rectangle.Y + p.Rectangle.Height / 2),
                60f,
                3,
                "split shot",
                "Fire three bullets in a cone shape, " + "\n" + "recharges every 1 second",
                new Rectangle(700, 100, 50, 50),
                TreasureRoomUpgradesSpriteSheet,
                80,
                80,
                0,
                4
            ));
            damageItems[2].CurrTextureIDX = 0;

            specialItems.Add(1, new PassiveItem(
                p => p.rerolls = 1,
                p => p.rerolls = 0, 
                4,
                "Reroll",
                "one chance to regnerate powerups in treasure room ", 
                new Rectangle(100,100,50,50), 
                TreasureRoomUpgradesSpriteSheet, 
                80,
                80,
                0,
                4));
            specialItems[1].CurrTextureIDX = 3;

        }

        public static void GenerateNormalRoomItems()
        {
            generatedRoomItems.Clear(); 

            int x = 100; //I'm starting it from 100, but modify start position to within frame

            Item healthItem = healthItems[1];
            healthItem.Rectangle = new Rectangle(x, healthItem.Rectangle.Y, healthItem.Rectangle.Width, healthItem.Rectangle.Height);
            generatedRoomItems.Add(healthItem);
            x += 100; //change increment if necessary

            List<Item> allOtherItems = new List<Item>();
            allOtherItems.AddRange(movementItems.Values);
            allOtherItems.AddRange(damageItems.Values);
            allOtherItems.AddRange(miscItems.Values);


            List<Item> randomItems = new List<Item>();
            for(int i = 0; i < 2; i++)
            {
                int num = random.Next(0, allOtherItems.Count);
                Item item = allOtherItems[num];
                while(ItemManager.activeItem?.Name.Equals(item.Name) == true)
                {
                    num = random.Next(0, allOtherItems.Count);
                    item = allOtherItems[num];
                }
                randomItems.Add(allOtherItems[num]);
                allOtherItems.RemoveAt(num);

            }

            foreach (Item item in randomItems)
            {
                item.Rectangle = new Rectangle(x, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height);
                generatedRoomItems.Add(item);
                x += 100;
            }

        }

        public static void GenerateSpecialRoomItems()
        {
            generatedRoomItems.Clear();
            int x = 100;
            Item healthItem = healthItems[1];
            healthItem.Rectangle = new Rectangle(x, healthItem.Rectangle.Y, healthItem.Rectangle.Width, healthItem.Rectangle.Height);
            generatedRoomItems.Add(healthItem);
            x += 100;
            foreach (Item item in specialItems.Values)
            {
                item.Rectangle = new Rectangle(x, item.Rectangle.Y, item.Rectangle.Width, item.Rectangle.Height);
                generatedRoomItems.Add(item);
                x += 100;
            }
        }


    }
}
