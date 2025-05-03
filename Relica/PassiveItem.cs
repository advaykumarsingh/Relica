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
    public abstract class PassiveItem : Item
    {
        public static Texture2D passiveSheet;
        public enum TypeIcon
        {
            Damage,
            Speed,
            Health,
        }


        public PassiveItem(string name, string description, int x,int y) : base( name, description, x,y, passiveSheet)
        {
            
        }

        public abstract void onFireUse();

        public abstract void OnPickup();

        public abstract void onTakeDamage();

        public abstract void onKillEnemy();

        public abstract void onActiveUse();

        public abstract void onDashUse();

        public abstract void onMove();

        public abstract void onHitEnemy();

        public abstract void onSpawn();

        public abstract void onPickupOtherItem();

        public abstract void onPlaceItem();

        public abstract void onRoomClear();

        public override void draw(GameTime gameTime,SpriteBatch spriteBatch)
        {
            base.draw(gameTime,spriteBatch);
        }


    }
}
