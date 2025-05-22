using System;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    [Serializable]
    public class HUDWindowView
    {
        [SerializeField] private RectTransform bottomBar;
        [SerializeField] private RectTransform navbar;
        [SerializeField] private CustomButton navbarVisibilityButton;

        private bool navbarShown;
        private Vector2 navbarShownPosition;
        private Vector2 navbarHiddenPosition;
        private const float NavbarAnimationDuration = 0.3f;

        private Sequence navbarSequence;

        public void Initialize()
        {
            InitializeNavbar();
        }
        
        private void InitializeNavbar()
        {
            navbarVisibilityButton?.onClick.RemoveAllListeners();
            navbarVisibilityButton?.onClick.AddListener(ToggleNavbar);

            navbarShownPosition = bottomBar.anchoredPosition;
            navbarHiddenPosition = navbarShownPosition - new Vector2(0.0f, navbar.rect.height);

            navbarShown = true;
            ToggleNavbar(false,3.0f);
        }

        private void ToggleNavbar()
        {
            ToggleNavbar(!navbarShown, 0);
        }

        private void ToggleNavbar(bool show, float delay)
        {
            if (navbarShown == show) return;
            
            delay = Mathf.Max(0, delay);
            
            navbarSequence?.Kill(true);
            
            var targetPos = show ? navbarShownPosition : navbarHiddenPosition;
            
            navbarSequence = DOTween.Sequence();
            navbarSequence.OnComplete(() => navbarShown = show);
            navbarSequence.AppendInterval(delay);
            navbarSequence.Append(bottomBar.DOAnchorPos(targetPos, NavbarAnimationDuration)
                .SetEase(Ease.InOutCubic));
            navbarVisibilityButton?.SetHighlight(show, ButtonSelectType.Flip, navbarSequence);
            
            navbarSequence.Play();
        }
    }
}