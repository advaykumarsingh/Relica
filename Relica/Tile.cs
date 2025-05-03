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
    public class Tile
    {
        public int id;
        Boolean collidable;
        Boolean inTopLayer;
        public const int TILE_SIZE = 32;
        List<int> collidables;
        List<int> topLayer;

        public Tile(int id)
        {
            this.id = id;

            collidables = new List<int>();
            for (int i = 24; i < 36; i++)
            {
                collidables.Add(i);
            }
            topLayer = new List<int>();
            for (int i = 18; i < 24; i++)
            {
                topLayer.Add(i);
            }

            collidable = collidables.Contains(id);
            inTopLayer = topLayer.Contains(id);
        }

        public Boolean Collidable
        {
            get { return collidable; }
        }
        public Boolean InTopLayer
        {
            get { return inTopLayer; }
        }
    }
}
