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

namespace Relica.Actives
{
    class Reroll : ActiveItem
    {
        public const int id = 1;
        public Reroll(int x, int y) : base(6,  "Ever-shifting Dice", "Reroll reality with your active key", x, y, TypeCooldown.RoomCooldown, TypeIcon.Reroll)
        {
            CurrTextureIDX = 1;
        }
        public override void ActiveUse()
        {
           if (cooldownReady() && Player.player.level.GetType() == typeof(Treasure_room))
            {
                Treasure_room curRoom = (Treasure_room)Player.player.level;
                curRoom.reroll();
                resetCooldown();
            }
        }
    }
}
