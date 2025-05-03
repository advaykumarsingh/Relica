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
    public abstract class Enemy : Entity
    {
        public static enemyController mastermind;
        private int id;
        private int hp;
        
        public Enemy(int id, Rectangle eRect, Texture2D eTxt) : base(eRect, eTxt)
        {
            this.id = id;
            hp = enemyController.idToHp[id];
            
        }
        public Enemy(int id, int x,int y) : 
            this(id, new Rectangle(x, y, enemyController.idToWidth[id], enemyController.idToHeight[id]), enemyController.idToSprites[id])
        {
        }
        public int Id
        {
            get { return id; }
        }
        public int Hp
        {
            get { return hp; }
        }

        public override void update()
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle srcRect)
        {
            Rectangle temp = eRect;
            temp.X += Level.xLevelOffset;
            temp.Y += Level.yLevelOffset;
            spriteBatch.Draw(eTxt, temp, srcRect, Color.White);
        }
        public void changeHp(int hp)
        {
            this.hp = hp;
        }
    }
}
