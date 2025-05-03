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
using System.IO;

namespace Relica
{
    class DungeonBuilder
    {
        public Player p = Player.player; //update to player
        Level[,] levelGrid;
        Point location;
        public DungeonBuilder()
        {
            levelGrid = new Level[7, 7];
            location = new Point(3, 3);
        }
        public void update()
        {
            if (p.Y <= Level.yLevelOffset)
            {
                p.Y = Level.yLevelOffset + 800 - 32;
                location.Y++;
            }
            else if (p.Y >= Level.yLevelOffset + 800)
            {
                p.Y = Level.yLevelOffset;
                location.Y--;
            }
            else if (p.X <= Level.xLevelOffset)
            {
                p.X = Level.xLevelOffset + 800 - 32;
                location.X++;
            }
            else if (p.X >= Level.xLevelOffset + 800)
            {
                p.X = Level.xLevelOffset;
                location.Y++;
            }
            p.level = levelGrid[location.Y,location.X];
        }
    }
}
