using System;
using _Main.Scripts.Global.DialogSystem.Services;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Global.UI.Loading
{
    public class LoadingScreen : SerializedMonoBehaviour
    {
        [SerializeField] private IDialogAnimator dialogAnimator;
        [SerializeField] private Image fillImage;
        [SerializeField] private float fillDuration;
        
        public event Action OnIdleAnimationFinished;
        
        private Tween _tween;
        private bool _idleIsActive;

        private void OnEnable()
        {
            fillImage.fillAmount = 0f;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public Tween PlayShowScreen()
        {
            _idleIsActive = true;
            return DOTween.Sequence()
                .Append(dialogAnimator.PlayShowDialog())
                .AppendCallback(() => AnimateIdle(1f));
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public Tween PlayHideScreen()
        {
            return DOTween.Sequence()
                .Append(dialogAnimator.PlayHideDialog())
                .AppendCallback(() => DOTween.Kill(fillImage));
        }

        public void StopIdleAnimationOnComplete()
        {
            _idleIsActive = false;
        }

        private void AnimateIdle(float endValue)
        {
            float newEndValue = Mathf.Approximately(endValue, 1f) ? 0f : 1f;
            fillImage.DOFillAmount(endValue, fillDuration).OnComplete(() =>
            {
                if (_idleIsActive)
                {
                    AnimateIdle(newEndValue);
                }
                else
                {
                    OnIdleAnimationFinished?.Invoke();
                }
            });
        }
        
    }
}