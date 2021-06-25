using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public static class LocationCache
    {
        public static void LoadLocations()
        {
            Main.Locations.AppendLocation("Nigeria", new Vector2(500,500));
            Main.Locations.AppendLocation("India", new Vector2(300, 200));
            Main.Locations.AppendLocation("UK", new Vector2(200, 800));
        }
    }
}
