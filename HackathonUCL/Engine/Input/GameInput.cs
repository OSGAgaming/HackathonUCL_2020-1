using HackathonUCL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HackathonUCL
{
    public class GameInput : IUpdate
    {
        public static GameInput Instance;

        static GameInput()
        {
            Instance = new GameInput();
        }

        private readonly Dictionary<string, InputBinding> _controls;

        public KeyboardState PreviousKeyState { get; private set; }
        public KeyboardState CurrentKeyState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public GamePadState PreviousControllerState { get; private set; }
        public GamePadState CurrentControllerState { get; private set; }

        public int DeltaScroll => CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
        public int DeltaScrollHorizontal => CurrentMouseState.HorizontalScrollWheelValue - PreviousMouseState.HorizontalScrollWheelValue;

        public Vector2 MousePosition => new Vector2(CurrentMouseState.X, CurrentMouseState.Y);

        public bool UsingController => CurrentControllerState.IsConnected;
        
        public GameInput()
        {
            Main.Updateables.Add(this);
            _controls = new Dictionary<string, InputBinding>();
        }

        /// <summary>
        /// Make sure this is called as early as possible in the Game's main Update method.
        /// </summary>
        public void Update()
        {
            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // Current controller implementation only checks for the first controller.
            // Local multiplayer implementation required? Assuming not for now.
            PreviousControllerState = CurrentControllerState;
            CurrentControllerState = GamePad.GetState(PlayerIndex.One);
        }

        public void RegisterControl(string name, Keys defaultKeyboard, Buttons defaultController)
        {
            _controls[name] = new InputBinding(defaultKeyboard, defaultController);
        }

        public void RegisterControl(string name, MouseInput defaultMouse, Buttons defaultController)
        {
            _controls[name] = new InputBinding(defaultMouse, defaultController);
        }

        public InputBinding this[string name] => _controls[name];
    }
}
