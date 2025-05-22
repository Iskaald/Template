using System;
using UnityEngine;

namespace Core.UI
{
    public class NavbarButton : CustomButton
    {
        [SerializeField] private ButtonSelectType buttonHighlightBehaviour = ButtonSelectType.IconAndBackgroundColorSwap;
        
        [Tooltip("The window this button should open")]
        [SerializeField] private string windowName;
        
        private IWindow windowInstance;
        private bool showing;
        private bool hasCustomClickHandler;

        #region MONOBEHAVIOR
        protected override void Start()
        {
            // Only remove listeners if we don't have a custom click handler
            if (!hasCustomClickHandler)
            {
                onClick.RemoveAllListeners();
                
                if (!string.IsNullOrEmpty(windowName))
                {
                    windowInstance = WindowManager.Instance?.GetWindow(windowName);
                    if (windowInstance != null)
                    {
                        onClick.AddListener(ShowOrHideWindow);

                        windowInstance.Shown += OnWindowShown;
                        windowInstance.Hidden += OnWindowHidden;
                    }
                }
            }
            base.Start();
        }

        protected override void OnDestroy()
        {
            onClick.RemoveAllListeners();
            
            if (windowInstance != null)
            {
                windowInstance.Shown -= OnWindowShown;
                windowInstance.Hidden -= OnWindowHidden;
            }
            base.OnDestroy();
        }
        #endregion

        public void SetOnClick(Action onClickAction)
        {       
            hasCustomClickHandler = true;
            onClick.AddListener(onClickAction.Invoke);
        }

        private void OnWindowShown()
        {
            SetHighlight(true, buttonHighlightBehaviour);
            showing = true;
        }

        private void OnWindowHidden()
        {
            SetHighlight(false, buttonHighlightBehaviour);
            showing = false;
        }

        private void ShowOrHideWindow()
        {
            if (windowInstance == null) return;
            
            if (showing)
            {
                windowInstance.HideInstant();
            }
            else
            {
                windowInstance.Show();
            }
        }
    }
}