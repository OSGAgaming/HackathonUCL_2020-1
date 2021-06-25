using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public class Location
    {
        public Vector2 Position { get; set; }

        public string Name { get; }

        public int NumberOfCases;

        //TODO: Find a better calc for this lmao
        public float CalculateRadius(int n) => n;
       
        public virtual void Render(SpriteBatch sb)
        {
            Utils.DrawClosedCircle(Position, CalculateRadius(NumberOfCases), 0.2f, Color.Red);
        }

        public Location(string Name, Vector2 Position)
        {
            this.Name = Name;
            this.Position = Position;

            NumberOfCases = Main.rand.Next(10,40);
        }
    }
}
