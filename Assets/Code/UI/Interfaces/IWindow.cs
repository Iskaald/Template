using System;

namespace Core.UI
{
    internal interface IInternalWindow
    {
        internal void ReadyWindow();
        internal void ShowInternal();
        internal void ShowInstantInternal();
        internal void HideInternal();
        internal void HideInstantInternal();
    }
    
    public interface IWindow<out TView> : IWindow where TView : IWindowView
    {
        TView View { get; }
    }
    
    public interface IWindow
    {
        public event Action WillShow;
        public event Action Shown;
        public event Action WillHide;
        public event Action Hidden;
        
        public bool IsInitialized { get; }
        public string Id { get; }
        public void Show();
        public void ShowInstant();
        public void Hide();
        public void HideInstant();
    }
}
