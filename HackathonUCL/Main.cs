using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace HackathonUCL
{
    public class Main : Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SpriteFont font;
        public static GameTime gameTime;
        public static Random rand;
        public static Camera mainCamera;
        public static List<IUpdate> Updateables = new List<IUpdate>();

        public static LocationHost Locations = new LocationHost();
        public static int GlobalTimer;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1033;
            graphics.PreferredBackBufferHeight = 516;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainCamera = new Camera();
            rand = new Random();

            mainCamera.CamPos = Vector2.Zero;
            font = Content.Load<SpriteFont>("HackFont");
            TextureCache.LoadTextures(Content);
            LocationCache.LoadLocations();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            GlobalTimer++;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (IUpdate update in Updateables)
            {
                update.Update();
            }

            Locations.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: mainCamera.Transform, samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(TextureCache.Map, TextureCache.Map.Bounds, Color.White);
            Locations.Render(spriteBatch);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
