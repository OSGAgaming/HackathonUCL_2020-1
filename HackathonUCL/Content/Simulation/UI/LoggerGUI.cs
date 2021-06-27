﻿using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HackathonUCL
{
    public static class Logger
    {
        private static readonly int LogCount = 100;
        internal static List<string> Logs = new List<string>();

        public static int TimeWithoutLog = 0;
        public static void NewText(string LogMessage)
        {
            TimeWithoutLog = 0;
            Logs.Insert(0, LogMessage);
            if(Logs.Count > LogCount)
            {
                Logs.RemoveAt(LogCount);
            }
        }

        public static void NewText(object LogMessage)
        {
            string? LM = LogMessage.ToString();
            if (LM != null)
            {
                TimeWithoutLog = 0;
                Logs.Insert(0, LM);
                if (Logs.Count > LogCount)
                {
                    Logs.RemoveAt(LogCount);
                }
            }
        }
    }
    internal class LoggerUI : UIScreen
    {
        public float LogAlpha;
        PlayButton? Play;
        PauseButton? Pause;
        Slider TimeSlider;
        StringencySlider StringencySlider;
        protected override void OnLoad()
        {
            Play = new PlayButton(TextureCache.Forward);
            Pause = new PauseButton(TextureCache.Pause);

            TimeSlider = new Slider();
            TimeSlider.EndLerp = Utils.ScreenSize.X - 20;
            TimeSlider.StartLerp = Utils.ScreenSize.X - 150;
            TimeSlider.Y = 50;

            StringencySlider = new StringencySlider();
            StringencySlider.EndLerp = Utils.ScreenSize.X - 20;
            StringencySlider.StartLerp = Utils.ScreenSize.X - 150;
            StringencySlider.Y = Utils.ScreenSize.Y - 50;

            Play.dimensions = new Rectangle(30,10, 50, 50);
            Pause.dimensions = new Rectangle(80, 10, 50, 50);

            elements.Add(Play);
            elements.Add(Pause);
            elements.Add(TimeSlider);
            elements.Add(StringencySlider);
        }
        protected override void OnUpdate()
        {
            Logger.TimeWithoutLog++;

            if (Logger.TimeWithoutLog > 180) LogAlpha = LogAlpha.ReciprocateTo(0);
            else LogAlpha = LogAlpha.ReciprocateTo(1,3f);
        }
        protected override void OnDraw()
        {
            int MaxOnscreenLogs = 10;
            var logger = Logger.Logs;

            Vector2 ASS = Utils.ScreenSize.ToVector2();
            int Count = MathHelper.Min(logger.Count , MaxOnscreenLogs);

            for (int i = 0; i < Count; i++)
            {
                float alpha = 1 - i / (float)MaxOnscreenLogs;
                Utils.DrawTextToLeft(logger[i], Color.Yellow * alpha * LogAlpha, new Vector2(30, ASS.Y - 30 - 20*i));
            }
        }
    }

    internal class PlayButton : UIElement
    {
        public string Text = "";
        protected string ExtraText = "";
        public float alpha = 1;

        public Texture2D tex;
        bool Hovering;
        bool Clicking;

        public PlayButton(Texture2D tex)
        {
            this.tex = tex;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawTextToLeft(Text + ExtraText, Color.White * alpha, dimensions.Location.ToVector2());

            if (Hovering) Utils.DrawRectangle(dimensions, Color.Gray);
            if (Clicking) spriteBatch.Draw(tex, dimensions, Color.Gray * alpha * 0.5f);
            else spriteBatch.Draw(tex, dimensions, Color.White * alpha);

            Clicking = false;
            Hovering = false;
        }
        protected override void OnLeftClick()
        {
            LocationHost.IsPlaying = true;

            Clicking = true;
        }

        protected override void OnHover()
        {
            Hovering = true;
        }
    }

    internal class PauseButton : UIElement
    {
        public string Text = "";
        protected string ExtraText = "";
        public float alpha = 1;

        public Texture2D tex;
        bool Hovering;
        bool Clicking;

        public PauseButton(Texture2D tex)
        {
            this.tex = tex;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawTextToLeft(Text + ExtraText, Color.White * alpha, dimensions.Location.ToVector2());

            if (Hovering) Utils.DrawRectangle(dimensions, Color.Gray);
            if (Clicking) spriteBatch.Draw(tex, dimensions, Color.Gray * alpha * 0.5f);
            else spriteBatch.Draw(tex, dimensions, Color.White * alpha);

            Clicking = false;
            Hovering = false;
        }
        protected override void OnLeftClick()
        {
            LocationHost.IsPlaying = false;

            Clicking = true;
        }

        protected override void OnHover()
        {
            Hovering = true;
        }

    }

    internal class Slider : UIElement
    {
        public float StartLerp;
        public float EndLerp;

        public float Current;
        public float Y;
        public float Alpha = 1;

        private int width = 20;
        private int height = 30;

        private bool isClicking;

        public float Lerp => (Current - StartLerp) / (EndLerp - StartLerp);
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawText("Time", Color.Black, new Vector2(StartLerp + (EndLerp - StartLerp) / 2, Y - 40));

            if (!isClicking)
                Current = (Main.Locations.Time / LocationHost.TotalTime) * (EndLerp - StartLerp) + StartLerp;

            Current = MathHelper.Clamp(Current, StartLerp, EndLerp);
            dimensions = new Rectangle((int)Current, (int)Y - height / 2, width, height);

            Utils.DrawLine(new Vector2(StartLerp, Y), new Vector2(EndLerp, Y), Color.Black, 1);

            Utils.DrawBoxFill(new Vector2(Current - 1, Y - height / 2 - 1), width + 2, height + 2, Color.Black);
            Utils.DrawBoxFill(new Vector2(Current, Y - height / 2), width, height, Color.White);

            Utils.DrawBoxFill(new Vector2(Current - 1, Y - height / 2 - 1), width + 2, height + 2, Color.Black);
            Utils.DrawBoxFill(new Vector2(Current, Y - height / 2), width, height, Color.White);

            isClicking = false;
        }
        protected override void OnLeftClick()
        {
            Current = Mouse.GetState().X - width /2;
            
            Main.Locations.Time = LocationHost.TotalTime * Lerp;

            isClicking = true;
        }

    }

    internal class StringencySlider : UIElement
    {
        public float StartLerp;
        public float EndLerp;

        public float Current;
        public float Y;
        public float Alpha = 1;

        private int width = 30;
        private int height = 30;

        private bool isClicking;

        public float Lerp => (Current - StartLerp) / (EndLerp - StartLerp);

        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawText("Stringency Coefficent", Color.Black, new Vector2(StartLerp + (EndLerp - StartLerp) / 2, Y - 40));

            if (!isClicking)
                Current = (LocationHost.StringencyModifier / 2) * (EndLerp - StartLerp) + StartLerp;

            Current = MathHelper.Clamp(Current, StartLerp, EndLerp);

            dimensions = new Rectangle((int)Current - width / 2, (int)Y - height / 2, width, height);

            Utils.DrawLine(new Vector2(StartLerp, Y), new Vector2(EndLerp, Y), Color.Black, 1);
            Utils.DrawClosedCircle(new Vector2(Current, Y), 12,1, Color.Black);
            Utils.DrawClosedCircle(new Vector2(Current, Y), 10, 1, Color.White);

            isClicking = false;
        }
        protected override void OnLeftClick()
        {
            Current = Mouse.GetState().X;
            LocationHost.StringencyModifier = Lerp * 2;

            isClicking = true;
        }
    }
}
