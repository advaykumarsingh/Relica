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
    public class Level
    {
        public readonly static int xLevelOffset = 560;
        public readonly static int yLevelOffset = 140;
        public static enemyController enemyController;
        private Texture2D floor;
        public Tile[,] floorGrid;
        public int waveCount = 0;
        public Enemy[,,] spawnLocations;
        public Level(string path)
        {
            StreamReader r = new StreamReader(path);
            string fRow = r.ReadLine();
            int floorRows = int.Parse(fRow.Substring(fRow.IndexOf('>')+2));
            string fCol = r.ReadLine();
            int floorCols = int.Parse(fCol.Substring(fCol.IndexOf('>') + 2));
            string fType = r.ReadLine();
            int floorType = int.Parse(fType.Substring(fType.IndexOf('>') + 2));
            initFloor(floorType);
            string fWaveCount = r.ReadLine();
            int maxWaves = int.Parse(fWaveCount.Substring(fWaveCount.IndexOf('>') + 2));
            initFloor(floorType);
            floorGrid = new Tile[floorRows * 25, floorCols * 25];
            spawnLocations = new Enemy[maxWaves,floorRows * 25 , floorCols * 25];
            for (int rowTemp = 0; rowTemp < floorGrid.GetLength(0); rowTemp++)
            {
                string[] idTemp = r.ReadLine().Split('\t');
                for (int colTemp = 0; colTemp < floorGrid.GetLength(1);colTemp++)
                {
                    floorGrid[rowTemp, colTemp] = new Tile(int.Parse(idTemp[colTemp]));
                }
            }
            r.ReadLine();
            for (int z = 0; z < spawnLocations.GetLength(0);z++)
            {
                for (int rowTemp = 0; rowTemp < spawnLocations.GetLength(1); rowTemp++)
                {
                    string[] idTemp = r.ReadLine().Split('\t');
                    for (int colTemp = 0; colTemp < spawnLocations.GetLength(2); colTemp++)
                    {
                        spawnLocations[z, rowTemp, colTemp] = enemyController.makeEnemy(int.Parse(idTemp[colTemp]), colTemp * 32, rowTemp * 32);
                    }
                }
            }
        }
        public Level(Tile[,] floorGrid, Enemy[,,] spawnLocations, int floorType)
        {
            this.floorGrid = floorGrid;
            this.spawnLocations = spawnLocations;
            this.initFloor(floorType);
        }

        private void initFloor(int floorType)
        {
            switch (floorType)
            {
                case 1:
                    floor = LevelBuilder.JungleTxt;
                    break;
                case 2:
                    floor = LevelBuilder.floor2Txt;
                    break;
                case 3:
                    floor = LevelBuilder.TowerTopTxt;
                    break;
            }
        }

        public void update()
        {

            Player.player.update();
            if (waveCount < spawnLocations.GetLength(0))
            {;
                int enemyUpdated = 0;
                for (int r = 0; r < spawnLocations.GetLength(1); r++)
                {
                    for (int c = 0; c < spawnLocations.GetLength(2); c++)
                    {
                        if (spawnLocations[waveCount, r, c] != null)
                        {
                            spawnLocations[waveCount, r, c].update();
                            enemyUpdated++;
                            if (spawnLocations[waveCount, r, c].Hp <= 0)
                                spawnLocations[waveCount, r, c] = null;
                        }
                        
                    }
                }
                if (enemyUpdated == 0 && waveCount < spawnLocations.GetLength(0)-1)
                    waveCount++;
                if (waveCount == spawnLocations.GetLength(2))
                    Player.player.roomClear();
            }

        }

        public Level rotate90()
        {
            Tile[,] temp = new Tile[floorGrid.GetLength(0), floorGrid.GetLength(1)];
            Enemy[,,] tempEnemy = new Enemy[spawnLocations.GetLength(0), spawnLocations.GetLength(1), spawnLocations.GetLength(2)];


            for (int rOff = -12; rOff < tempEnemy.GetLength(1) / 2 + tempEnemy.GetLength(1) % 2; rOff++)
            {
                for (int cOff = -12; cOff < tempEnemy.GetLength(2) / 2 + tempEnemy.GetLength(2) % 2; cOff++)
                {
                    for (int z = 0; z < tempEnemy.GetLength(0); z++)
                    {
                        tempEnemy[z, 12 + rOff, 12 + cOff] = spawnLocations[z, 12 + cOff, -(rOff - 12)];
                    }
                    temp[12 + rOff, 12 + cOff] = floorGrid[12 + cOff, -(rOff - 12)];
                    temp[12 + rOff, 12 + cOff].id = temp[12 + rOff, 12 + cOff].id / 4 * 4 + (temp[12 + rOff, 12 + cOff].id + 1) % 4;
                }
            }
            floorGrid = temp;
            spawnLocations = tempEnemy;
            return this;

        }
        public bool InBounds(int r, int c)
        {
            return r >= 0 && r < floorGrid.GetLength(0) && c >= 0 && c < floorGrid.GetLength(1);
        }
        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Rectangle destRect = new Rectangle(xLevelOffset, yLevelOffset, 32, 32);
            for (int r = 0; r < floorGrid.GetLength(0); r++)
            {
                for (int c = 0; c < floorGrid.GetLength(1); c++)
                {
                    spriteBatch.Draw(floor, destRect,
                        new Rectangle(Math.Max((4 * (floorGrid[r, c].id % 24) - 1), 0) + floorGrid[r, c].id % 24 * 32,
                        Math.Max((4 * (floorGrid[r, c].id / 24)), 0) + floorGrid[r, c].id / 24 * 32, Tile.TILE_SIZE, Tile.TILE_SIZE), Color.White);
                    if (r == 0 && c == 12)
                        spriteBatch.Draw(floor, new Rectangle(xLevelOffset + 12 * 32, yLevelOffset - 40, 32, 32), Color.White);
                    destRect.X += 32;
                    if (spawnLocations[waveCount, r, c] != null)
                        spawnLocations[waveCount, r, c].draw(gameTime, spriteBatch);
                }
                destRect.X = xLevelOffset;
                destRect.Y += 32;
            }
            Player.player.draw(gameTime, spriteBatch);
        }
    }
}
