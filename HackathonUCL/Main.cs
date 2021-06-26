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
        public static Boids boids;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 737;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainCamera = new Camera();
            rand = new Random();
            boids = new Boids();
            boids.OnLoad();

            mainCamera.CamPos = Vector2.Zero;
            font = Content.Load<SpriteFont>("HackFont");
            TextureCache.LoadTextures(Content);
            LocationCache.LoadLocations();

            foreach (Type type in Utils.GetInheritedClasses(typeof(UIScreen)))
            {
                UIScreen Screen = (UIScreen)Activator.CreateInstance(type);
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            GlobalTimer++;
            mainCamera.Invoke();
            boids.OnUpdate();
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

            spriteBatch.Begin(transformMatrix: mainCamera.Transform);
            Utils.DrawBoxFill(new Rectangle(-500, -500, 3000, 3000), new Color(223 / 255f, 243 / 255f, 247 / 255f));
            spriteBatch.Draw(TextureCache.Map, TextureCache.Map.Bounds, Color.White);
            Locations.Render(spriteBatch);
            boids.OnDraw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < UIScreenManager.Instance?.Components.Count; i++)
            {
                UIScreenManager.Instance.Components[i].active = true;

                UIScreenManager.Instance?.Components[i].Update();
                UIScreenManager.Instance?.Components[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
