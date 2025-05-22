#if DOTWEEN
using DG.Tweening;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DOTweenAnimationData
    {
        public Image fade;
        public GameObject window;
        public float animationDuration;
#if DOTWEEN
        public Ease animationEase;
#endif
        public Color? defaultFadeColor;
        public Transform scalableContent;
        
        public DOTweenAnimationData(){}
#if DOTWEEN
        public DOTweenAnimationData(Image fadeImage, GameObject window, float animationDuration, Ease animationEase, Color? defaultFadeColor = null, Transform scalableContent = null)
        {
            fade = fadeImage;
            this.window = window;
            this.animationDuration = animationDuration;
            this.animationEase = animationEase;
            this.defaultFadeColor = defaultFadeColor;
            this.scalableContent = scalableContent;
        }
#endif
    }
}