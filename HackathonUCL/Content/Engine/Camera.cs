using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HackathonUCL
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public float scale { get; set; } = 1;
        public float rotation { get; set; }

        public static int screenShake;

        public Vector2 CamPos
        {
            get => CameraCenter - new Vector2(Utils.ScreenSize.X / 2, Utils.ScreenSize.Y / 2) / scale;
            set => CameraCenter = value + new Vector2(Utils.ScreenSize.X / 2, Utils.ScreenSize.Y / 2) / scale;
        }
        public Vector3 GetScreenScale()
        {
            var scaleX = 1;
            var scaleY = 1;
            return new Vector3(scaleX * scale, scaleY * scale, 1.0f);
        }

        public Vector2 CameraCenter;
        public Vector2 offset;
        public void Invoke()
        {
            //Temporarily here only
            if (screenShake > 0) screenShake--;

            var shake = new Vector2(Main.rand.Next(-screenShake, screenShake), Main.rand.Next(-screenShake, screenShake));

            scale = Math.Clamp(scale, 0.3f, 5);

            CamPos = new Vector2(Math.Clamp(CamPos.X, 0, Utils.ScreenSize.X - Utils.ScreenSize.X / scale), Math.Clamp(CamPos.Y,0, Utils.ScreenSize.Y - Utils.ScreenSize.Y / scale));

            Transform =
                 Matrix.CreateTranslation(new Vector3(-CameraCenter + shake, 0)) *
                 Matrix.CreateScale(GetScreenScale()) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateTranslation(Utils.ScreenSize.X / 2, Utils.ScreenSize.Y / 2, 0);
        }

    }
}
