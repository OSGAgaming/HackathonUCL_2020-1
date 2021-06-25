namespace HackathonUCL
{
    public class UIScreenManager : Manager<UIScreen>
    {
        public static UIScreenManager? Instance;

        public UIScreenManager() : base(false)
        {
        }

        public void DrawOnScreen()
        {
            foreach (UIScreen UIS in Components)
            {
                UIS.DrawToScreen();
            }
        }

        static UIScreenManager()
        {
            Instance = new UIScreenManager();
        }
    }
}
