using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public struct HoverInfo
    {
        public string CountryName;
        public int Population;
        public int NumberOfCases;
        public float Stringency;
        public string GovernmentalSystem;
    }
    public class LocationHost
    {
        public const float SPEEDOFSIMULATION = 0.1f;
        public const int NUMBEROFDATAPOINTS = 100;
        public const float TIMESTEP = 10;

        public static bool IsPlaying;
        public static float StringencyModifier = 1;

        public List<Location> Locations = new List<Location>();
        public Vector2 GraphPosition => new Vector2(10, Utils.ScreenSize.Y - 10);

        public static float GraphInterp;
        public static float TotalTime => NUMBEROFDATAPOINTS * TIMESTEP;
        public static string FocalLocation = "";
        public static int FocalIndex = -1;
        public static bool HasHover = false;

        public int DeltaFetchLength;
        public float DeltaTime;
        public float PreviousTime;
        public float Time { get; set; }

        public void AppendLocation(string Name, Vector2 position, int Population = 0, string GT = "")
        {
            Location l = new Location(Name, position, Population);
            l.GovernmentType = GT;
            Locations.Add(l);
        }

        public void Render(SpriteBatch sb)
        {
            if (GraphInterp < 1) GraphInterp += 0.01f;

            int FetchLength = 0;
            foreach (Location l in Locations)
            {
                l.Render(sb);
                FetchLength = l.InterpolationValueCache.Count;
            }

            if (DeltaFetchLength != FetchLength) GraphInterp = 0;

            uint ColorPicker = 0;

            foreach (Location l in Locations)
            {
                ColorPicker++;
                Color c = new Color(((ColorPicker * 800) % 255) / 255f, ((ColorPicker * 400) % 255) / 255f, ((ColorPicker * 200) % 255) / 255f);

                Utils.DrawBoxFill(
                new Rectangle(
                Utils.ScreenSize.X - 60,
                100 + (int)ColorPicker * 50,
                30,
                20).Inf(1, 1), Color.White);

                Utils.DrawBoxFill(
                new Rectangle(
                Utils.ScreenSize.X - 60,
                100 + (int)ColorPicker * 50,
                30,
                20), c);

                Utils.DrawTextFromCenter(l.Name, Color.Black, new Vector2(Utils.ScreenSize.X - 170, 100 + (int)ColorPicker * 50));

                if (FetchLength > 1)
                {
                    for (int i = 1; i < FetchLength; i++)
                    {
                        float v1 = l.InterpolationValueCache[i - 1];
                        float v2 = l.InterpolationValueCache[i];

                        Vector2 g1 = new Vector2(GraphPosition.X + (i - 1) * 20, GraphPosition.Y - v1);
                        Vector2 g2 = new Vector2(GraphPosition.X + i * 20, GraphPosition.Y - v2);

                        if (i == FetchLength - 1)
                        {
                            Vector2 SmoothStepPos = new Vector2(MathHelper.SmoothStep(g1.X, g2.X, GraphInterp), MathHelper.SmoothStep(g1.Y, g2.Y, GraphInterp));

                            Utils.DrawLine(g1, SmoothStepPos, c, 1);
                        }
                        else
                        {
                            Utils.DrawLine(g1, g2, c, 1);
                        }
                    }
                }
            }

            Utils.DrawLine(GraphPosition, GraphPosition - new Vector2(0, MathHelper.SmoothStep(0, 200, Main.GlobalTimer / 100f)), Color.Black, 2);
            Utils.DrawLine(GraphPosition, GraphPosition + new Vector2(MathHelper.SmoothStep(0, 400, Main.GlobalTimer / 100f), 0), Color.Black, 2);

            Utils.DrawText("Total Cases", Color.Black * MathHelper.SmoothStep(0, 1, Main.GlobalTimer / 100f), GraphPosition + new Vector2(MathHelper.SmoothStep(0, 50, Main.GlobalTimer / 100f), -250), 0);

            DeltaFetchLength = FetchLength;
        }


        public void Update()
        {
            DeltaTime = Time - PreviousTime;

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                FocalLocation = "";
                FocalIndex = -1;
            }

            if (FocalIndex == -1 && FocalLocation != "")
            {
                for (int i = 0; i < Locations.Count; i++)
                {
                    if (Locations[i].Name == FocalLocation)
                    {
                        FocalIndex = i;
                        break;
                    }
                }

            }
            PreviousTime = Time;

            if (IsPlaying && Time < TotalTime) Time += SPEEDOFSIMULATION;

            if (FocalIndex != -1)
            {
                Main.mainCamera.CamPos += (Locations[FocalIndex].DrawPosition - Utils.ScreenSize.ToVector2() / (2 * Main.mainCamera.scale) - Main.mainCamera.CamPos) / 64f;
                Main.mainCamera.scale += (2 - Main.mainCamera.scale) / 64f;
            }
            else
            {
                Main.mainCamera.CamPos += (Vector2.Zero - Main.mainCamera.CamPos) / 64f;
                Main.mainCamera.scale += (1 - Main.mainCamera.scale) / 20f;
            }

            HasHover = false;
        }
    }
}
