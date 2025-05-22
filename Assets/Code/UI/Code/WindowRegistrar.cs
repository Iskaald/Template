using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Core.UI
{
    public class WindowRegistrar : MonoBehaviour
    {
        private List<IWindow> localWindows = new();
        private Tooltip tooltip = new();

        private void OnEnable()
        {
            if (WindowManager.Instance == null)
            {
                WindowManager.OnInitialized += RegisterWindows;
                WindowManager.OnInitialized += RegisterTooltip;
                return;
            }
            RegisterWindows();
            RegisterTooltip();
        }

        private void OnDisable()
        {
            WindowManager.OnInitialized -= RegisterWindows;
            WindowManager.OnInitialized -= RegisterTooltip;
            
            foreach (var window in localWindows)
            {
                WindowManager.Instance?.UnregisterWindow(window);
            }
            WindowManager.Instance?.UnRegisterTooltip(tooltip);
        }

        private void RegisterWindows()
        {
            try
            {
                WindowManager.OnInitialized -= RegisterWindows;
                
            localWindows = GetComponentsInChildren<IWindow>(true).ToList();
            foreach (var window in localWindows)
            {
                    WindowManager.Instance?.RegisterWindow(window);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error registering windows: {e.Message}\n{e.StackTrace}");
            }
        }

        private void RegisterTooltip()
        {
            try
            {
                tooltip = GetComponentInChildren<Tooltip>(true);
                WindowManager.Instance?.RegisterTooltip(tooltip);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error registering tooltip: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}