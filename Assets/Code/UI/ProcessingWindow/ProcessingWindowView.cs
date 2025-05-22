using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [Serializable]
    public class ProcessingWindowView : IWindowView
    {
        [SerializeField] private Image workingIcon;
        [SerializeField] private Vector3 rotationAxis = Vector3.back;
        [SerializeField] private float rotationSpeed = 90f;

        private Sequence sequence;
        
        public void Initialize()
        {
        }

        public void StartAnimation()
        {
            if (!workingIcon) return;
            
            sequence?.Kill(true);
            
            sequence = DOTween.Sequence();
            sequence.SetLoops(-1, LoopType.Restart);
            
            var duration = 360f / rotationSpeed;
            
            sequence.Append(workingIcon.transform.DORotate(rotationAxis * 360, duration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear));

            sequence.Play();
        }

        public void StopAnimation()
        {
            sequence?.Kill(true);
            workingIcon.transform.rotation = Quaternion.identity;
        }
    }
}