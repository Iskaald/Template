using System;
using UnityEngine;

namespace Core.UI
{
#if DOTWEEN
    using DG.Tweening;
#endif
    public class WindowAnimator
    {
#if DOTWEEN
        private Sequence animationSequence;
#endif
        public void AnimateShow(DOTweenAnimationData animationData, Action callback = null)
        {
            AnimateShowUsingDoTween(animationData, callback);
        }
        
        public void AnimateShow(AnimatorAnimationData animationData, Action callback = null)
        {
            AnimateShowUsingAnimation(animationData, callback);
        }

        public void AnimateHide(DOTweenAnimationData animationData, Action callback = null)
        {
            AnimateHideUsingDoTween(animationData, callback);
        }
        
        public void AnimateHide(AnimatorAnimationData animationData, Action callback = null)
        {
            AnimateHideUsingAnimator(animationData, callback);
        }
        
        private void AnimateShowUsingDoTween(DOTweenAnimationData data, Action callback)
        {
#if !DOTWEEN
            return;
#else
            animationSequence?.Complete(false);
            
            if (data.fade != null)
            {
                var transparent = data.fade.color;
                transparent.a = 0;
                data.fade.color = transparent;
            }

            if (data.scalableContent == null) data.scalableContent = data.window.transform;
            data.scalableContent.localScale = Vector3.zero;
            data.window.SetActive(true);
            
            animationSequence = DOTween.Sequence();
            animationSequence.OnComplete(() => callback?.Invoke());
            
            animationSequence.Append(data.scalableContent.DOScale(Vector3.one, data.animationDuration).SetEase(data.animationEase));
            if (data.fade != null)
                animationSequence.Join(data.fade.DOColor(data.defaultFadeColor!.Value, data.animationDuration).SetEase(data.animationEase));

            animationSequence.Play();
#endif
        }

        private void AnimateShowUsingAnimation(AnimatorAnimationData data, Action callback)
        {
            throw new NotImplementedException();
        }
        
        private void AnimateHideUsingDoTween(DOTweenAnimationData data, Action callback)
        {
#if !DOTWEEN
            return;
#else
            animationSequence?.Complete(false);
            
            animationSequence = DOTween.Sequence();
            animationSequence.OnComplete(() => callback?.Invoke());

            var transparent = Color.white;
            if (data.defaultFadeColor != null)
            {
                transparent = data.defaultFadeColor!.Value;
                transparent.a = 0;
            }
            
            if (data.scalableContent == null) data.scalableContent = data.window.transform;
            
            animationSequence.Append(data.scalableContent.DOScale(Vector3.zero, data.animationDuration).SetEase(data.animationEase));
            if (data.fade != null)
                animationSequence.Append(data.fade.DOColor(transparent, data.animationDuration).SetEase(data.animationEase));
            
            animationSequence.Play();
#endif
        }

        private void AnimateHideUsingAnimator(AnimatorAnimationData data, Action callback = null)
        {
            throw new NotImplementedException();
        }
    }
}