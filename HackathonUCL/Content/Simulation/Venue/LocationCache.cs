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
        public static Dictionary<string, List<TimeStamp>> CountryVariables = new Dictionary<string, List<TimeStamp>>();

        public static void LoadLocations()
        {
            List<string> CountryNames = new List<string>();
            List<int> Population = new List<int>();

            //List<string> CountryNames = new List<string>();
            List<float> Stringency = new List<float>();

            int rowCounter = 0;

            using (var reader = new StreamReader(@"C:\Users\tafid\Documents\HackathonUCL_2020-1\HackathonUCL\Content\Simulation\Datasets\dummy_data.csv"))
            {
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    if (rowCounter > 0)
                    {
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

            int rowCounter2 = 0;
            using (var reader = new StreamReader(@"C:\Users\tafid\Documents\HackathonUCL_2020-1\HackathonUCL\Content\Simulation\Datasets\stringency.csv"))
            {
                while (!reader.EndOfStream)
                {
                    if (rowCounter2 != 0)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (!CountryVariables.ContainsKey(values[0]))
                        {
                            CountryVariables.Add(values[0], new List<TimeStamp>());
                            if (float.TryParse(values[3], out _))
                            {
                                CountryVariables[values[0]].Add(new TimeStamp(CountryVariables[values[0]].Count * 10, new DataPoint(0, float.Parse(values[3]), 0)));
                            }
                        }
                        else
                        {
                            if (float.TryParse(values[3], out _))
                            {
                                CountryVariables[values[0]].Add(new TimeStamp(CountryVariables[values[0]].Count * 10, new DataPoint(Main.rand.Next(100), float.Parse(values[3]), 0)));
                            }
                        }
                        //CountryNames.Add(values[0]);

                        if (float.TryParse(values[3], out _))
                        {
                            Stringency.Add(float.Parse(values[3]));
                            //Debug.WriteLine(values[3]);
                        }
                        else
                        {
                            Stringency.Add(-1);
                        }
                    }
                    rowCounter2++;
                }
            }

            for (int i = 0; i < rowCounter - 2; i++)
            {
                Vector2 Location = Vector2.Zero;

                switch (CountryNames[i])
                {
                    case "Brazil":
                        Location = new Vector2(319, 399);
                        break;
                    case "South Africa":
                        Location = new Vector2(614, 488);
                        break;
                    case "United Kingdom":
                        Location = new Vector2(516, 121);
                        break;
                    case "China":
                        Location = new Vector2(895, 203);
                        break;
                    case "Saudi Arabia":
                        Location = new Vector2(695, 252);
                        break;
                    case "United States":
                        Location = new Vector2(147, 193);
                        break;
                    case "Germany":
                        Location = new Vector2(560, 130);
                        break;
                    case "Democratic Republic of Congo":
                        Location = new Vector2(608, 369);
                        break;
                    case "New Zealand":
                        Location = new Vector2(1140, 540);
                        break;
                    default:
                        break;
                }

                Main.Locations.AppendLocation(CountryNames[i], Location, Population[i]);
            }
        }
    }
}
