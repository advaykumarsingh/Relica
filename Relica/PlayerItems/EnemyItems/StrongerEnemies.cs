using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica.PlayerItems.EnemyItems
{
    public class StrongerEnemies : EnemyItem
    {
        public const int id = 3;
        public static new List<Type> acceptedTypeOfEnemy;
        public StrongerEnemies(int x, int y) : base("Stronger Enemies", "Enemies gain more health", x, y) {
            acceptedTypeOfEnemy.Add(typeof(Enemy));
        }

        public override void onDashUse()
        {
            throw new NotImplementedException();
        }

        public override void onFireUse()
        {
        }

        public override void onHitEnemy()
        {
            throw new NotImplementedException();
        }

        public override void onMove()
        {
            throw new NotImplementedException();
        }

        public override void OnPickup()
        {
            foreach (var key in enemyController.idToHp.Keys.ToList()) // .ToList() avoids modifying during enumeration
            {
                enemyController.idToHp[key] *= 2;
            }
            throw new NotImplementedException();
        }

        public override void onRoomClear()
        {
            throw new NotImplementedException();
        }

        public override void onSpawn()
        {
            throw new NotImplementedException();
        }

        public override void onTakeDamage(Bullet b)
        {
            
        }
    }
}
