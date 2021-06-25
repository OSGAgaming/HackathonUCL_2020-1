using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HackathonUCL
{
    public class Scene
    {
        protected int TransitionTimer;

        public int[] InteralTimerCache;
        protected virtual int TransitionLength { get; } = 100;

        public int TransitionFrom => TransitionLength - TransitionTimer;

        internal int Step;

        protected List<SubRoutine> Actions = new List<SubRoutine>();

        public SubRoutine CurrentRoutine => 
            Actions[Step];
                             
        public int InternalTimer => CurrentRoutine._progress;

        public int GlobalTimer;

        internal List<Element> Elements = new List<Element>();

        public Scene()
        {
            LoadElements(this);
            LoadActions();
            InteralTimerCache = new int[Actions.Count];
        }
        public float AlphaFrom(int Point, float factor) => (InternalTimer - Point) / factor;
        public float AlphaFrom(int Point, float factor, int Step) => (InteralTimerCache[Step] - Point) / factor;
        public virtual void LoadElements(Scene scene) { }
        public virtual void LoadActions() { }

        internal void Next() { Step++; if (Step >= Actions.Count) TransitionTimer = TransitionLength; }

        internal void Previous() => Step--;

        public bool Finished => Step >= Actions.Count && TransitionTimer == -1;

        public virtual void Update()
        {
            foreach (Element e in Elements)
                e.Update();

            GlobalTimer++;
            for(int i = 0; i<Actions.Count; i++)
            {
                if(i <= Step)
                {
                    InteralTimerCache[i]++;
                }
            }
            if (Step < Actions.Count)
                CurrentRoutine.Update();
            else
            {
                OnDeath();
                if (TransitionTimer > -1) TransitionTimer--;
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Step < Actions.Count)
                CurrentRoutine.Draw(spriteBatch);
            else
            {
                OnDeathDraw(spriteBatch);
            }
            foreach (Element e in Elements)
                e.Draw(spriteBatch);
        }

        public virtual void OnDeath() { }

        public virtual void OnDeathDraw(SpriteBatch sb) { }
        public void AddStepToRoutine(Step step, int SubroutineIndex)
        {
            //Actions[Step].AddStep(step.LowerTimeFrame,step.UpperTimeFrame,ste);
        }

        public void AppendAction(Action UpdateDelegate, Action<SpriteBatch> DrawDelegate) 
        { 
            Actions.Add(new SubRoutine(UpdateDelegate, DrawDelegate)); 
            Actions[Actions.Count - 1].parent = this; 
        }
    }
    public struct Step
    {
        internal int LowerTimeFrame { get; set; }

        internal int UpperTimeFrame { get; set; }

        public Action UpdateDelegate;

        public Action<SpriteBatch> DrawDelegate;

        public Step(int L, int U, Action UD, Action<SpriteBatch> DD)
        {
            LowerTimeFrame = L;
            UpperTimeFrame = U;
            UpdateDelegate = UD;
            DrawDelegate = DD;
        }
    }
    public class SubRoutine
    {
        internal int _progress;

        public Action UpdateDelegate;

        public Action<SpriteBatch> DrawDelegate;

        List<Step> Steps = new List<Step>();

        public Scene parent;
        public void AddStep(int L, int U, Action UD, Action<SpriteBatch> DD) { Steps.Add(new Step(L, U, UD, DD)); }
        public virtual void Update() 
        { 
            foreach(Step step in Steps)
            {
                if(_progress > step.LowerTimeFrame && _progress < step.UpperTimeFrame)
                {
                    step.UpdateDelegate.Invoke();
                }
            }
            UpdateDelegate.Invoke(); 
            _progress++; 
        }

        public virtual void Draw(SpriteBatch spriteBatch) => DrawDelegate.Invoke(spriteBatch);

        public SubRoutine(Action UpdateDelegate, Action<SpriteBatch> DrawDelegate)
        {
            this.UpdateDelegate = UpdateDelegate;
            this.DrawDelegate = DrawDelegate;
        }
    }

}
