using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Relica.PlayerItems.EnemyItems
{
    public class PartialImmunity : EnemyItem
    {
        public const int id = 2;
        public static new List<Type> acceptedTypeOfEnemy;

        private Dictionary<Enemy, bool> teleportUsed = new Dictionary<Enemy, bool>();
        private Random rand = new Random();

        public PartialImmunity(int x, int y) : base("Partial Immunity", "Enemy has a 30% chance of being immune to incoming damage", x, y)
        {
            acceptedTypeOfEnemy = new List<Type> { typeof(Slime) }; // Add your supported types
            rand = new Random();
        }

        public override void onTakeDamage(Bullet b)
        {
            if(rand.Next(10) < 3)
            {
                b.damage = 0;
            }
        }

        public override void OnPickup() { }
        public override void onSpawn() { }
        public override void onMove() { }
        public override void onFireUse() { }
        public override void onHitEnemy() { }
        public override void onRoomClear() { }
        public override void onDashUse() { }
    }
}
