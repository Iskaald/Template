using System;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    [Serializable]
    public class DialogWindowView : IWindowView
    {
        [SerializeField] private TextMeshProUGUI windowTitle;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Transform buttonsParent;

        public void Initialize()
        {
            Initialize("", "", null, null, null);
        }
        
        public void Initialize(string title, string content, string[] buttonTitles, Action[] buttonActions, Func<GameObject, Transform, bool> addButtonCallback)
        {
            windowTitle.SetText(title);
            description.SetText(content);
            
            SetUpButtons(buttonTitles, buttonActions, addButtonCallback);
        }

        private void SetUpButtons(string[] buttonTitles, Action[] buttonActions, Func<GameObject, Transform, bool> addButtonCallback)
        {
            if (buttonsParent.childCount < 1) return;
            
            for (var i = 0 ; i < buttonsParent.childCount ; i++)
            {
                buttonsParent.GetChild(i).gameObject.SetActive(i < 1);
            }

            if ((buttonTitles == null || buttonTitles.Length == 0) ||
                (buttonActions == null || buttonActions.Length == 0))
            {
                var child = buttonsParent.GetChild(0);
                var button = child.GetComponent<CustomButton>();
                button.SetLabel("OK");
                return;
            }
            
            for (var i = 0; i < buttonTitles.Length; i++)
            {
                var idx = i;
                if (buttonsParent.childCount <= idx)
                {
                    if (!TryAddButton(addButtonCallback)) break;
                }
                var buttonTransform = buttonsParent.GetChild(idx);
                if (!buttonTransform) continue;
                var button = buttonTransform.GetComponent<CustomButton>();
                if (!button) continue;
                
                button.SetLabel(buttonTitles[idx]);
                if (buttonActions.Length <= idx)
                {
                    button.gameObject.SetActive(false);
                    continue;
                }
                button.onClick.AddListener(() => buttonActions[idx]?.Invoke());
                button.gameObject.SetActive(true);
            }
        }
        
        private bool TryAddButton(Func<GameObject, Transform, bool> addButtonCallback)
        {
            if (buttonsParent.childCount == 0) return false;
            var buttonTransform = buttonsParent.GetChild(0);
            if (!buttonTransform) return false;
            
            var button = buttonTransform.GetComponent<CustomButton>();
            return button && addButtonCallback(button.gameObject, buttonsParent);
        }
    }
}