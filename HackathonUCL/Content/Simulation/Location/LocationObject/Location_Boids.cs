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
        private Flock flock;
        private Flock flock2;

        public void ManageBoids()
        {
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
        }
    }
}
