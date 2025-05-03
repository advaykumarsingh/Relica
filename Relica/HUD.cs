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
    class HUD
    {
        public Rectangle frame;
        public Rectangle HUDrect;
        Rectangle dashRect;
        Rectangle dashSource;
        static Rectangle activeIcon = new Rectangle(1064, 112, 80, 80);
        static Rectangle iconDest = new Rectangle(0, 0, 80, 80);
        Rectangle healthBar;
        Color healthBarColor;

        Dictionary<string, Texture2D> textures;

        public HUD(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            textures = new Dictionary<string, Texture2D>();
            frame = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 440, 0, 880, 1080);
            HUDrect = new Rectangle((GraphicsDevice.Viewport.Width / 2) - 400, 40, 800, 200);
            dashRect = new Rectangle(1064, 0, 88, 16);
            dashSource = new Rectangle(0, 0, 88, 16);
            healthBar = new Rectangle(656, 56, Player.player.stats[(int)Player.stat.Hp] * 4, 32);
            healthBarColor = new Color(222, 36, 36);
            //if (player.itemManager.activeItem != null)
            //{
            //    iconText = player.itemManager.activeItem.Icon;
            //}
        }


        public void UpdateHUD()
        {




            //Spritebatch.Draw(iconText, activeIcon, iconDest, Color.White);
        }

        public void AddTexture(string key, Texture2D texture)
        {
            textures.Add(key, texture);
        }
        public void draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            spriteBatch.Draw(textures["frame"], frame, Color.White);
            spriteBatch.Draw(textures["blank"], healthBar, healthBarColor);
            spriteBatch.Draw(textures["HUD"], HUDrect, Color.White);
            for (int i = 0; i < Player.player.stats[(int)Player.stat.MaxDashes]; i++)
            {
                Color c = i < Player.player.dashesAvailable ? Color.White : Color.Gray;
                dashRect.Y = 50 + i * 24;
                dashRect.Width = 88;
                spriteBatch.Draw(textures["dash"], dashRect, c);
            }
            for (int i = Player.player.dashesAvailable; i < Player.player.stats[(int)Player.stat.MaxDashes]; i++)
            {
                dashRect.Y = 50 + i * 24;
                dashRect.Width = (Player.player.dashRefillCurTime - Player.player.stats[(int)Player.stat.DashCooldown]) / Player.player.dashRefillCurTime * textures["dash"].Width;
                dashSource.Width = (Player.player.dashRefillCurTime - Player.player.stats[(int)Player.stat.DashCooldown]) / Player.player.dashRefillCurTime * textures["dash"].Width;
                spriteBatch.Draw(textures["dash"], dashRect, dashSource, Color.DarkGray);
            }
        }
    }
}
