
using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace HackathonUCL
{
    internal class Box : UIElement
    {
        protected virtual Color color => Color.Black;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawBoxFill(dimensions.Inf(2, 2), Color.CadetBlue);
            Utils.DrawBoxFill(dimensions,color);
            PostDraw(spriteBatch); 
        }
        protected virtual void PostDraw(SpriteBatch spriteBatch) { }
    }
    internal class Text : Box
    {
        protected override Color color => Color.White;
        public string inputText = "";
        public bool hasCursor;
        protected KeyboardState oldKeyboardState = Keyboard.GetState();
        protected KeyboardState currentKeyboardState = Keyboard.GetState();
        protected float alpha = 1;
        public void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (oldKeyboardState.IsKeyUp(key) && key != Keys.OemSemicolon)
                {
                    KeyboardInput.Instance?.InputKey(key, ref inputText);
                }
            }
        }
        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = Main.font.MeasureString(inputText);
            int disp = dimensions.Height / 2 - (int)FS.Y/2;
            Utils.DrawTextToLeft(inputText, Color.Black * alpha, dimensions.Location.ToVector2() + new Vector2(0, disp));
            if (hasCursor)
            {
                Point pos = new Point(dimensions.Location.X + (int)FS.X + 1, dimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                spriteBatch.Draw(TextureCache.pixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * alpha);
            }
            CustomDraw(spriteBatch);
        }
        protected override void OnUpdate()
        {
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
        protected virtual void CustomDraw(SpriteBatch spriteBatch) { }
        protected virtual void PostUpdate() { }
        protected override void OnLeftClick()
        =>
            hasCursor = true;
        protected override void OnLeftClickAway()
        =>
            hasCursor = false;
    }
    internal class NumberBox : Box
    {
        protected override Color color => Color.White;
        public string inputText = "";
        public float alpha = 1f;
        public bool hasCursor;
        public float lerp;
        public float Number => float.Parse(inputText);
        protected KeyboardState oldKeyboardState = Keyboard.GetState();
        protected KeyboardState currentKeyboardState = Keyboard.GetState();
        public void UpdateInput()
        {
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();
            if (hasCursor)
            {
                foreach (Keys key in pressedKeys)
                {
                    bool IsNumber = (key >= Keys.D0
                                             &&
                                            key <= Keys.D9 || key == Keys.OemPeriod || key == Keys.OemMinus);
                    if (oldKeyboardState.IsKeyUp(key) && (IsNumber && !(inputText.Contains(".") && key == Keys.OemPeriod) || key == Keys.Back))
                    {
                        KeyboardInput.Instance?.InputKey(key, ref inputText);
                    }
                }
            }
        }
        protected override void PostDraw(SpriteBatch spriteBatch)
        {
            Vector2 FS = Main.font.MeasureString(inputText);
            int disp = dimensions.Height / 2 - (int)FS.Y / 2;
            Utils.DrawTextToLeft(inputText, Color.Black * alpha, dimensions.Location.ToVector2() + new Vector2(0, disp));
            if (hasCursor)
            {
                Point pos = new Point(dimensions.Location.X + (int)FS.X + 1, dimensions.Location.Y + disp);
                Point size = new Point(2, (int)FS.Y);
                spriteBatch.Draw(TextureCache.pixel, new Rectangle(pos, size), Color.Black * Time.SineTime(10f) * alpha);
            }
            CustomDraw(spriteBatch);
        }
        protected virtual void CustomDraw(SpriteBatch spriteBatch) { }
        protected override void OnUpdate()
        {
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
        protected virtual void PostUpdate() { }
        protected override void OnLeftClick()
        =>
            hasCursor = true;
        protected override void OnLeftClickAway()
        =>
            hasCursor = false;
    }
    internal class NumberBoxScalable : NumberBox
    {
        protected override void OnUpdate()
        {
            dimensions.Width = (int)Main.font.MeasureString(inputText).X + 10;
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
    }
    internal class TextBoxScalable : Text
    {
        protected override void OnUpdate()
        {
            dimensions.Width = (int)Main.font.MeasureString(inputText).X + 10;
            if (hasCursor)
            {
                UpdateInput();
            }
            PostUpdate();
        }
    }
}
