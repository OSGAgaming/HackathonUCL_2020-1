using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public struct TimeStamp
    {
        public float Time;
        public DataPoint Data;

        public TimeStamp(float Time, DataPoint Data)
        {
            this.Time = Time;
            this.Data = Data;
        }
    }

    public struct DataPoint
    {
        public int Cases;
        public int Deaths;
        public float Stringency;

        public DataPoint(int Cases = 0, float Stringency = 0f, int Deaths = 0)
        {
            this.Cases = Cases;
            this.Deaths = Deaths;
            this.Stringency = Stringency;
        }
    }
}
