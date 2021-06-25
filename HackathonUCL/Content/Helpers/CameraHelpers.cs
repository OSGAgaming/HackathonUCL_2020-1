using HackathonUCL;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace HackathonUCL
{
    public static partial class Utils
    {
        public static void ReciprocateCameraCenterTo(Vector2 position, float ease = 16f) => Main.mainCamera.CameraCenter = Main.mainCamera.CameraCenter.ReciprocateTo(position, ease);

        public static void ReciprocateCameraPositionTo(Vector2 position, float ease = 16f) => Main.mainCamera.CamPos = Main.mainCamera.CamPos.ReciprocateTo(position, ease);
    }
}
