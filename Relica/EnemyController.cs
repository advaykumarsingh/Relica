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
    public class enemyController
    {
        public static Dictionary<int, int> idToWidth;
        public static Dictionary<int, int> idToHeight;
        public static Dictionary<int, int> idToHp;
        public static Dictionary<int, Texture2D> idToSprites;
        public static Dictionary<int, int> idToMaxSpeed;

        public static List<EnemyItem> enemyItems;
        public static List<Bullet> enemyBullets = new List<Bullet>();
        public static Player player;
        public static Texture2D EnemySpriteSheet;
        public enemyController(ContentManager contentManager)
        {
            
            idToWidth = new Dictionary<int, int>();
            idToHeight = new Dictionary<int, int>();
            idToHp = new Dictionary<int, int>();
            idToSprites = new Dictionary<int, Texture2D>();
            idToMaxSpeed = new Dictionary<int, int>();
            LoadContent(contentManager);
        }

        private void LoadContent(ContentManager contentManager)
        {
            EnemySpriteSheet = contentManager.Load<Texture2D>("EnemySheet");
            Slime.slimeSheet = contentManager.Load<Texture2D>("EnemySheet");
            loadSlime();
        }
        private void loadSlime()
        {
            idToWidth.Add(Slime.id, (int)(32 * Slime.size));
            idToHeight.Add(Slime.id, (int)(32 * Slime.size));
            idToHp.Add(Slime.id, Slime.hp);
            idToSprites.Add(Slime.id, Slime.slimeSheet);
            idToMaxSpeed.Add(Slime.id, Slime.maxSpeed);
        }

        public Enemy makeEnemy(int id, int x, int y)
        {
            switch (id)
                {
                case Slime.id:
                    return new Slime(x, y);
                default:
                return null;
            }
        }
        public static Point CenteredPointForRectangle(int width, int height)
        {
            return new Point(player.eRect.Center.X - width, player.eRect.Center.Y - height);
        }

        public static void updateEnemyItems()
        {
            foreach (EnemyItem enemyItem in enemyItems)
            {
                enemyItem.update();
            }
        }
    }
}