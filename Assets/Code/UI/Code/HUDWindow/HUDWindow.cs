using JetBrains.Annotations;

namespace Core.UI
{
    public class HUDWindow : BaseWindow<HUDWindowView>
    {
        private bool _isInitialized;

        public override bool IsInitialized => _isInitialized;

        protected override void Initialize()
        {
            View.Initialize();
            _isInitialized = true;
        }

        protected override void UpdateData()
        {
            
        }

        [UsedImplicitly]
        public void ShowSettings()
        {
            if (!WindowManager.Instance) return;

            var wnd = WindowManager.Instance.GetWindow<SettingsWindow>();
            wnd?.Show();
        }
    }
}