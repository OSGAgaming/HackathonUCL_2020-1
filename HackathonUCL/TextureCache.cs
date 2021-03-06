using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace HackathonUCL
{
    // TODO dude.
#nullable disable
    public class TextureCache
    {
        public static Texture2D pixel;
        public static Texture2D Circle;
        public static Texture2D ClosedCircle;
        public static Texture2D Map;

        public static Texture2D Pause;
        public static Texture2D Forward;

        public static void LoadTextures(ContentManager content)
        {
            pixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            Circle = content.Load<Texture2D>("Textures/Circle");
            ClosedCircle = content.Load<Texture2D>("Textures/ClosedCircle");
            Map = content.Load<Texture2D>("Textures/Map");

            Pause = content.Load<Texture2D>("Textures/Pause");
            Forward = content.Load<Texture2D>("Textures/Forward");
        }
    }
}
