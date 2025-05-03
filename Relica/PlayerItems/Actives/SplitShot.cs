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

namespace Relica.Actives
{
    class SplitShot : ActiveItem
    {
        public const int id = 0;
        public SplitShot(int x, int y) : base(30,"Split Shot", "Fire 3 split shots with your active key" , x,y, TypeCooldown.TimedCooldown,TypeIcon.SplitShot)
        {
            CurrTextureIDX = 0;
        }
 
        public override void ActiveUse()
        {
            Vector2 mouseVec = new Vector2(curMouse.X, curMouse.Y);
            Vector2 playerVec = new Vector2(Player.player.eRect.Center.X, Player.player.eRect.Center.Y);
            Vector2 direction = mouseVec - playerVec;
            direction.Normalize();

            float spreadAngle = MathHelper.ToRadians(5); // 5 degrees cone

            Vector2[] directions = new Vector2[3]
            {
                
                Vector2.Transform(direction, Matrix.CreateRotationZ(-spreadAngle)), //left
                direction,//center
                Vector2.Transform(direction, Matrix.CreateRotationZ(spreadAngle)) //right
            };
            foreach (Vector2 dir in directions)
            {
                Vector2 velocity = dir * Player.player.stats[(int)Player.stat.BulletSpeed];
                float rot = (float)Math.Atan2(-dir.Y, -dir.X);
                Player.player.playerBullets.Add(new Bullet(new Rectangle(Player.player.eRect.Center.X, Player.player.eRect.Center.Y, 5, 5),
                    velocity, rot, true, Player.player.stats[(int)Player.stat.Dmg]));
            }
            base.curCharge = base.MaxCharge;
            resetCooldown();
        }
    }
}
