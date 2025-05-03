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

namespace Relica.Passives
{
    public class StrongerLazerz : PassiveItem
    {
        public const int id = 0;
        public StrongerLazerz (int x,int y) : base("Stronger Lazers", "Upgrade your lasers to deal extra damage",x,y)
        {
            CurrTextureIDX = 0;
        }
        public  override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.Dmg] += 5;
        }

        public override void onTakeDamage()
        {
            
        }

        public override void onKillEnemy()
        {
            
        }

        public override void onActiveUse()
        {
            
        }

        public override void onDashUse()
        {
            
        }

        public override void onMove()
        {
            
        }

        public override void onHitEnemy()
        {
            
        }

        public override void onSpawn()
        {
            
        }

        public override void onPickupOtherItem()
        {
            
        }

        public override void onPlaceItem()
        {
            
        }
        public override void onRoomClear()
        {

        }
    }
}
