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
        /// <summary>
        /// really messy lol
        /// </summary>

        public float DILerp = 0;
        public void ZoomState()
        {
            if (IsZoomed)
            {
                Completion += (1.4f - Completion) / 16f;
                DILerp += (1 - DILerp) / 32f;
            }
            else
            {
                Completion += (0 - Completion) / 16f;
                DILerp += (0 - DILerp) / 32f;
            }

            if (Completion > 0.1f)
            {
                Vector2 GStyleVec = Utils.MouseScreen.ToVector2() + new Vector2(40, MathHelper.SmoothStep(0, 60, Completion));

                Utils.DrawText("Government Style:", Color.Black * Completion, GStyleVec, MathHelper.SmoothStep(0.2f, 0, Completion), new Vector2(0.65f));
                Color lerp = default;

                switch (GovernmentType)
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

                int Height = 70;
                int VHeight = (int)MathHelper.SmoothStep(0,Height * (DemocracyIndex / 10f), DILerp);

                Utils.DrawBoxFill(new Rectangle((GraphPos3 - new Vector2(0,75 + VHeight)).ToPoint(), new Point(10, VHeight)), Color.Lerp(Color.Red, Color.LimeGreen, VHeight / 100f));
                Utils.DrawRectangle(new Rectangle((GraphPos3 - new Vector2(0, 75 + Height)).ToPoint(), new Point(10, Height)).Inf(1,1), Color.Black * DILerp);

                Utils.DrawText("Demomcracy Index", Color.Black * DILerp, (GraphPos3 - new Vector2(20, VHeight + 75)), MathHelper.SmoothStep(-0.1f, 0, DILerp), new Vector2(0.6f));
                Utils.DrawText(Math.Round(MathHelper.SmoothStep(0, DemocracyIndex, DILerp), 1).ToString(), Color.Black * DILerp, (GraphPos3 - new Vector2(20, VHeight + 60)), MathHelper.SmoothStep(0.1f, 0, DILerp), new Vector2(0.6f));

                Utils.DrawTextToLeft(GovernmentType, lerp * Completion, GStyleVec + new Vector2(20, 0), 0.65f, MathHelper.SmoothStep(0.2f, 0, Completion));

                Utils.DrawLine(GraphPos1, GraphPos1 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.1f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos1, GraphPos1 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.2f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos2, GraphPos2 - new Vector2(0, MathHelper.SmoothStep(0, 50, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos2, GraphPos2 + new Vector2(MathHelper.SmoothStep(0, 90, Completion - 0.4f), 0), Color.Black * Completion, 1);

                Utils.DrawLine(GraphPos3, GraphPos3 - new Vector2(0, MathHelper.SmoothStep(0, 30, Completion - 0.3f)), Color.Black * Completion, 1);
                Utils.DrawLine(GraphPos3, GraphPos3 + new Vector2(MathHelper.SmoothStep(0, 30, Completion - 0.4f), 0), Color.Black * Completion, 1);

                Utils.DrawText("Stringency", Color.Black * Completion, GraphPos1 + new Vector2(50, -80), MathHelper.SmoothStep(-0.1f, 0, Completion), new Vector2(0.8f));

                Utils.DrawText("Daily Cases", Color.Black * Completion, GraphPos2 + new Vector2(50, -60), MathHelper.SmoothStep(-0.2f, 0, Completion), new Vector2(0.8f));

                for (int i = 1; i < CacheCount; i++)
                {
                    int Index = StampIndex() - CacheCount + i;

                    if (Index - 1 >= 0)
                    {
                        //Time crunch :)

                        float s1 = Info[Index].Stringency * 0.5f;
                        float s2 = Info[Index - 1].Stringency * 0.5f;

                        Vector2 v1 = new Vector2(GraphPos1.X + Index * 5, GraphPos1.Y - s1 - 1);
                        Vector2 v2 = new Vector2(GraphPos1.X + (Index - 1) * 5, GraphPos1.Y - s2 - 1);

                        Vector2 SmoothStepPos1 = new Vector2(MathHelper.SmoothStep(v1.X, v2.X, 1 - InterpolationValue()), MathHelper.SmoothStep(v1.Y, v2.Y, 1 - InterpolationValue()) - 1);

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
        }

        public void UpdateZoomFields()
        {
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
        }

    }
}
