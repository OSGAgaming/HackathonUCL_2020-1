using Microsoft.Xna.Framework.Input;
using System;

namespace HackathonUCL
{
    public enum MouseInput
    {
        Left,
        Middle,
        Right,
        ScrollUp,
        ScrollDown,
        MouseButton1,
        MouseButton2
    }
    public class InputBinding
    {
        private readonly bool _isMouse;
        private readonly int _assignedValueKeyboard;
        private readonly int _assignedValueController;

        public InputBinding(Keys key, Buttons button)
        {
            _assignedValueKeyboard = (int)key;
            _assignedValueController = (int)button;
        }

        public InputBinding(MouseInput mouse, Buttons button)
        {
            _isMouse = true;
            _assignedValueKeyboard = (int)mouse;
            _assignedValueController = (int)button;
        }

        public bool IsDown()
        {
            return IsDown(GameInput.Instance.CurrentControllerState) || (_isMouse ? IsDown(GameInput.Instance.CurrentMouseState) : IsDown(GameInput.Instance.CurrentKeyState));
        }

        public bool IsDown(GamePadState gamepad)
        {
            if (!gamepad.IsConnected) return false;

            Buttons button = (Buttons)_assignedValueController;
            GamePadThumbSticks sticks = gamepad.ThumbSticks;
            GamePadTriggers triggers = gamepad.Triggers;
            switch (button)
            {
                case Buttons.LeftThumbstickLeft:
                case Buttons.LeftThumbstickRight:
                case Buttons.LeftThumbstickDown:
                case Buttons.LeftThumbstickUp:
                case Buttons.RightThumbstickLeft:
                case Buttons.RightThumbstickRight:
                case Buttons.RightThumbstickDown:
                case Buttons.RightThumbstickUp:
                case Buttons.LeftTrigger:
                case Buttons.RightTrigger:
                    return GetPressValue(gamepad) > 0f;
                default:
                    return gamepad.IsButtonDown(button);
            }
        }

        public bool IsDown(KeyboardState keyboard)
        {
            return keyboard.IsKeyDown((Keys)_assignedValueKeyboard);
        }

        public bool IsDown(MouseState mouse)
        {
            MouseInput button = (MouseInput)_assignedValueKeyboard;
            switch (button)
            {
                default:
                case MouseInput.Left:
                    return mouse.LeftButton == ButtonState.Pressed;
                case MouseInput.Middle:
                    return mouse.MiddleButton == ButtonState.Pressed;
                case MouseInput.Right:
                    return mouse.RightButton == ButtonState.Pressed;
                case MouseInput.MouseButton1:
                    return mouse.XButton1 == ButtonState.Pressed;
                case MouseInput.MouseButton2:
                    return mouse.XButton2 == ButtonState.Pressed;
                case MouseInput.ScrollDown:
                    return GameInput.Instance.DeltaScroll < 0;
                case MouseInput.ScrollUp:
                    return GameInput.Instance.DeltaScroll > 0; // TODO: Check these comparisons are actually correct, was a total guess.
            }
        }

        public bool IsUp()
        {
            return GameInput.Instance.UsingController ? IsUp(GameInput.Instance.CurrentControllerState) : (_isMouse ? IsUp(GameInput.Instance.CurrentMouseState) : IsUp(GameInput.Instance.CurrentKeyState));
        }

        public bool IsUp(GamePadState gamepad)
        {
            if (!gamepad.IsConnected) return true;

            return gamepad.IsButtonUp((Buttons)_assignedValueController);
        }

        public bool IsUp(KeyboardState keyboard)
        {
            return keyboard.IsKeyUp((Keys)_assignedValueKeyboard);
        }

        public bool IsUp(MouseState mouse)
        {
            MouseInput button = (MouseInput)_assignedValueKeyboard;
            switch (button)
            {
                default:
                case MouseInput.Left:
                    return mouse.LeftButton == ButtonState.Released;
                case MouseInput.Middle:
                    return mouse.MiddleButton == ButtonState.Released;
                case MouseInput.Right:
                    return mouse.RightButton == ButtonState.Released;
                case MouseInput.MouseButton1:
                    return mouse.XButton1 == ButtonState.Released;
                case MouseInput.MouseButton2:
                    return mouse.XButton2 == ButtonState.Released;
                case MouseInput.ScrollDown:
                    return GameInput.Instance.DeltaScroll > 0;
                case MouseInput.ScrollUp:
                    return GameInput.Instance.DeltaScroll < 0;
            }
        }

        public bool IsJustPressed()
        {
            return IsJustPressed(GameInput.Instance.PreviousControllerState, GameInput.Instance.CurrentControllerState) || (_isMouse ? IsJustPressed(GameInput.Instance.PreviousMouseState, GameInput.Instance.CurrentMouseState) : IsJustPressed(GameInput.Instance.PreviousKeyState, GameInput.Instance.CurrentKeyState));
        }

        public bool IsJustPressed(GamePadState old, GamePadState current)
        {
            if (!current.IsConnected) return false;

            return IsUp(old) && IsDown(current);
        }

        public bool IsJustPressed(KeyboardState old, KeyboardState current)
        {
            return IsUp(old) && IsDown(current);
        }

        public bool IsJustPressed(MouseState old, MouseState current)
        {
            return IsUp(old) && IsDown(current);
        }

        public float GetPressValue()
        {
            return Math.Max(GetPressValue(GameInput.Instance.CurrentControllerState), GetPressValue(GameInput.Instance.CurrentKeyState));
        }

        public float GetPressValue(KeyboardState keyboard)
        {
            return IsDown(keyboard) ? 1f : 0f;
        }

        public float GetPressValue(MouseState mouse)
        {
            MouseInput myValue = (MouseInput)_assignedValueKeyboard;
            switch (myValue)
            {
                case MouseInput.ScrollDown:
                case MouseInput.ScrollUp:
                    return GameInput.Instance.DeltaScroll;
                default:
                    return IsDown(mouse) ? 1f : 0f;
            }
        }

        public float GetPressValue(GamePadState gamepad)
        {
            if (!gamepad.IsConnected) return 0f;

            Buttons myValue = (Buttons)_assignedValueController;
            GamePadThumbSticks sticks = gamepad.ThumbSticks;
            GamePadTriggers triggers = gamepad.Triggers;
            switch (myValue)
            {
                case Buttons.LeftThumbstickLeft:
                    return Math.Min(0f, sticks.Left.X) * -1f;
                case Buttons.LeftThumbstickRight:
                    return Math.Max(0f, sticks.Left.X);
                case Buttons.LeftThumbstickDown:
                    return Math.Min(0f, sticks.Left.Y) * -1f;
                case Buttons.LeftThumbstickUp:
                    return Math.Max(0f, sticks.Left.Y);

                case Buttons.RightThumbstickLeft:
                    return Math.Min(0f, sticks.Right.X) * -1f;
                case Buttons.RightThumbstickRight:
                    return Math.Max(0f, sticks.Right.X);
                case Buttons.RightThumbstickDown:
                    return Math.Min(0f, sticks.Right.Y) * -1f;
                case Buttons.RightThumbstickUp:
                    return Math.Max(0f, sticks.Right.Y);

                case Buttons.LeftTrigger:
                    return triggers.Left;
                case Buttons.RightTrigger:
                    return triggers.Right;

                default:
                    return IsDown(gamepad) ? 1f : 0f;
            }
        }
    }
}
