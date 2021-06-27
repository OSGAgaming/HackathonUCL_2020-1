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
    public class Location
    {
        public bool IsActive => LocationCache.CountryVariables.ContainsKey(Name);
        public bool IsZoomed => LocationHost.FocalLocation == Name;
        private Flock flock;
        private Flock flock2;

        public Vector2 DrawPosition { get; set; }
        public string Name { get; }
        public string GovernmentType { get; set; }

        public float TextAlpha = 0;
        public float Completion = 0;


        public int NumberOfCases;

        public bool IsGoingForward => Main.Locations.DeltaTime > 0;
        public int CacheCount = 20;
        public int Population;
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

        public List<float> InterpolationValueCache = new List<float>();

        public Vector2 GraphPos1 => Utils.MouseScreen.ToVector2() - new Vector2(150, 80);
        public Vector2 GraphPos2 => Utils.MouseScreen.ToVector2() - new Vector2(150, -80);
        public Vector2 GraphPos3 => Utils.MouseScreen.ToVector2() + new Vector2(100, 0);

        public virtual void Render(SpriteBatch sb)
        {
            float ProcessedCases = GetProcessedCases(GetTotalCases() + (float)Math.E);

            Utils.DrawClosedCircle(DrawPosition, ProcessedCases * 10, 0.2f, Color.Lerp(Color.Green, Color.DarkRed, ProcessedCases / 10f));
            float Stringency = GetStringency();

            flock.MaxVel = (100 - Stringency) / 1000f;
            flock.MaxForce = (100 - Stringency) / 200000f;
            flock.Vision = (100 - Stringency) / 4f;
            flock.Alpha = Completion;
            flock.Range = Stringency + 15;

            flock2.MaxVel = (100 - Stringency) / 1000f;
            flock2.MaxForce = (100 - Stringency) / 200000f;
            flock2.Vision = (100 - Stringency) / 4f;
            flock2.Alpha = Completion;
            flock2.Range = Stringency + 15;

            if (IsZoomed)
            {
                Completion += (1.4f - Completion) / 16f;                
            }
            else
            {
                Completion += (0 - Completion) / 16f;
            }

            if (Completion > 0.1f)
            {
                Vector2 GStyleVec = Utils.MouseScreen.ToVector2() + new Vector2(40, MathHelper.SmoothStep(0, 60, Completion));

                Utils.DrawText("Government Style:", Color.Black * Completion, GStyleVec, MathHelper.SmoothStep(0.2f, 0, Completion), new Vector2(0.8f));
                Color lerp = default;
                switch(GovernmentType)
                {
                    case "Flawed democracy":
                        lerp = Color.Lerp(Color.White, Color.Yellow, Completion);
                        break;
                    case "Authoritarian":
                        lerp = Color.Lerp(Color.White, Color.Red, Completion);
                        break;
                    case "Full democracy":
                        lerp = Color.Lerp(Color.White, Color.Green, Completion);
                        break;
                    default:
                        break;
                }
                Utils.DrawTextToLeft(GovernmentType, lerp * Completion, GStyleVec + new Vector2(40, 0), 0.8f, MathHelper.SmoothStep(0.2f, 0, Completion));

                Utils.DrawLine(GraphPos1, GraphPos1 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.1f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos1, GraphPos1 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.2f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos2, GraphPos2 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos2, GraphPos2 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.4f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos3, GraphPos3 - new Vector2(0, MathHelper.SmoothStep(0, 30, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos3, GraphPos3 + new Vector2(MathHelper.SmoothStep(0, 30, Completion - 0.4f), 0), Color.Black * Completion, 1);

                Utils.DrawText("Stringency", Color.Black * Completion, GraphPos1 + new Vector2(50, -80), MathHelper.SmoothStep(-0.1f, 0, Completion), new Vector2(0.8f));

                Utils.DrawText("Daily Cases", Color.Black * Completion, GraphPos2 + new Vector2(50, -80), MathHelper.SmoothStep(-0.2f, 0, Completion), new Vector2(0.8f));

                for (int i = 1; i < CacheCount; i++)
                {
                    int Index = StampIndex() - CacheCount + i;

                    if (Index - 1 >= 0)
                    {
                        float s1 = Info[Index].Stringency * 0.6f;
                        float s2 = Info[Index - 1].Stringency * 0.6f;

                        Vector2 v1 = new Vector2(GraphPos1.X + Index * 5, GraphPos1.Y - s1 - 1);
                        Vector2 v2 = new Vector2(GraphPos1.X + (Index - 1) * 5, GraphPos1.Y - s2 - 1);

                        Vector2 SmoothStepPos1 = new Vector2(MathHelper.SmoothStep(v1.X, v2.X, 1- InterpolationValue()), MathHelper.SmoothStep(v1.Y, v2.Y, 1 - InterpolationValue()) - 1);

                        float s1A = GetProcessedCases(History[Index].Data.Cases + 1) * 4f;
                        float s2A = GetProcessedCases(History[Index - 1].Data.Cases + 1) * 4f;

                        Vector2 v1A = new Vector2(GraphPos2.X + Index * 5, GraphPos2.Y - s1A);
                        Vector2 v2A = new Vector2(GraphPos2.X + (Index - 1) * 5, GraphPos2.Y - s2A);

                        Vector2 SmoothStepPos2 = new Vector2(MathHelper.SmoothStep(v1A.X, v2A.X, 1 - InterpolationValue()), MathHelper.SmoothStep(v1A.Y, v2A.Y, 1 - InterpolationValue()));


                        Vector2 v1B = new Vector2(GraphPos3.X + Index * 2, GraphPos3.Y - s1 * 0.5f - 1);
                        Vector2 v2B = new Vector2(GraphPos3.X + (Index - 1) * 2, GraphPos3.Y - s2 * 0.5f - 1);

                        Vector2 SmoothStepPos1B = new Vector2(MathHelper.SmoothStep(v1B.X, v2B.X, 1 - InterpolationValue()), MathHelper.SmoothStep(v1B.Y, v2B.Y, 1 - InterpolationValue()) - 1);

                        Vector2 v1AB = new Vector2(GraphPos3.X + Index * 2, GraphPos3.Y - s1A * 0.5f);
                        Vector2 v2AB = new Vector2(GraphPos3.X + (Index - 1) * 2, GraphPos3.Y - s2A * 0.5f);

                        Vector2 SmoothStepPos2B = new Vector2(MathHelper.SmoothStep(v1AB.X, v2AB.X, 1 - InterpolationValue()), MathHelper.SmoothStep(v1AB.Y, v2AB.Y, 1 - InterpolationValue()));

                        if (StampIndex() > CacheCount)
                        {
                            float Smoothen = MathHelper.SmoothStep(5, 0, InterpolationValue()) - (StampIndex() - CacheCount) * 5;
                            float Smoothen2 = MathHelper.SmoothStep(2, 0, InterpolationValue()) - (StampIndex() - CacheCount) * 2;

                            v1A.X += Smoothen;
                            v2A.X += Smoothen;

                            v1.X += Smoothen;
                            v2.X += Smoothen;

                            SmoothStepPos1.X += Smoothen;
                            SmoothStepPos2.X += Smoothen;

                            v1AB.X += Smoothen2;
                            v2AB.X += Smoothen2;

                            v1B.X += Smoothen2;
                            v2B.X += Smoothen2;

                            SmoothStepPos1B.X += Smoothen2;
                            SmoothStepPos2B.X += Smoothen2;
                        }

                        if (i == CacheCount - 1)
                        {
                            Utils.DrawLine(v2, SmoothStepPos1, Color.MediumPurple * Completion, 1);
                            Utils.DrawLine(v2A, SmoothStepPos2, Color.DarkBlue * Completion, 1);

                            Utils.DrawClosedCircle(SmoothStepPos1, 2f, Completion, Color.MediumPurple);
                            Utils.DrawClosedCircle(SmoothStepPos2, 2f, Completion, Color.DarkBlue);
                        }
                        else
                        {
                            Utils.DrawLine(v1, v2, Color.MediumPurple * Completion, 1);
                            Utils.DrawLine(v1A, v2A, Color.DarkBlue * Completion, 1);
                        }

                        if (i == CacheCount - 1)
                        {
                            Utils.DrawLine(v2B, SmoothStepPos1B, Color.Red * Completion, 1);
                            Utils.DrawLine(v2AB, SmoothStepPos2B, Color.Maroon * Completion, 1);

                            Utils.DrawClosedCircle(SmoothStepPos1B, 2f, Completion, Color.Red);
                            Utils.DrawClosedCircle(SmoothStepPos2B, 2f, Completion, Color.Maroon);
                        }
                        else
                        {
                            Utils.DrawLine(v1B, v2B, Color.Red * Completion, 1);
                            Utils.DrawLine(v1AB, v2AB, Color.Maroon * Completion, 1);
                        }
                    }
                }
            }

            if (Main.Locations.Time % LocationHost.TIMESTEP <= LocationHost.SPEEDOFSIMULATION && Main.Locations.DeltaTime != 0)
            {
                InterpolationValueCache = new List<float>();
                for (int i = 0; i < CacheCount; i++)
                {
                    if (StampIndex() - CacheCount + i >= 0)
                        InterpolationValueCache.Add(GetProcessedCases(Info[StampIndex() - CacheCount + i].NumberOfCases) * 10);
                }
            }

            if ((Utils.MouseScreen.ToVector2() - DrawPosition).LengthSquared() <= 50 * 50 && !LocationHost.HasHover)
            {
                TextAlpha += (1 - TextAlpha) / 16f;

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    LocationHost.FocalLocation = Name;
                }

                LocationHost.HasHover = true;
            }
            else
            {
                TextAlpha += (0 - TextAlpha) / 16f;
            }
            Color c = Color.Black * TextAlpha;
            Vector2 mousep = Utils.MouseScreen.ToVector2();

            Utils.DrawText(Name, c, mousep - new Vector2(-20, 100 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));

            Utils.DrawText("Population: " + Population.ToString(), c, mousep - new Vector2(-20, 85 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            Utils.DrawText("Cases Today: " + Math.Round(GetCases()).ToString(), c, mousep - new Vector2(-20, 70 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            Utils.DrawText("Total Cases: " + Math.Round(GetTotalCases()).ToString(), c, mousep - new Vector2(-20, 55 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));

            if (PreviousModifier != LocationHost.StringencyModifier) Recalculate();

            PreviousModifier = LocationHost.StringencyModifier;
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
                    hI.Stringency = History[0].Data.Stringency * (LocationHost.StringencyModifier * LocationHost.StringencyModifier);

                    Info.Add(hI);
                }
                else
                {
                    HoverInfo hI = new HoverInfo();
                    hI.CountryName = Name;

                    int Coeficient = 20;
                    int Factor = (int)(History[i].Data.Stringency * (1 - LocationHost.StringencyModifier)) * Coeficient;

                    hI.NumberOfCases = Math.Max(Info[i - 1].NumberOfCases + History[i].Data.Cases + Factor, 0);
                    hI.Stringency = History[i].Data.Stringency * (LocationHost.StringencyModifier * LocationHost.StringencyModifier);

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
