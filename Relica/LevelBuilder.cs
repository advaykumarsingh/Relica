using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Relica
{
    class LevelBuilder
    {
        private static GraphicsDevice graphicsDevice;
        public static Texture2D JungleTxt;
        public static Texture2D floor2Txt;
        public static Texture2D TowerTopTxt;
        public static Texture2D gridLine;
        private static Texture2D enemySprites;
        private static Texture2D saveButton;
        private static Texture2D checkBox;
        public static ContentManager content;
        private static Rectangle saveButtonRect;
        private static Rectangle saveStringEntry;
        private static Rectangle[] mouseRadiusSlider;
        private static Rectangle checkBoxRect;
        private Color saveStringTypingColor;
        private int saveState;
        private int floorNum;
        private int roomWidth;
        private int roomHeight;
        public int selTile;
        private int mouseRadius;
        private int selEnemyId;
        private int curMode;
        private bool enemyDrawRadius;
        private Texture2D floor;
        Tile[,] floorGrid;
        Enemy[,,] spawnLocations;
        int waveNum;
        string lvlName;
        string saveResult;
        private KbHandler kbHandler;
        public class KbHandler
        {
            private Keys[] lastPressedKeys;
            private Stack<char> textInput;
            public KbHandler()
            {
                lastPressedKeys = new Keys[0];
                textInput = new Stack<char>();
            }

            public void Update()
            {
                KeyboardState kbState = Keyboard.GetState();
                Keys[] pressedKeys = kbState.GetPressedKeys();

                //check if any of the previous update's keys are no longer pressed
                foreach (Keys key in lastPressedKeys)
                {
                    if (!pressedKeys.Contains(key))
                        OnKeyUp(key);
                }

                //check if the currently pressed keys were already pressed
                foreach (Keys key in pressedKeys)
                {
                    if (!lastPressedKeys.Contains(key))
                        OnKeyDown(key);
                }

                //save the currently pressed keys so we can compare on the next update
                lastPressedKeys = pressedKeys;
            }

            private void OnKeyDown(Keys key)
            {
                if (key >= Keys.A && key <= Keys.Z)
                    textInput.Push((char)key);
                else if (key == Keys.Back && textInput.Count != 0)
                    textInput.Pop();
                else if (key == Keys.Space)
                    textInput.Push(' ');
            }
            public string textOutput()
            {
                string ret = ToString();
                textInput = new Stack<char>();
                return ret;
            }
            public override string ToString()
            {
                string ret = "";
                Stack<char> temp = new Stack<char>();
                while (textInput.Count > 0)
                {
                    char c = textInput.Pop();
                    ret = c + ret;
                    temp.Push(c);
                }
                while (temp.Count > 0)
                    textInput.Push(temp.Pop());
                return ret;
            }
            private void OnKeyUp(Keys key)
            {
                //do stuff
            }
        }
        int shiftStillPaint;
        int setPos;
        Level level;

        public static enemyController mastermind;
        public LevelBuilder(GraphicsDevice graphicsDevice, ContentManager contentManager,enemyController masterMind)
        {
            LevelBuilder.graphicsDevice = graphicsDevice;
            content = contentManager;
            LoadContent(content);
            saveState = 0;
            floorNum = 1;
            roomWidth = 1;
            roomHeight = 1;
            selTile = 0;
            mouseRadius = 1;
            selEnemyId = 0;
            lvlName = "";
            curMode = 0;
            enemyDrawRadius = false;
            floor = JungleTxt;
            floorGrid = new Tile[25, 25];
            waveNum = 1;
            spawnLocations = new Enemy[waveNum, 25 * roomHeight, 25 * roomWidth];
            level = new Level(floorGrid, spawnLocations, floorNum);
            changeFloor(1);
            saveResult = "";
            kbHandler = new KbHandler();
            shiftStillPaint = 0;
            setPos = 0;
            mastermind = masterMind;
        }

        private void LoadContent(ContentManager contentManager)
        {
            gridLine = contentManager.Load<Texture2D>("gridLine");
            JungleTxt = contentManager.Load<Texture2D>("Level1RoomSpritesV3");
            enemySprites = contentManager.Load<Texture2D>("EnemySheet");
            saveButton = contentManager.Load<Texture2D>("SaveButton");
            checkBox = contentManager.Load<Texture2D>("CheckBox");
            saveButtonRect = new Rectangle(200, 260, 100, 55);
            saveStringEntry = new Rectangle(200, 200, 200, 50);
            checkBoxRect = new Rectangle(200, 135, 32, 32);
            saveStringTypingColor = Color.Gray;
            mouseRadiusSlider = new Rectangle[2];
            mouseRadiusSlider[0] = new Rectangle(200, 100, 200, 30);
            mouseRadiusSlider[1] = new Rectangle(mouseRadiusSlider[0].X, 95, 10, 40);
            
        }

        public void update()
        {
            MouseState curMouse = Entity.curMouse;
            KeyboardState curKey = Entity.curKey;
            if (saveStringTypingColor != Color.White && Entity.wasKeyPressed(Keys.Space))
                curMode = (curMode + 1) % 3;
            if (Entity.wasKeyPressed(Keys.D1))
                changeFloor(1);
            else if (Entity.wasKeyPressed(Keys.D2))
                changeFloor(2);
            else if (Entity.wasKeyPressed(Keys.D3))
                changeFloor(3);

            if (curMouse.ScrollWheelValue < Entity.oldMouse.ScrollWheelValue)
            {
                mouseRadiusSlider[1].X = Math.Max(mouseRadiusSlider[0].X, mouseRadiusSlider[1].X - 10);
                mouseRadius = (mouseRadiusSlider[1].X - (mouseRadiusSlider[0].X - 50)) / 50;
            }
            if (curMouse.ScrollWheelValue > Entity.oldMouse.ScrollWheelValue)
            {
                mouseRadiusSlider[1].X = Math.Min(mouseRadiusSlider[0].X + mouseRadiusSlider[0].Width - 1, mouseRadiusSlider[1].X + 10);
                mouseRadius = (mouseRadiusSlider[1].X - (mouseRadiusSlider[0].X - 50)) / 50;
            }
            if (curMouse.LeftButton == ButtonState.Pressed && new Rectangle(curMouse.X, curMouse.Y, 30, 30).Intersects(mouseRadiusSlider[0]))
            {
                mouseRadiusSlider[1].X = Math.Max(Math.Min(curMouse.X, mouseRadiusSlider[0].X + mouseRadiusSlider[0].Width), mouseRadiusSlider[0].X);
                mouseRadius = (mouseRadiusSlider[1].X - (mouseRadiusSlider[0].X - 50)) / 50;
            }

            if (curMouse.LeftButton == ButtonState.Pressed && Entity.oldMouse.LeftButton != ButtonState.Pressed
                && new Rectangle(curMouse.X, curMouse.Y, 5, 5).Intersects(checkBoxRect))
                enemyDrawRadius = !enemyDrawRadius;

            if (curKey.IsKeyDown(Keys.LeftShift))
            {
                if (Entity.oldKey.IsKeyUp(Keys.LeftShift))
                {
                    if (Math.Abs(curMouse.X - Entity.oldMouse.X) > Math.Abs(curMouse.Y - Entity.oldMouse.Y))
                    {
                        shiftStillPaint = 1;
                        setPos = Entity.oldMouse.Y;
                    }
                    else
                    {
                        shiftStillPaint = 2;
                        setPos = Entity.oldMouse.X;
                    }
                }
                else
                {
                    if (shiftStillPaint == 1)
                        Mouse.SetPosition(curMouse.X, setPos);
                    else if (shiftStillPaint == 2)
                        Mouse.SetPosition(setPos, curMouse.Y);
                }
            }
            if (Entity.wasKeyPressed(Keys.I))
            {
                level.rotate90();
                spawnLocations = level.spawnLocations;
                floorGrid = level.floorGrid;
            }

            if ((curMouse.LeftButton == ButtonState.Pressed || curMouse.RightButton == ButtonState.Pressed) && curMode == 0)
                place(curMouse);
            else if ((curMouse.LeftButton == ButtonState.Pressed && curMode != 0))
                setTile(curMouse);

            if (Entity.wasKeyPressed(Keys.Down))
            {
                if (waveNum > 1)
                {
                    waveNum--;
                    Enemy[,,] temp = spawnLocations;
                    spawnLocations = new Enemy[waveNum, roomHeight * 25, roomWidth * 25];
                    for (int z = 0; z < spawnLocations.GetLength(0); z++)
                    {
                        for (int r = 0; r < temp.GetLength(1); r++)
                        {
                            for (int c = 0; c < temp.GetLength(2); c++)
                            {
                                spawnLocations[z, r, c] = temp[z, r, c];
                            }
                        }
                    }
                    level.waveCount--;
                    level.spawnLocations = this.spawnLocations;
                }
                else
                    waveNum = 1;
            }
            if (Entity.wasKeyPressed(Keys.Right))
            {
                Enemy[,,] temp = spawnLocations;
                waveNum++;
                spawnLocations = new Enemy[waveNum, roomHeight * 25 , roomWidth * 25 ];
                level.waveCount++;
                for (int z = 0; z < temp.GetLength(0);z++)
                {
                    for (int r = 0; r < temp.GetLength(1);r++)
                    {
                        for (int c = 0;c< temp.GetLength(2);c++)
                        {
                            spawnLocations[z, r, c] = temp[z, r, c];
                        }
                    }
                }
                level.spawnLocations = this.spawnLocations;
            }



            if (curMouse.LeftButton == ButtonState.Pressed
                && Entity.oldMouse.LeftButton != ButtonState.Pressed
                && new Rectangle(curMouse.X, curMouse.Y, 1, 1).Intersects(saveButtonRect))
                saveLevel();
            if (curMouse.LeftButton == ButtonState.Pressed
                && Entity.oldMouse.LeftButton != ButtonState.Pressed)
            {
                if (new Rectangle(curMouse.X, curMouse.Y, 1, 1).Intersects(saveStringEntry))
                    saveStringTypingColor = Color.White;
                else
                    saveStringTypingColor = Color.Gray;
            }
            if (saveStringTypingColor == Color.White)
            {
                kbHandler.Update();
            }
            if (curKey.IsKeyDown(Keys.B))
            {
                level = new Level((content.RootDirectory + "\\Levels\\" + "F1_JUNGLE" + "\\" + "ENEMYTEST" + ".lvl"));
            }
            if (curMouse.MiddleButton == ButtonState.Pressed && Entity.oldMouse.MiddleButton != ButtonState.Pressed)
                pickTile(curMouse);
        }

        //private void setTile(MouseState curMouse)
        //{
        //    if (curMouse.X - Level.xLevelOffset < 0 || (curMouse.X - Level.xLevelOffset) / 144 > 5)
        //        return;
        //    if (curMode == 1)
        //    {
        //        if (curMouse.Y - 140 < 0 || (curMouse.Y - 140) / 144 > 5)
        //            return;
        //        int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 144;
        //        int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 144;
        //        selTile = mouseYIndex * 6 + mouseXIndex;
        //    }
        //    if (curMode == 2)
        //    {
        //        if (curMouse.Y - 140 < 0 || (curMouse.Y - 140) / 144 > 2)
        //            return;
        //        int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 144;
        //        int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 144;
        //        selEnemyId = mouseYIndex * 6 + mouseXIndex;
        //    }
        //}
        private void setTile(MouseState curMouse)
        {
            if (curMouse.X - Level.xLevelOffset < 0 || (curMouse.X - Level.xLevelOffset) / 144 > 5)
                return;
            if (curMode == 1)
            {
                if (curMouse.Y - 140 < 0 || (curMouse.Y - 140) / 36 > 12)
                    return;
                int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 36;
                int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 36;
                selTile = mouseYIndex * 24 + mouseXIndex;
            }
            if (curMode == 2)
            {
                if (curMouse.Y - 140 < 0 || (curMouse.Y - 140) / 144 > 2)
                    return;
                int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 144;
                int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 144;
                selEnemyId = mouseYIndex * 6 + mouseXIndex;
            }
        }

        //private void place(MouseState curMouse)
        //{
        //    int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 32;
        //    int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 32;
        //    if (mouseXIndex < 0 || mouseXIndex > 25 * roomWidth - 1 || mouseYIndex < 0 || mouseYIndex > 25 * roomHeight - 1)
        //        return;
        //    if (curMouse.LeftButton == ButtonState.Pressed)  //&& oldMouse.LeftButton != ButtonState.Pressed)
        //    {
        //        for (int rOff = -1 * (mouseRadius - 1); rOff < mouseRadius; rOff++)
        //            for (int cOff = -1 * (mouseRadius - 1); cOff < mouseRadius; cOff++)
        //                if (mouseYIndex + rOff >= 0 && mouseXIndex + cOff >= 0 && mouseYIndex + rOff <= 25 * roomHeight - 1 && mouseXIndex + cOff <= 25 * roomWidth - 1)
        //                    floorGrid[mouseYIndex + rOff, mouseXIndex + cOff] = selTile;
        //    }
        //    else if (curMouse.RightButton == ButtonState.Pressed)
        //        for (int rOff = -1 * (mouseRadius - 1); rOff < mouseRadius; rOff++)
        //            for (int cOff = -1 * (mouseRadius - 1); cOff < mouseRadius; cOff++)
        //                if (mouseYIndex + rOff >= 0 && mouseXIndex + cOff >= 0 && mouseYIndex + rOff <= 25 * roomHeight - 1 && mouseXIndex + cOff <= 25 * roomWidth - 1)
        //                    spawnLocations[waveNum-1, mouseYIndex + rOff, mouseXIndex + cOff] = mastermind.makeEnemy(selEnemyId, 32 * (mouseXIndex+cOff), 32 * (mouseYIndex+ rOff));
        //    level.floorGrid = this.floorGrid;
        //    level.spawnLocations = this.spawnLocations;

        //}
        private void place(MouseState curMouse)
        {
            int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 32;
            int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 32;
            if (mouseXIndex < 0 || mouseXIndex > 25 * roomWidth - 1 || mouseYIndex < 0 || mouseYIndex > 25 * roomHeight - 1)
                return;
            if (curMouse.LeftButton == ButtonState.Pressed)  //&& oldMouse.LeftButton != ButtonState.Pressed)
            {
                for (int rOff = -1 * (mouseRadius - 1); rOff < mouseRadius; rOff++)
                    for (int cOff = -1 * (mouseRadius - 1); cOff < mouseRadius; cOff++)
                        if (mouseYIndex + rOff >= 0 && mouseXIndex + cOff >= 0 && mouseYIndex + rOff <= 25 * roomHeight - 1 && mouseXIndex + cOff <= 25 * roomWidth - 1)
                            floorGrid[mouseYIndex + rOff, mouseXIndex + cOff].id = selTile;
            }
            else if (curMouse.RightButton == ButtonState.Pressed)
                for (int rOff = -1 * (mouseRadius - 1); rOff < mouseRadius; rOff++)
                    for (int cOff = -1 * (mouseRadius - 1); cOff < mouseRadius; cOff++)
                        if (mouseYIndex + rOff >= 0 && mouseXIndex + cOff >= 0 && mouseYIndex + rOff <= 25 * roomHeight - 1 && mouseXIndex + cOff <= 25 * roomWidth - 1)
                            spawnLocations[waveNum - 1, mouseYIndex + rOff, mouseXIndex + cOff] = mastermind.makeEnemy(selEnemyId, 32 * (mouseXIndex + cOff), 32 * (mouseYIndex + rOff));
            level.floorGrid = this.floorGrid;
            level.spawnLocations = this.spawnLocations;

        }

        public void pickTile(MouseState curMouse)
        {
            int mouseXIndex = (curMouse.X - Level.xLevelOffset) / 32;
            int mouseYIndex = (curMouse.Y - Level.yLevelOffset) / 32;
            if (mouseXIndex < 0 || mouseXIndex > 25 * roomWidth - 1 || mouseYIndex < 0 || mouseYIndex > 25 * roomHeight - 1 || curMode != 0)
                return;
            selTile = floorGrid[mouseYIndex, mouseXIndex].id;

        }

        private void saveLevel()
        {
            lvlName += kbHandler.textOutput();
            if (lvlName.Length < 1)
            {
                saveResult = "Level needs a name with length \ngreater than 0";
                lvlName = "";
                saveState = 1;
                return;
            }
            string prefix = "";
            switch (floorNum)
            {
                case 1:
                    prefix += "F1_Jungle";
                    break;
                case 2:
                    prefix += "F2_Scrapyard";
                    break;
                case 3:
                    prefix += "F3_TowerTop";
                    break;
            }

            //Random rand = new Random();
            //string randChar = "";
            //do
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        randChar += (char)rand.Next(96,123); // the ascii of 'a' and one after the ascii of 'z'
            //    }
            //}
            //while (File.Exists(prefix + randChar));
            lvlName = lvlName.Trim();
            StreamWriter writer = new StreamWriter((content.RootDirectory + "\\Levels\\" + prefix + "\\" + lvlName + ".lvl"));
            writer.WriteLine("width -> " + floorGrid.GetLength(0) / 25);
            writer.WriteLine("height -> " + floorGrid.GetLength(1) / 25);
            writer.WriteLine("floor -> " + floorNum);
            writer.WriteLine("WaveCount -> " + waveNum);
            for (int rowTemp = 0; rowTemp < floorGrid.GetLength(0); rowTemp++)
            {
                string idTemp = "";
                for (int colTemp = 0; colTemp < floorGrid.GetLength(1); colTemp++)
                {
                    idTemp += floorGrid[rowTemp, colTemp] + "\t";
                }
                writer.WriteLine(idTemp);
            }
            writer.WriteLine("");
            for (int z = 0; z < spawnLocations.GetLength(0); z++)
            {
                for (int rowTemp = 0; rowTemp < spawnLocations.GetLength(1); rowTemp++)
                {
                    string idTemp = "";
                    for (int colTemp = 0; colTemp < spawnLocations.GetLength(2); colTemp++)
                    {
                        if (spawnLocations[z, rowTemp, colTemp] != null)
                            idTemp += spawnLocations[z,rowTemp,colTemp].Id + "\t";
                        else
                            idTemp += "-1\t";
                    }
                    writer.WriteLine(idTemp);
                }
                writer.WriteLine("");
            }
            writer.Close();
            floorGrid = new Tile[floorGrid.GetLength(0), floorGrid.GetLength(1)];
            //spawnLocations = new Enemy[spawnLocations.GetLength(0), spawnLocations.GetLength(1)];
            saveResult = "Save successful\nsaved as\n" + (content.RootDirectory + "\\Levels\\" + prefix + "\\" + lvlName + ".lvl");
            lvlName = "";
            saveState = 2;

        }

        private void changeFloor(int selFloor)
        {
            floorNum = selFloor;
            switch (selFloor)
            {
                case 1:
                    floor = JungleTxt;
                    break;
                case 2:
                    floor = floor2Txt;
                    break;
                case 3:
                    floor = TowerTopTxt;
                    break;
            }
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font1)
        {
            //if (curMode == 0)
            //{
            //    level.draw(gameTime, spriteBatch);
            //    spriteBatch.Draw(floor, new Rectangle(Entity.oldMouse.X, Entity.oldMouse.Y, 32, 32),
            //        new Rectangle((selTile % 6) * 32 + Math.Max((4 * (selTile % 6) - 1), 0),
            //        (selTile / 6) * 32 + Math.Max((4 * (selTile / 6)), 0), 32, 32), Color.White);

            //    Rectangle temp = new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 800, 1);
            //    for (int r = 0; r < 26; r++)
            //    {
            //        spriteBatch.Draw(gridLine, temp, Color.White);
            //        temp.Y += 32;
            //    }
            //    temp.Y = 140;
            //    temp.Width = 1;
            //    temp.Height = 800;
            //    for (int c = 0; c < 26; c++)
            //    {
            //        spriteBatch.Draw(gridLine, temp, Color.White);
            //        temp.X += 32;
            //    }
            //}

            //else
            //{
            //    if (curMode == 1)
            //        spriteBatch.Draw(floor, new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 848, 848), Color.White);
            //    if (curMode == 2)
            //        spriteBatch.Draw(enemySprites, new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 848, 416), Color.White);
            //    spriteBatch.Draw(gridLine, new Rectangle(Entity.oldMouse.X, Entity.oldMouse.Y, 8, 8), Color.Red);

            //}

            if (curMode == 0)
            {
                level.draw(gameTime, spriteBatch);
                spriteBatch.Draw(floor, new Rectangle(Entity.oldMouse.X, Entity.oldMouse.Y, 32, 32),
                    new Rectangle((selTile % 24) * 32 + Math.Max((4 * (selTile % 24) - 1), 0),
                    (selTile / 24) * 32 + Math.Max((4 * (selTile / 24)), 0), 32, 32), Color.White);

                Rectangle temp = new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 800, 1);
                for (int r = 0; r < 26; r++)
                {
                    spriteBatch.Draw(gridLine, temp, Color.White);
                    temp.Y += 32;
                }
                temp.Y = 140;
                temp.Width = 1;
                temp.Height = 800;
                for (int c = 0; c < 26; c++)
                {
                    spriteBatch.Draw(gridLine, temp, Color.White);
                    temp.X += 32;
                }
            }

            else
            {
                if (curMode == 1)
                    spriteBatch.Draw(floor, new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 860, 176), Color.White);
                if (curMode == 2)
                    spriteBatch.Draw(enemySprites, new Rectangle(Level.xLevelOffset, Level.yLevelOffset, 848, 416), Color.White);
                spriteBatch.Draw(gridLine, new Rectangle(Entity.oldMouse.X, Entity.oldMouse.Y, 8, 8), Color.Red);

            }
            spriteBatch.DrawString(font1, selEnemyId + "", new Vector2(Entity.oldMouse.X - 30, Entity.oldMouse.Y - 20), Color.White);

            spriteBatch.Draw(gridLine, saveStringEntry, saveStringTypingColor);
            spriteBatch.DrawString(font1, "Level Save Name:", new Vector2(200, 175), Color.White);
            spriteBatch.DrawString(font1, kbHandler.ToString(), new Vector2(200, 200), Color.White);
            spriteBatch.Draw(saveButton, saveButtonRect,
                new Rectangle(0, 44 * saveState + 4 * Math.Max(saveState, 0), 100, 44), Color.White);
            spriteBatch.DrawString(font1, saveResult, new Vector2(200, 320), Color.White);

            spriteBatch.Draw(gridLine, mouseRadiusSlider[0], Color.White);
            spriteBatch.Draw(gridLine, mouseRadiusSlider[1], Color.Gray);
            if (!enemyDrawRadius)
                spriteBatch.Draw(checkBox, checkBoxRect, new Rectangle(0, 0, 32, 32), Color.White);
            else if (enemyDrawRadius)
                spriteBatch.Draw(checkBox, checkBoxRect, new Rectangle(37, 0, 32, 32), Color.White);
            spriteBatch.DrawString(font1, "affect enemies?", new Vector2(checkBoxRect.X + 40, checkBoxRect.Y), Color.White);
            spriteBatch.DrawString(font1, "paint radius: " + mouseRadius, new Vector2(mouseRadiusSlider[0].X, 70), Color.White);

            spriteBatch.DrawString(font1, "wave num: " + waveNum, new Vector2(960, 100), Color.White);
            spriteBatch.DrawString(font1,
                "hold shift while moving in a direction to snap \nto the other direction \n(must be moving before clicking shift)\n\n" +
                "scroll to change paint radius\n\nspace to open sprite sheet\n\nright/left arrow to change wave num\n\n" +
                "left click to place tile\nright click for enemy\n\nclick box, type and click save to finish level", new Vector2(20, 400), Color.White);
        }
    }
}
