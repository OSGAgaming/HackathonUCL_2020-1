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
        public void UpdateGraph()
        {
            if (Main.Locations.Time % LocationHost.TIMESTEP <= LocationHost.SPEEDOFSIMULATION && Main.Locations.DeltaTime != 0)
            {
                InterpolationValueCache = new List<float>();
                for (int i = 0; i < CacheCount; i++)
                {
                    if (StampIndex() - CacheCount + i >= 0)
                        InterpolationValueCache.Add(GetProcessedCases(Info[StampIndex() - CacheCount + i].NumberOfCases) * 10);
                }
            }
        }
    }
}
