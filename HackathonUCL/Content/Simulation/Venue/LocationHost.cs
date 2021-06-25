using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public class LocationHost
    {
        public List<Location> Locations = new List<Location>();

        public float Time { get; set; }
        public void AppendLocation(string Name, Vector2 position) => Locations.Add(new Location(Name, position));

        public void Render(SpriteBatch sb)
        {
            foreach(Location l in Locations)
            {
                l.Render(sb);
            }
        }
    }
}
