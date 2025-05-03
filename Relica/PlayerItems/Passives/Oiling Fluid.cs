using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.Passives
{
    public class Oiling_Fluid : PassiveItem
    {
        public const int id = 5;
        public Oiling_Fluid(int x, int y) : base( "Oiling Fluid", "Better oiled barrels allow your bullets to move faster", x, y)
        {
            CurrTextureIDX = 0;
        }
        public override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.BulletSpeed] += (int)(Player.player.stats[(int)Player.stat.BulletSpeed] * .2);
            Player.player.stats[(int)Player.stat.BulletSpeed] = Math.Min(100, Player.player.stats[(int)Player.stat.BulletSpeed]);
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

