using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relica
{
    public abstract class EnemyItem : PassiveItem
    {
        public static List<Type> acceptedTypeOfEnemy;
        public EnemyItem(string name, string desc, int x, int y) : base(name, desc, x, y)
        {

        }

        public override void onActiveUse()
        {
            
        }
        public override void onKillEnemy()
        {
            
        }

        public override void onPickupOtherItem()
        {
            
        }

        public override void onPlaceItem()
        {
            
        }
        public override void onTakeDamage()
        {
            
        }
        public abstract void onTakeDamage(Bullet b);



        public bool validEnemy(Enemy e)
        {
            return acceptedTypeOfEnemy.Contains(e.GetType());
        }
    }
}
