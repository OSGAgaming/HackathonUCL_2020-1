using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

#nullable disable
// TODO fix this..
namespace HackathonUCL
{
    public static class LocationCache
    {
        public static void LoadLocations()
        {
            List<string> CountryNames = new List<string>();
            List<int> Population = new List<int>();

            int rowCounter = 0;

            using (var reader = new StreamReader(@"C:\Users\tafid\Documents\HackathonUCL_2020-1\HackathonUCL\Content\Simulation\Datasets\dummy_data.csv"))
            {
                while (!reader.EndOfStream)
                {
                    if (rowCounter != 0)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        CountryNames.Add(values[0]);

                        if (int.TryParse(values[1], out _))
                        {
                            Population.Add(int.Parse(values[1]));
                        }
                        else
                        {
                            Population.Add(-1);
                        }
                    }

                    rowCounter++;
                }
            }

            for (int i = 0; i < rowCounter - 1; i++)
            {
                Debug.WriteLine(Population[i]);
                Main.Locations.AppendLocation(CountryNames[i], new Vector2(Main.rand.Next(1000), Main.rand.Next(500)), Population[i]);
            }
        }
    }
}
