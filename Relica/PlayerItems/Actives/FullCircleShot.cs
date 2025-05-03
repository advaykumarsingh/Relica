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
    public class FullCircleShot: ActiveItem
    {
        public const int id = 2;

        public FullCircleShot(int x, int y) : base(30, "Full Circle Shot", "Shoot 12 projectiles in a 360 degree spread", x, y, TypeCooldown.TimedCooldown, TypeIcon.SplitShot)
        {

        }
        public override void ActiveUse()
        {

            int numProjectiles = 45;
            Vector2 mouseVec = new Vector2(curMouse.X, curMouse.Y);
            Vector2 playerVec = new Vector2(Player.player.eRect.Center.X, Player.player.eRect.Center.Y);
            Vector2 direction = mouseVec - playerVec;
            direction.Normalize();
            float spreadAngle = MathHelper.ToRadians(360f / numProjectiles);

            List<Vector2> directions = new List<Vector2>();
            for (int i = 0; i < numProjectiles; i++)
            {

                directions.Add(Vector2.Transform(direction, Matrix.CreateRotationZ(spreadAngle * (i - numProjectiles))));
            }


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
