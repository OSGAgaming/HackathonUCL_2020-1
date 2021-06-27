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
        public void OnHover()
        {
            Color c = Color.Black * TextAlpha;
            Vector2 mousep = Utils.MouseScreen.ToVector2();

            float ProcessedCases = GetProcessedCases(GetTotalCases() + (float)Math.E);
            float Perc = GetDeaths() / GetCases();

            if(GetCases() == 0) Perc = 0;

            Perc = Math.Clamp(Perc, 0, 2);

            Utils.DrawClosedCircle(DrawPosition, Perc * ProcessedCases * 10, TextAlpha * 0.3f, Color.Purple);

            Utils.DrawText(Name, c, mousep - new Vector2(-20, 100 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));

            if (Name == "South Africa") Utils.DrawText("Population: " + Population.ToString(), c, mousep - new Vector2(-20, 85 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            else Utils.DrawText("Population: " + Math.Round(GetPopulation()).ToString(), c, mousep - new Vector2(-20, 85 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));

            Utils.DrawText("Cases Today: " + Math.Round(GetCases()).ToString(), c, mousep - new Vector2(-20, 70 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            Utils.DrawText("Total Cases: " + Math.Round(GetTotalCases()).ToString(), c, mousep - new Vector2(-20, 55 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));

            if (GetDeaths() < 100000) Utils.DrawText("Deaths Today: " + Math.Round(GetDeaths()).ToString(), c, mousep - new Vector2(-20, 40 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
            else Utils.DrawText("Data Error in Deaths Per Day", Color.Red * TextAlpha, mousep - new Vector2(-20, 40 / Main.mainCamera.scale), 0f, new Vector2(1 / Main.mainCamera.scale));
        }
    }
}
