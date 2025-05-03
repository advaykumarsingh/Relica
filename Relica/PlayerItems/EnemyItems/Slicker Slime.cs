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

namespace Relica.PlayerItems.EnemyItems
{
    class Slicker_Slime : EnemyItem
    {
        public const int id = 1;
        public static new List<Type> acceptedTypeOfEnemy;
        public Slicker_Slime(int x, int y) : base("Slicker Slime", "More slippery slime allows slime to move faster", x, y)
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
        }

        public override void onMove()
        {
        }

        public override void OnPickup()
        {

            enemyController.idToHp.Add(Slime.id, (int)(enemyController.idToHp[Slime.id] * 1.2));
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

        public void update()
        {

        }
    }
}