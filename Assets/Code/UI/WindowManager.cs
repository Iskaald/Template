using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.UI
{
    public class WindowManager : MonoBehaviour, IWindowManager
    {
        public static event Action OnInitialized;
        public static WindowManager Instance { get; private set; }

        private readonly Dictionary<Type, List<IWindow>> windows = new();
        private readonly List<IInternalWindow> windowsHistory = new();

        private Tooltip tooltip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"There is already another window manager of type {GetType().Name}. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            OnInitialized?.Invoke();
        }

        internal void RegisterWindow(IWindow window)
        {
            if (window == null)
            {
                Console.WriteLine("Attempted to register a null window");
                return;
            }
            
            try
            {
                var type = window.GetType();
                Console.WriteLine($"Registering window of type: {type.Name}");
                
                if (windows.ContainsKey(type) && windows[type].Contains(window))
                {
                    Console.WriteLine($"Window of type {type.Name} is already registered");
                    return;
                }
                
                if (!windows.ContainsKey(type))
                {
                    windows[type] = new List<IWindow>();
                }

                windows[type].Add(window);

                if (window is not IInternalWindow internalWindow || window.IsInitialized) return;
                
                Console.WriteLine($"Initializing window of type: {type.Name}");
                internalWindow.ReadyWindow();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error registering window of type {window?.GetType()?.Name ?? "unknown"}: {e.Message}\n{e.StackTrace}");
                if (windows.ContainsKey(window.GetType()))
                {
                    windows[window.GetType()].Remove(window);
                }
            }
        }

        internal void UnregisterWindow(IWindow window)
        {
            var type = window.GetType();
            if (!windows.TryGetValue(type, out var list)) return;

            list.Remove(window);
            if (list.Count == 0)
                windows.Remove(type);

            if (window is IInternalWindow internalWindow)
                windowsHistory.Remove(internalWindow);
        }

        internal void RegisterTooltip(Tooltip newTooltip)
        {
            if (!newTooltip) return;
            if (tooltip) Console.WriteLine("You're registering a duplicate for tooltip, only one will be preserved");
            tooltip = newTooltip;
            tooltip.HideInstant();
        }

        internal void UnRegisterTooltip(Tooltip newTooltip)
        {
            if (!newTooltip) return;
            if (tooltip != newTooltip) return;
            tooltip = null;
        }

        public T GetWindow<T>(string id = null) where T : class, IWindow
        {
            if (!windows.TryGetValue(typeof(T), out var windowList)) return null;

            if (string.IsNullOrEmpty(id)) return windowList.FirstOrDefault() as T;

            return windowList.OfType<T>().FirstOrDefault(w => w.Id == id);
        }

        public Tooltip GetTooltip()
        {
            return tooltip;
        }

        public IWindow GetWindow(string id)
        {
            IWindow window = null;
            
            foreach (var windowType in windows)
            {
                var wnd = windowType.Value.FirstOrDefault(w => w.Id == id);
                if (wnd == null) continue;

                window = wnd;
                break;
            }
            return window;
        }
        
        public void ShowWindow(string id, bool instant = false)
        {
            IWindow wnd = null;
            foreach (var windowType in windows)
            {
                wnd = windowType.Value.FirstOrDefault(w => w.Id == id);
                if (wnd == null) continue;
                
                break;
            }

            ShowWindow(wnd, instant);
        }
        
        public void ShowWindow(IWindow wnd, bool instant = false)
        {
            if (wnd is not IInternalWindow internalWindow) return;

            windowsHistory.Remove(internalWindow);

            if (windowsHistory.Count > 0)
                windowsHistory[^1].HideInstantInternal();

            windowsHistory.Add(internalWindow);

            if (instant) internalWindow.ShowInstantInternal();
            else internalWindow.ShowInternal();
        }
        
        public void HideWindow(string id, bool instant = false)
        {
            IWindow wnd = null;
            foreach (var windowType in windows)
            {
                wnd = windowType.Value.FirstOrDefault(w => w.Id == id);
                if (wnd == null) continue;
                
                break;
            }

            HideWindow(wnd, instant);
        }
        
        public void HideWindow(IWindow wnd, bool instant = false)
        {
            if (wnd is not IInternalWindow internalWindow) return;

            windowsHistory.Remove(internalWindow);

            if (instant) internalWindow.HideInstantInternal();
            else internalWindow.HideInternal();

            if (windowsHistory.Count > 0)
                windowsHistory[^1].ShowInstantInternal();
        }

        public void RequestBack(bool instant = false)
        {
            if (windowsHistory.Count == 0) return;

            var last = windowsHistory[^1];
            windowsHistory.RemoveAt(windowsHistory.Count - 1);

            if (instant)
                last.HideInstantInternal();
            else
                last.HideInternal();

            if (windowsHistory.Count > 0)
                windowsHistory[^1].ShowInstantInternal();
        }
    }
}
