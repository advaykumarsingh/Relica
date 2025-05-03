using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.Passives
{
    public class Gas_Powered_Thrusters : PassiveItem
    {
        public const int id = 2;
        public Gas_Powered_Thrusters(int x, int y) : base( "Gas Powered Thursters", "Increase your drone speed by a large amount", x, y)
        {
            CurrTextureIDX = 1;
        }
        public override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.MaxMoveSpeed] += (int)(Player.player.stats[(int)Player.stat.MaxMoveSpeed] * .5);
            Player.player.stats[(int)Player.stat.MaxMoveSpeed] = Math.Min(50, Player.player.stats[(int)Player.stat.MaxMoveSpeed]);
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

