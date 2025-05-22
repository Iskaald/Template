using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DialogWindow : BaseWindow<DialogWindowData>
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
            View.windowTitle.SetText(title);
            View.description.SetText(content);

            if (View.buttonsParent.childCount < 1) return;
            
            for (var i = 0 ; i < View.buttonsParent.childCount ; i++)
            {
                View.buttonsParent.GetChild(i).gameObject.SetActive(i < 1);
            }

            if ((buttonTitles == null || buttonTitles.Length == 0) ||
                (buttonActions == null || buttonActions.Length == 0))
            {
                var child = View.buttonsParent.GetChild(0);
                var button = child.GetComponent<CustomButton>();
                button.SetLabel("OK");
                button.onClick.AddListener(Hide);
                Show();
                return;
            }
            
            for (var i = 0; i < buttonTitles.Length; i++)
            {
                var idx = i;
                if (View.buttonsParent.childCount <= idx)
                {
                    if (!TryAddButton()) break;
                }
                var buttonTransform = View.buttonsParent.GetChild(idx);
                if (buttonTransform == null) continue;
                var button = buttonTransform.GetComponent<CustomButton>();
                if (button == null) continue;
                
                button.SetLabel(buttonTitles[idx]);
                if (buttonActions.Length <= idx)
                {
                    button.gameObject.SetActive(false);
                    continue;
                }
                button.onClick.AddListener(() => buttonActions[idx]?.Invoke());
                button.gameObject.SetActive(true);
            }
            Show();
        }

        private bool TryAddButton()
        {
            if (View.buttonsParent.childCount == 0) return false;
            var buttonTransform = View.buttonsParent.GetChild(0);
            if (buttonTransform == null) return false;
            
            var button = buttonTransform.GetComponent<CustomButton>();
            if (button == null) return false;
            
            Instantiate(buttonTransform.gameObject, View.buttonsParent);
            return true;
        }
    }
}