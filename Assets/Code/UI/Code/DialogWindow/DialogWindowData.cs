using System;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    [Serializable]
    public class DialogWindowData
    {
        public TextMeshProUGUI windowTitle;
        public TextMeshProUGUI description;
        public Transform buttonsParent;
    }
}