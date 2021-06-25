
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HackathonUCL
{
    public class UIScreen : IComponent
    {
        public List<UIElement> elements = new List<UIElement>();

        public bool active;

        public UIScreen()
        {
            UIScreenManager.Instance?.AddComponent(this);
            OnLoad();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            OnDraw();
            if (active)
            {
                foreach (UIElement element in elements)
                {
                    element.Draw(spriteBatch);
                }
            }
        }

        protected virtual void OnUpdate() { }

        protected virtual void OnDraw() { }

        internal virtual void DrawToScreen() { }

        protected virtual void OnLoad() { }

        public void Update()
        {
            if (active)
            {
                OnUpdate();
                foreach (UIElement element in elements.ToArray())
                {
                    element.Update();
                }
            }
        }
    }
}
