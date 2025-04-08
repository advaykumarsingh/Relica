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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SpriteFont font;

        Player p1;
        Cursor cursor;
        int timer;
        
        Texture2D HUDspriteSheet;
        Texture2D SpecialSymbolsSpriteSheet;
        Texture2D TreasureRoomUpgradesSpriteSheet;


        public static int screenWidth;
        public static int screenHeight;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
       

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here


            HUDspriteSheet = this.Content.Load<Texture2D>("HUDupgrades");
            SpecialSymbolsSpriteSheet = this.Content.Load<Texture2D>("SpecialSymbols");
            TreasureRoomUpgradesSpriteSheet = this.Content.Load<Texture2D>("TreasureRoomUpgrades");
            GameItems.HUDspriteSheet = HUDspriteSheet;
            GameItems.SpecialSymbolsSpriteSheet = SpecialSymbolsSpriteSheet;
            GameItems.TreasureRoomUpgradesSpriteSheet = TreasureRoomUpgradesSpriteSheet;

            GameItems.createItems();
            p1 = new Player(new Rectangle(5, 5, 50, 50), this.Content.Load<Texture2D>("box"));
            cursor = new Cursor(new Rectangle(0, 0, 50, 50), this.Content.Load<Texture2D>("Cursor sprite"));

            font = this.Content.Load<SpriteFont>("SpriteFont1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            timer++;
            cursor.Update(timer);
            p1.Update(timer);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            p1.Draw(spriteBatch);
            spriteBatch.Draw(cursor.Texture, cursor.Rectangle, cursor.SourceRect, Color.White);


            //call only in treasure room


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
