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
    public class GameItemManager
    { 

        public static Dictionary<int, Item> movementItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> damageItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> specialItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> miscItems = new Dictionary<int, Item>();
        public static Dictionary<int, Item> healthItems = new Dictionary<int, Item>();
        public static List<Item> seenItems = new List<Item>();
        private static Random random = new Random();
        public static Texture2D HUDspriteSheet;
        public static Texture2D SpecialSymbolsSpriteSheet;
        public static Texture2D TreasureRoomUpgradesSpriteSheet;
        public static int spriteWidth = 80;
        public static int spriteHeight = 80;
        public static int NumActiveItem = 2;
        public GameItemManager(Texture2D hudSheet, Texture2D ssSheet, Texture2D tRoomSheet)
        {
            movementItems = new Dictionary<int, Item>();
            damageItems = new Dictionary<int, Item>();
            specialItems = new Dictionary<int, Item>();
            miscItems = new Dictionary<int, Item>();
            healthItems = new Dictionary<int, Item>();
            HUDspriteSheet = hudSheet;
            SpecialSymbolsSpriteSheet = ssSheet;
            TreasureRoomUpgradesSpriteSheet = tRoomSheet;
            seenItems = new List<Item>();
            random = new Random();
        }

        /// <summary>
        /// creates all game items and assigns each an ID

        /// </summary>
         public static PassiveItem createPassiveItem(int id, int x, int y)
        {
            switch (id)
            {
                case Passives.ScrapMetal.id:
                    return new Passives.ScrapMetal(x, y);
                case Passives.StrongerLazerz.id:
                    return new Passives.StrongerLazerz(x, y);
                case Passives.Gas_Powered_Thrusters.id:
                    return new Passives.Gas_Powered_Thrusters(x, y);
                case Passives.Deeper_Reserves.id:
                    return new Passives.Deeper_Reserves(x, y);
                case Passives.Thermionic_Cooling.id:
                    return new Passives.Thermionic_Cooling(x, y);
                case Passives.Oiling_Fluid.id:
                    return new Passives.Oiling_Fluid(x, y);


                default:
                    return null;
            }
        }
         public static ActiveItem createActiveItem(int id,int x,int y)
        {
            switch (id)
            {
                case Actives.SplitShot.id:
                    return new Actives.SplitShot(x, y);
                case Actives.Reroll.id:
                    return new Actives.Reroll(x, y);
                case Actives.FullCircleShot.id:
                    return new Actives.FullCircleShot(x, y);

                default:
                    return null;
            }
        }
        

        public static PassiveItem GeneratePassiveItem(int x, int y)
        {
            PassiveItem p = null;
            do
            {
                p = createPassiveItem(random.Next(6), x, y);
            } while (seenItems.Contains((Item)p));
            seenItems.Add(p);
            return p;
        }

        public static ActiveItem GenerateActiveItem(int x, int y)
        {
            ActiveItem a = null;
            do
            {
                a = createActiveItem(random.Next(NumActiveItem + 1), x, y);
            } while (a == Player.player.actives[0]);
            return a;

        }


    }
}
