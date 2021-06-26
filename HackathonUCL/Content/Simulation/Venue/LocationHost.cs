using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int Stringency;
        public string GovernmentalSystem;
    }
    public class LocationHost
    {
        public const float SPEEDOFSIMULATION = 0.1f;
        public const float TIMESTEP = 10;

        public List<Location> Locations = new List<Location>();
        public Vector2 GraphPosition => new Vector2(50, 460);

        public float Time { get; set; }
        public float GraphInterp;

        public int DeltaFetchLength;
        public void AppendLocation(string Name, Vector2 position, int Population = 0)
        {
            Location l = new Location(Name, position);
            l.Population = Population;
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
                if (FetchLength > 1)
                {
                    for (int i = 1; i < FetchLength; i++)
                    {
                        float v1 = l.InterpolationValueCache[i - 1];
                        float v2 = l.InterpolationValueCache[i];

                        Vector2 g1 = new Vector2(GraphPosition.X + (i - 1) * 10, GraphPosition.Y - v1 * 2);
                        Vector2 g2 = new Vector2(GraphPosition.X + i * 10, GraphPosition.Y - v2 * 2);

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
            Utils.DrawLine(GraphPosition, GraphPosition + new Vector2(MathHelper.SmoothStep(0, 200, Main.GlobalTimer / 100f), 0), Color.Black, 2);

            DeltaFetchLength = FetchLength;
        }

        public void Update() => Time += SPEEDOFSIMULATION;
    }
}
