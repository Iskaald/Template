#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Action = System.Action;

namespace Core.UI
{
    public abstract class BaseWindow<TView> : MonoBehaviour, IWindow<TView>, IInternalWindow where TView : class
    {
        public event Action WillShow;
        public event Action Shown;
        public event Action WillHide;
        public event Action Hidden;

        [SerializeField] private bool startHidden;

        [Header("Window Identifier")]
        [SerializeField] private string id;

        [SerializeField] private WindowType windowType;

        [Header("Window Animation")]
        [SerializeField] private bool animated;
        [SerializeField] private bool useDoTween = true;
#if DOTWEEN
        [SerializeField] private Ease animationEase = Ease.InQuad;
#endif
        [SerializeField] private float animationDuration = 0.25f;
        [SerializeField] private Image fade;
        [SerializeField] private Transform scalableContent;

        [SerializeField] private TView view;

        private Color? defaultFadeColor;
        private WindowAnimator animator;
        private bool isInitializing;

        public TView View => view;
        public abstract bool IsInitialized { get; }
        public string Id => id;

        #region Abstract Methods
        protected abstract void Initialize();
        protected abstract void UpdateData();
        #endregion

        void IInternalWindow.ReadyWindow()
        {
            if (isInitializing) return;
            isInitializing = true;
            try
            {
                animator = new WindowAnimator();

                if (startHidden) HideInstant();
                else ShowInstant();

                Initialize();
            }
            finally
            {
                isInitializing = false;
            }
        }

        void IInternalWindow.ShowInternal()
        {
            WillShow?.Invoke();

            if (!animated)
            {
                ShowInstant();
                return;
            }

            ProcessShow();
            AnimateShow();
        }

        void IInternalWindow.ShowInstantInternal()
        {
            ProcessShow();

            if (scalableContent)
                scalableContent.localScale = Vector3.one;
            transform.localScale = Vector3.one;

            gameObject.SetActive(true);
            OnShown();
        }

        void IInternalWindow.HideInternal()
        {
            WillHide?.Invoke();

            if (!animated)
            {
                HideInstant();
                return;
            }

            AnimateHide();
        }

        void IInternalWindow.HideInstantInternal()
        {
            OnHidden();
        }

        public void Show()
        {
            if (windowType != WindowType.Standard) ((IInternalWindow)this).ShowInternal();
            else WindowManager.Instance.ShowWindow(this);
        }

        public void ShowInstant()
        {
            if (windowType != WindowType.Standard) ((IInternalWindow)this).ShowInstantInternal();
            else WindowManager.Instance.ShowWindow(this, true);
        }

        public void Hide()
        {
            if (windowType != WindowType.Standard) ((IInternalWindow)this).HideInternal();
            else WindowManager.Instance.HideWindow(this);
        }

        public void HideInstant()
        {
            if (windowType != WindowType.Standard) ((IInternalWindow)this).HideInstantInternal();
            else WindowManager.Instance.HideWindow(this, true);
        }

        private void OnWillShow()
        {
            WillShow?.Invoke();
        }

        private void OnWillHide()
        {
            WillHide?.Invoke();
        }

        protected virtual void OnShown()
        {
            Shown?.Invoke();
        }

        protected virtual void OnHidden()
        {
            gameObject.SetActive(false);
            Hidden?.Invoke();
        }

        private void ProcessShow()
        {
            if (!IsInitialized && !isInitializing)
            {
                Initialize();
                return;
            }

            UpdateData();
        }

        private void AnimateShow()
        {
            StoreSettings();

            if (useDoTween)
            {
#if !DOTWEEN
                Debug.Log($"[UI.BaseWindow] DOTween (HOTWeen) not installed. Window will not animate. Install DOTween from Unity's Package Manager or use Animation");
                ShowInstant();
#else
                var doTweenAnimationData = new DOTweenAnimationData(fade, gameObject, animationDuration, animationEase, defaultFadeColor, scalableContent);
                animator.AnimateShow(doTweenAnimationData, OnShown);
#endif
            }
            else
            {
                var animationData = new AnimatorAnimationData();
                animator.AnimateShow(animationData, OnShown);
            }
        }

        private void AnimateHide()
        {
            StoreSettings();
            if (useDoTween)
            {
#if !DOTWEEN
                Debug.Log($"[UI.BaseWindow] DOTween (HOTWeen) not installed. Window will not animate. Install DOTween from Unity's Package Manager or use Animation");
                HideInstant();
#else
                var doTweenAnimationData = new DOTweenAnimationData(fade, gameObject, animationDuration, animationEase, defaultFadeColor,
                    scalableContent);
                animator.AnimateHide(doTweenAnimationData, OnHidden);
#endif
            }
            else
            {
                var animationData = new AnimatorAnimationData();
                animator.AnimateHide(animationData, OnHidden);
            }
        }

        private void StoreSettings()
        {
            if (!fade || defaultFadeColor != null && defaultFadeColor.HasValue) return;

            defaultFadeColor = fade.color;
        }
    }
}