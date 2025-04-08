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
    class PassiveItem : Item
    {
        public event Action<Player> OnEffectApplied; // Event for subscriber-publisher system
        private event Action<Player> reverseEffect; // Event for subscriber-publisher system




        public PassiveItem(Action<Player> effect, Action<Player> reverseEffect, int id, string name, string description, Rectangle rectangle, Texture2D icon, int sWidth, int sHeight, int sPadding, int sPerLine) : base(id, name, description, rectangle, icon, sWidth, sHeight, sPadding, sPerLine)
        {
            OnEffectApplied += effect;
            this.reverseEffect = reverseEffect;
        }

        public override void ApplyEffect(Player player)
        {
            Console.WriteLine(player.previousHealth);

            OnEffectApplied?.Invoke(player);
            Console.WriteLine(player.health);

        }

        public override void OnPickup()
        {
            base.Collected = true;
        }

        public override void OnRemove(Player player)
        {

            base.Collected = false;
            GameItems.generatedRoomItems.Add(this);
            reverseEffect?.Invoke(player);
            Console.WriteLine(player.health);
            

        }

        public void DrawItem()
        {
            base.Draw(Game1.spriteBatch, Game1.font);
        }

 
    }
}
