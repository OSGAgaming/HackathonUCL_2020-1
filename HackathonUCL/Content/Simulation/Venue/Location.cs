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
        Flock flock;
        public Vector2 DrawPosition { get; set; }
        public float TextAlpha = 0;
        public float Completion = 0;

        public string Name { get; }

        public int NumberOfCases;
        public bool IsGoingForward => Main.Locations.DeltaTime > 0;
        public int CacheCount = 20;
        public int Population;
        public List<TimeStamp> History = new List<TimeStamp>();
        public List<HoverInfo> Info = new List<HoverInfo>();
        //TODO: Find a better calc for this lmao

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

        public float GetStringency() => CalculateRadius(MathHelper.SmoothStep(Info[StampIndex()].Stringency, Info[StampIndex() + 1].Stringency, InterpolationValue()));

        public List<float> InterpolationValueCache = new List<float>();

        public Vector2 GraphPos1 => Utils.MouseScreen.ToVector2() - new Vector2(150, 80);
        public Vector2 GraphPos2 => Utils.MouseScreen.ToVector2() - new Vector2(150, -80);
        public Vector2 GraphPos3 => Utils.MouseScreen.ToVector2() + new Vector2(100, 0);

        public virtual void Render(SpriteBatch sb)
        {
            Utils.DrawClosedCircle(DrawPosition, GetTotalCases() * 0.05f, 0.2f, Color.Red);

            float Stringency = GetStringency();

            flock.MaxVel = (100 - Stringency) / 500f;
            flock.MaxForce = (100 - Stringency) / 100000f;
            flock.Vision = (100 - Stringency) / 4f;
            flock.Alpha = Completion;
            flock.Range = Stringency + 5;

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
                Utils.DrawLine(GraphPos1, GraphPos1 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.1f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos1, GraphPos1 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.2f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos2, GraphPos2 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos2, GraphPos2 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.4f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos3, GraphPos3 - new Vector2(0, MathHelper.SmoothStep(0, 30, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos3, GraphPos3 + new Vector2(MathHelper.SmoothStep(0, 30, Completion - 0.4f), 0), Color.Black * Completion, 1);

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

                        float s1A = History[Index].Data.Cases * 0.2f;
                        float s2A = History[Index - 1].Data.Cases * 0.2f;

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
                        InterpolationValueCache.Add(Info[StampIndex() - CacheCount + i].NumberOfCases);
                }
            }

            if ((Utils.MouseScreen.ToVector2() - DrawPosition).LengthSquared() <= 50 * 50)
            {
                TextAlpha += (1 - TextAlpha) / 16f;

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    LocationHost.FocalLocation = Name;
                }
            }
            else
            {
                TextAlpha += (0 - TextAlpha) / 16f;
            }
            Color c = Color.Black * TextAlpha;
            Vector2 mousep = Utils.MouseScreen.ToVector2();

            Utils.DrawText(Name, c, mousep - new Vector2(-20, 100 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            Utils.DrawText(Population.ToString(), c, mousep - new Vector2(-20, 85 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            Utils.DrawText(Math.Round(GetCases()).ToString(), c, mousep - new Vector2(-20, 70 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
        }

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
                    hI.Stringency = History[0].Data.Stringency;

                    Info.Add(hI);
                }
                else
                {
                    HoverInfo hI = new HoverInfo();
                    hI.CountryName = Name;
                    hI.NumberOfCases = Info[i - 1].NumberOfCases + History[i].Data.Cases;
                    hI.Stringency = History[i].Data.Stringency;

                    Info.Add(hI);
                }
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

                flock.Populate(DrawPosition + new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), (int)Math.Log10(Population), 10, this);
            }

        }
    }
}
