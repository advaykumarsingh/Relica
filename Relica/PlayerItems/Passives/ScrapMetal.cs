using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.Passives
{
    public class ScrapMetal : PassiveItem
    {
        public const int id = 1;
        public ScrapMetal(int x, int y) : base("Scrap Metal", "Heal your drone by a moderate Amount", x, y)
        {
            CurrTextureIDX = 2;
        }
        public override void onFireUse()
        {
        }

        public override void OnPickup()
        {
            Player.player.stats[(int)Player.stat.Hp] = 100;
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
