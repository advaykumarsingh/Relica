using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.Passives
{
    public class Thermionic_Cooling : PassiveItem
    {
        public const int id = 4;
        public Thermionic_Cooling(int x, int y) : base( "Thermionic Cooling", "Faster cooling allows you to dash more often", x, y)
        {
            CurrTextureIDX = 1;
        }
        public override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.DashCooldown] -= 60;
            Player.player.stats[(int)Player.stat.DashCooldown] = Math.Max(30, Player.player.stats[(int)Player.stat.DashCooldown]);
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
