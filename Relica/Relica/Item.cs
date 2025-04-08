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
    abstract class Item : Entity
    {
        public int Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool ShowDescription { get; set; }

        public string Name { get; set; }
        public Texture2D Icon { get; set; }

        public bool Collected { get; set; } = false;


        public Item(int id, string name, string description, Rectangle rectangle, Texture2D icon, int sWidth, int sHeight, int sPadding, int sPerLine) : base(rectangle, icon, sWidth, sHeight, sPadding, sPerLine)
        {
            Name = name;
            Icon = icon;
            Description = description;
            Id = id;

        }
        public bool IsNearPlayer(Player player)
        {
            return Rectangle.Intersects(player.Rectangle); // or use Vector2.Distance for smoother check
        }
        private void DrawDescription(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (ShowDescription)
            {
                spriteBatch.DrawString(font, Description, new Vector2(Rectangle.X, Rectangle.Y - 20), Color.Black);
                spriteBatch.DrawString(font, "Press Q to pick up item", new Vector2(100, Game1.screenHeight - 30), Color.Red);



            }
        }

        public abstract void OnPickup();
        public abstract void OnRemove(Player player);
        public abstract void ApplyEffect(Player player);
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            spriteBatch.Draw(Icon, base.Rectangle, base.SourceRect, Color.White);
            if (ShowDescription)
            {
                DrawDescription(spriteBatch, font);

            }
        }

    }
}
