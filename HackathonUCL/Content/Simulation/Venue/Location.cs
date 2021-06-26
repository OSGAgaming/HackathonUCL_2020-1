using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public class Location
    {
        public Vector2 DrawPosition { get; set; }
        public float TextAlpha = 0;
        public string Name { get; }

        public int NumberOfCases;
        public int Population;
        public List<TimeStamp> History = new List<TimeStamp>();

        //TODO: Find a better calc for this lmao

        public float CalculateRadius(float r) => r;
        public int StampIndex()
        {
            int index = 0;

            for(int i = 0; i < History.Count; i++)
            {
                if (Main.Locations.Time >= History[i].Time) index = i;
            }

            return Math.Clamp(index, 0, History.Count - 2);
        }

        public float InterpolationValue() => (Main.Locations.Time - History[StampIndex()].Time) / (History[StampIndex() + 1].Time - History[StampIndex()].Time);

        public float GetCases() => CalculateRadius(MathHelper.SmoothStep(History[StampIndex()].Data.Cases, History[StampIndex() + 1].Data.Cases, InterpolationValue()));

        public List<float> InterpolationValueCache = new List<float>();

        public virtual void Render(SpriteBatch sb)
        {
            Utils.DrawClosedCircle(DrawPosition, GetCases(), 0.2f, Color.Red);

            if(Main.Locations.Time % LocationHost.TIMESTEP <= LocationHost.SPEEDOFSIMULATION)
            {
                InterpolationValueCache.Add(GetCases());
            }

            if((Mouse.GetState().Position.ToVector2() - DrawPosition).LengthSquared() <= 50 * 50)
            {
                TextAlpha += (1 - TextAlpha) / 16f;
            }
            else
            {
                TextAlpha += (0 - TextAlpha) / 16f;
            }
            Color c = Color.Black * TextAlpha;

            Utils.DrawText(Name, c, Mouse.GetState().Position.ToVector2() - new Vector2(-20, 100));
            Utils.DrawText(Population.ToString(), c, Mouse.GetState().Position.ToVector2() - new Vector2(-20, 85));
            Utils.DrawText(Math.Round(GetCases()).ToString(), c, Mouse.GetState().Position.ToVector2() - new Vector2(-20, 70));
        }

        public Location(string Name, Vector2 Position)
        {
            this.Name = Name;
            this.DrawPosition = Position;

            NumberOfCases = Main.rand.Next(10,40);

            for(int i = 0; i < 40; i++)
            History.Add(new TimeStamp(i * 10, new DataPoint(Main.rand.Next(10,100),0f)));
        }
    }
}
