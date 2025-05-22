using System;
using UnityEngine;

namespace Core.UI
{
    public class DialogWindow : BaseWindow<DialogWindowView>
    {
        private bool _isInitialized;
        public override bool IsInitialized => _isInitialized;
        protected override void Initialize()
        {
            _isInitialized = true;
        }

        protected override void UpdateData()
        {
        }

        public void Show(string title, string content, string[] buttonTitles, Action[] buttonActions)
        {
            if (buttonActions == null || buttonActions.Length == 0)
                buttonActions = new Action[] {Hide };
            
            View.Initialize(title, content, buttonTitles, buttonActions, TryAddButton);
        }
        
        private bool TryAddButton(GameObject button, Transform parent)
        {
            if (!button || !parent) return false;
            
            Instantiate(button, parent);
            return true;
        }
    }
}