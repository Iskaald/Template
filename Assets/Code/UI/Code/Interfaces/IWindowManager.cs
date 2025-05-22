namespace Core.UI
{
    public interface IWindowManager
    {
        public T GetWindow<T>(string id = null) where T : class, IWindow;
    }
}