using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.PlayerItems.EnemyItems
{
    class StickierSlime: EnemyItem
    {
        public const int id = 2;
        public static new List<Type> acceptedTypeOfEnemy;
        public float slowDuration = 1f;
        public float slowTimer = 0;
        public bool isPlayerSlowed = false;
        public StickierSlime(int x, int y) : base("Sticker Slime", "Temporarily slows down the player when hit by the slime", x, y)
        {
            acceptedTypeOfEnemy = new List<Type>();
            acceptedTypeOfEnemy.Add(typeof(Slime));
        }

        public override void onDashUse()
        {
        }

        public override void onFireUse()
        {
        }

        public override void onHitEnemy()
        {
            if (!isPlayerSlowed)
            {
                Player.player.velocity *= 0.5f;
                isPlayerSlowed = true;
                slowTimer = slowDuration;
            }

        }

        public override void update()
        {
            if (isPlayerSlowed)
            {
                slowTimer -= (float)Game1.totalGameTime.ElapsedGameTime.TotalSeconds;
                if (slowTimer <= 0)
                {
                    Player.player.velocity *= 2f; // restore original speed
                    isPlayerSlowed = false;
                }
            }
        }
        public override void onMove()
        {
        }

        public override void OnPickup()
        {

        }

        public override void onRoomClear()
        {
        }

        public override void onSpawn()
        {
        }

        public override void onTakeDamage(Bullet b)
        {
        }

    }
}
