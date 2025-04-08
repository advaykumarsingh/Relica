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
    class ActiveItem: Item
    {
        Action<Player> effect;

        private float cooldown;
        private float cooldownTimer;
        public bool isActive
        {
            get;
            set;
        } = false;
        public ActiveItem(Action<Player> effect, float cooldown, int id, string name, string description, Rectangle rectangle, Texture2D icon, int sWidth, int sHeight, int sPadding, int sPerLine) : base(id, name, description, rectangle, icon, sWidth, sHeight, sPadding, sPerLine)
        {
            this.effect += effect;
            this.cooldown = cooldown;
            
        }

        public override void ApplyEffect(Player player)
        {
            if (cooldownTimer <= 0)
            {
                isActive = true;
                effect?.Invoke(player);
                cooldownTimer = cooldown;
            }
            else
            {
                isActive = false;
            }
        }

        public override void OnPickup()
        {
            base.Collected = true;
        }

        public override void OnRemove(Player player)
        {
            base.Collected = false;
            if (!GameItems.generatedRoomItems.Contains(this))
            {
                this.Rectangle = new Rectangle(GameItems.generatedRoomItems.Last().Rectangle.X + 100, this.Rectangle.Y, this.Rectangle.Width, this.Rectangle.Height);
                GameItems.generatedRoomItems.Add(this);


            }

        }
        public void Update()
        {
            if (cooldownTimer > 0)
                cooldownTimer -= 1;
        }

        public void DrawItem()
        {
            base.Draw(Game1.spriteBatch, Game1.font);
        }
    }
}
