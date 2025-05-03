using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.Passives
{
    public class Deeper_Reserves : PassiveItem
    {
        public const int id = 3;
        public Deeper_Reserves(int x, int y) : base( "Deeper Reserves", "Increase your fuel storage to gain an extra dash", x, y)
        {
            CurrTextureIDX = 1;
        }
        public override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.MaxDashes] += 1;
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

