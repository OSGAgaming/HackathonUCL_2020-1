
using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace HackathonUCL
{
    public static partial class Utils
    {
        public static void DrawCircle(Vector2 pos, float rad, float alpha = 1f)
        {
            Point point = new Point((int)pos.X - (int)rad, (int)pos.Y - (int)rad);
            Main.spriteBatch.Draw(TextureCache.Circle, new Rectangle(point, new Point((int)rad *2, (int)rad *2)), Color.Red* alpha);
        }

        public static void DrawClosedCircle(Vector2 pos, float rad, float alpha = 1f, Color color = default)
        {
            Texture2D tex = TextureCache.ClosedCircle;

            Main.spriteBatch.Draw(tex, pos, tex.Bounds, color * alpha, 0f, tex.TextureCenter(), rad / tex.Width, SpriteEffects.None, 0f);
        }
        public static bool isClicking => Mouse.GetState().LeftButton == ButtonState.Pressed;
        public static Point MouseScreen => Mouse.GetState().Position;
        public static Point ScreenSize => new Point(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height);
        public static Point ScreenCenter => new Point(Main.graphics.GraphicsDevice.Viewport.Width/2, Main.graphics.GraphicsDevice.Viewport.Height/2);
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static void QuickApplyShader(Effect effect)
        => effect?.CurrentTechnique.Passes[0].Apply();
        public static void QuickApplyShader(Effect effect, params float[] yo)
        {
            effect?.CurrentTechnique.Passes[0].Apply();
            for (int i = 0; i < yo.Length; i++)
            {
                effect?.Parameters[i]?.SetValue(yo[i]);
            }
        }
        public static Vector2 ScreenPerc(Vector2 perc) => Vector2.Multiply(ScreenSize.ToVector2(), perc); 
        public static void DrawPixel(Vector2 pos, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, pos, tint);
        public static void DrawBoxFill(Vector2 pos, int width, int height, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, pos, new Rectangle(0, 0, width, height), tint);
        public static void DrawBoxFill(Rectangle rectangle, Color tint) => Main.spriteBatch.Draw(TextureCache.pixel, rectangle.Location.ToVector2(), new Rectangle(0, 0, rectangle.Width, rectangle.Height), tint);
        public static void DrawLine(Vector2 p1, Vector2 p2, Color tint, float lineWidth = 1f)
        {
            Vector2 between = p2 - p1;
            float length = between.Length();
            float rotation = (float)Math.Atan2(between.Y, between.X);
            Main.spriteBatch.Draw(TextureCache.pixel, p1, null, tint, rotation, new Vector2(0f, 0.5f), new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public static void DrawText(string text, Color colour, Vector2 position, float rotation = 0f, Vector2 scale = default)
        {
            if (scale == default)
                scale = Vector2.One;
            SpriteFont font = Main.font;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void DrawTextFromCenter(string text, Color colour, Vector2 position, float rotation = 0f, Vector2 scale = default)
        {
            if (scale == default)
                scale = Vector2.One;
            SpriteFont font = Main.font;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y) + textSize / 2, colour, rotation, textSize/2, scale, SpriteEffects.None, 0f);
        }
        public static float DrawTextToLeft(string text, Color colour, Vector2 position)
        {
            SpriteFont font = Main.font;
            float textPositionLeft = position.X;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            return font.MeasureString(text).X;
        }

        public static void DrawSquare(Vector2 point, float size, Color color)
        {
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X, point.Y + size), color);
            DrawLine(new Vector2(point.X + size, point.Y + size), new Vector2(point.X + size, point.Y), color);
            DrawLine(point, new Vector2(point.X, point.Y + size), color);
            DrawLine(point, new Vector2(point.X + size, point.Y), color);
        }

        public static void DrawRectangle(Vector2 point, float sizeX, float sizeY, Color color, float thickness = 1)
        {
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
        public static void DrawRectangle(Rectangle rectangle, Color color, float thickness = 1)
        {
            Vector2 point = rectangle.Location.ToVector2();
            int sizeX = rectangle.Size.X;
            int sizeY = rectangle.Size.Y;
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(new Vector2(point.X + sizeX, point.Y + sizeY), new Vector2(point.X + sizeX, point.Y), color, thickness);
            DrawLine(point, new Vector2(point.X, point.Y + sizeY), color, thickness);
            DrawLine(point, new Vector2(point.X + sizeX, point.Y), color, thickness);
        }
    }
}
