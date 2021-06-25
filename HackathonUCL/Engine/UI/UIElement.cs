using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackathonUCL
{
    public class UIElement
    {
        public Rectangle dimensions;

        public virtual void Draw(SpriteBatch spriteBatch) { }

        protected virtual void OnUpdate() { }

        protected virtual void OnHover() { }

        protected virtual void NotOnHover() { }

        protected virtual void OnLeftClick() { }

        protected virtual void OnLeftClickAway() { }

        protected virtual void OnRightClick() { }

        public void Update()
        {
            OnUpdate();
            MouseState state = Mouse.GetState();
            if (dimensions.Contains(state.Position)) OnHover();
            if (!dimensions.Contains(state.Position)) NotOnHover();
            if (state.LeftButton == ButtonState.Pressed && dimensions.Contains(state.Position)) OnLeftClick();
            if (state.LeftButton == ButtonState.Pressed && !dimensions.Contains(state.Position)) OnLeftClickAway();
            if (state.RightButton == ButtonState.Pressed && dimensions.Contains(state.Position)) OnRightClick();
        }

        public void SetDimensions(int x, int y, int width, int height)
        {
            dimensions = new Rectangle(x, y, width, height);
        }

        public void SetDimensionsPercentage(float x, float y, int width, int height)
        {
            dimensions = new Rectangle((int)(x * Utils.ScreenSize.X), (int)(y * Utils.ScreenSize.Y), width, height);
        }
    }
}
