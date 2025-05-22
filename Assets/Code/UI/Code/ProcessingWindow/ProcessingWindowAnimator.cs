#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using UnityEngine;

namespace Core.UI
{
    public class ProcessingWindowAnimator : MonoBehaviour
    {
#if DOTWEEN
        private Sequence sequence;
#endif
        private Vector3 rotationAxis = Vector3.back;
        private float rotationSpeed = 90f;
        
        private Coroutine rotationCoroutine;
        
        public void StartAnimation(Transform gear)
        {
            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);

            rotationCoroutine = StartCoroutine(RotateGear(gear));
        }
        
        public void StartDOTweenAnimation(Transform gear)
        {
#if !DOTWEEN
            StartAnimation(gear);
            return;
#else
            sequence?.Complete(false);
            
            sequence = DOTween.Sequence();
            sequence.SetLoops(-1, LoopType.Restart);
            
            var duration = 360f / rotationSpeed;
            
            sequence.Append(gear.DORotate(rotationAxis * 360, duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear));

            sequence.Play();
#endif
        }
        
        public void StopAnimation()
        {
#if DOTWEEN
            sequence?.Kill();
#endif
            StopRotation();
        }
        
        private IEnumerator RotateGear(Transform gear)
        {
            while (true)
            {
                var rotationAmount = rotationSpeed * Time.deltaTime;
                gear.Rotate(rotationAxis * rotationAmount, Space.Self);
                yield return null;
            }
        }
        
        private void StopRotation()
        {
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
                rotationCoroutine = null;
            }
        }
    }
}