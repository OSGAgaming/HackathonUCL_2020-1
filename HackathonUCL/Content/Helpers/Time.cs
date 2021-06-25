using HackathonUCL;
using Microsoft.Xna.Framework;
using System;

namespace HackathonUCL
{
    public static class Time
    {
        public static double DeltaTime(this GameTime gt) => gt.ElapsedGameTime.TotalSeconds;
        public static float SineTime(float period) => (float)Math.Sin(Main.gameTime.TotalGameTime.TotalSeconds * period);
        public static float CosTime(float period) => (float)Math.Cos(Main.gameTime.TotalGameTime.TotalSeconds * period);
        public static float DeltaT => (float)Main.gameTime.DeltaTime();

        public static float DeltaVar(float mult) => (float)Main.gameTime.DeltaTime() * mult;

        public static float DeltaTimeRoundedVar(float mult, int nearest) => Utils.Round(mult, nearest);
        public static float TotalTimeMil => (float)Main.gameTime.TotalGameTime.TotalMilliseconds;
        public static float TotalTimeSec => (float)Main.gameTime.TotalGameTime.TotalSeconds;

        public static float QuickDelta => (float)Main.gameTime.DeltaTime() * 60;
    }
}
