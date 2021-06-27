using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public partial class Location
    {
        public bool IsActive => LocationCache.CountryVariables.ContainsKey(Name);
        public bool IsZoomed => LocationHost.FocalLocation == Name;

        public Vector2 DrawPosition { get; set; }
        public string Name { get; }
        public string GovernmentType { get; set; }

        public float TextAlpha = 0;
        public float Completion = 0;
        public int CacheCount = 20;
        public int Population;
        public float DemocracyIndex;
        public List<TimeStamp> History = new List<TimeStamp>();
        public List<HoverInfo> Info = new List<HoverInfo>();

        public float CalculateRadius(float r) => r;
        public int StampIndex()
        {
            int index = 0;

            for (int i = 0; i < History.Count; i++)
            {
                if (Main.Locations.Time >= History[i].Time) index = i;
            }

            return Math.Clamp(index, 0, History.Count - 2);
        }

        public float InterpolationValue() => (Main.Locations.Time - History[StampIndex()].Time) / (History[StampIndex() + 1].Time - History[StampIndex()].Time);

        public float GetCases() => CalculateRadius(MathHelper.SmoothStep(History[StampIndex()].Data.Cases, History[StampIndex() + 1].Data.Cases, InterpolationValue()));

        public float GetTotalCases() => CalculateRadius(MathHelper.SmoothStep(Info[StampIndex()].NumberOfCases, Info[StampIndex() + 1].NumberOfCases, InterpolationValue()));

        public float GetProcessedCases(float n) => (float)Math.Log(n);

        public float GetStringency() => CalculateRadius(MathHelper.SmoothStep(Info[StampIndex()].Stringency, Info[StampIndex() + 1].Stringency, InterpolationValue()));

        public float GetPopulation() => CalculateRadius(MathHelper.SmoothStep(Info[StampIndex()].Population, Info[StampIndex() + 1].Population, InterpolationValue()));

        public float GetDeaths() => CalculateRadius(MathHelper.SmoothStep(History[StampIndex()].Data.Deaths, History[StampIndex() + 1].Data.Deaths, InterpolationValue()));



        public List<float> InterpolationValueCache = new List<float>();

        public Vector2 GraphPos1 => Utils.MouseScreen.ToVector2() - new Vector2(150, 80);
        public Vector2 GraphPos2 => Utils.MouseScreen.ToVector2() - new Vector2(150, -80);
        public Vector2 GraphPos3 => Utils.MouseScreen.ToVector2() + new Vector2(100, 0);

        public virtual void Render(SpriteBatch sb)
        {
            float ProcessedCases = GetProcessedCases(GetTotalCases() + (float)Math.E);

            Utils.DrawClosedCircle(DrawPosition, ProcessedCases * 10, 0.2f, Color.Lerp(Color.Green, Color.DarkRed, ProcessedCases / 10f));

            ManageBoids();
            UpdateZoomFields();
            ZoomState();
            UpdateGraph();
            OnHover();

            if (PreviousModifier != (LocationHost.StringencyModifier + LocationHost.DemocracyModifier)) Recalculate();

            PreviousModifier = LocationHost.StringencyModifier + LocationHost.DemocracyModifier;
        }

        public float PreviousModifier;
        public void Recalculate()
        {
            Info = new List<HoverInfo>();

            for (int i = 0; i < History.Count; i++)
            {
                if (i == 0)
                {
                    HoverInfo hI = new HoverInfo();
                    hI.CountryName = Name;
                    hI.NumberOfCases = History[0].Data.Cases;
                    hI.Deaths = History[0].Data.Deaths;
                    hI.Population = Population;
                    hI.Stringency = Math.Clamp(History[0].Data.Stringency * (LocationHost.StringencyModifier * LocationHost.StringencyModifier), 0, 100);

                    Info.Add(hI);
                }
                else
                {
                    HoverInfo hI = new HoverInfo();
                    hI.CountryName = Name;

                    int Coeficient = 5;
                    int Factor = (int)(History[i].Data.Stringency * (1 - LocationHost.StringencyModifier)) * Coeficient + (int)((LocationHost.DemocracyModifier - 1) * Info[i - 1].NumberOfCases * 0.01f);

                    hI.NumberOfCases = Math.Max(Info[i - 1].NumberOfCases + History[i].Data.Cases + Factor, 0);
                    hI.Stringency = Math.Clamp(History[i].Data.Stringency * (LocationHost.StringencyModifier * LocationHost.StringencyModifier),0,100);
                    hI.Deaths = Math.Max(Info[i - 1].Deaths + History[i].Data.Deaths + Factor / 2, 0);
                    hI.Population = Info[i - 1].Population - History[i].Data.Deaths;

                    Info.Add(hI);
                }
            }

            InterpolationValueCache = new List<float>();
            for (int i = 0; i < CacheCount; i++)
            {
                if (StampIndex() - CacheCount + i >= 0)
                    InterpolationValueCache.Add(GetProcessedCases(Info[StampIndex() - CacheCount + i].NumberOfCases + (float)Math.E) * 10);
            }
        }
        public Location(string Name, Vector2 DrawPosition, int Population)
        {
            this.Name = Name;
            this.DrawPosition = DrawPosition;
            this.Population = Population;

            if (IsActive)
            {
                History = LocationCache.CountryVariables[Name];
                Recalculate();

                flock = new Flock("Particles/Coralfin", 1f, 5, 15);
                Main.boids.fishflocks.Add(flock);

                flock.Populate(DrawPosition + new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), (int)Math.Log10(Population) * 2, 10, this);

                flock2 = new Flock("Particles/Coralfin", 1f, 5, 15);
                Main.boids.fishflocks.Add(flock2);

                flock2.Populate(DrawPosition + new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), (int)Math.Log10(Population) * 2, 10, this);
            }

        }
    }
}
