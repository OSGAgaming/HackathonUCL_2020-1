using Microsoft.Xna.Framework;
using System;

namespace HackathonUCL
{
    public static class Extensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("min cannot be greater than max.");
            }

            return min + (float)random.NextDouble() * (max - min);
        }
        public static float NextFloat(this Random random, float max)
        {
            return (float)(random.NextDouble() * max);
        }
        public static bool IsBetween(this float num, float min, float max)
        {
            return num > min && num < max;
        }
        public static float ReciprocateTo(this float num, float target, float ease = 16f)
        {
            return num + (target - num) / ease;
        }
        public static float ReciprocateTo(this int num, float target, float ease)
        {
            return (target - num - 16) / ease;
        }
        public static Color ReciprocateTo(this Color num, Color target, float ease = 16f)
        {
            return Color.Lerp(num, target, 1 / ease);
        }
        public static void Shuffle<T>(this Random random, ref T[] input)
        {
            for (int i = input.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);

                T value = input[index];
                input[index] = input[i];
                input[i] = value;
            }
        }
        public static Vector2 ReciprocateTo(this Vector2 v, Vector2 target, float ease = 16f)
        {
            return v + (target - v) / ease;
        }

        public static Vector2 SHMTo(this Vector2 currentVelocity, Vector2 orig, Vector2 dest, float ease = 16f, float dampening = 10f)
        {
            return (dest - orig) / ease - currentVelocity / dampening;
        }
        public static Vector2 ReciprocateToInt(this Vector2 v, Vector2 target, float ease = 16f)
        {
            return v + new Vector2((int)((target - v) / ease).X, (int)((target - v) / ease).Y);
        }
        public static Vector2 Snap(this Vector2 v, int snap) => new Vector2((int)(v.X / snap) * snap, (int)(v.Y / snap) * snap);
      
        public static Rectangle Inf(this Rectangle R,int h, int v)
        {
            return new Rectangle(
                new Point(R.X - h, R.Y - v), 
                new Point(R.Width + h*2, R.Height + v*2));
        }
        public static float Slope(this Vector2 v)
        {
            return v.Y / v.X;
        }

    }
}
