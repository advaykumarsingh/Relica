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

namespace Relica
{
    public abstract class ActiveItem : Item
    {
        public static Texture2D activeSheet;
        public int curCharge;
        public int MaxCharge;
        public readonly bool active;
        public TypeCooldown chargeType;
        public TypeIcon icon;
        public enum TypeCooldown
        {
            TimedCooldown,
            RoomCooldown,
            KillCooldown,
        }
        public enum TypeIcon
        {
            SplitShot,
            Reroll,
            FullCircleShot
        }
        public ActiveItem(int maxCharge, string name, string description, int x, int y, TypeCooldown chargeType,TypeIcon icon) : base(name, description, x, y, activeSheet)
        {
            active = false;
            this.MaxCharge = maxCharge;
            this.chargeType = chargeType;
            this.curCharge = maxCharge;
            this.icon = icon;
        }

        public abstract void ActiveUse();

        public bool cooldownReady()
        {
            return curCharge == MaxCharge;
        }
        public void resetCooldown()
        {
            curCharge = 0;
        }
        
    }
}
