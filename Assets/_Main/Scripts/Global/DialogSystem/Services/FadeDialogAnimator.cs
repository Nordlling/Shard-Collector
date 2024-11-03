using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.Global.DialogSystem.Services
{
    public class FadeDialogAnimator : MonoBehaviour, IDialogAnimator
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float targetFade = 1f;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private float pause;

        public Tween PlayShowDialog()
        {
            canvasGroup.alpha = 0f;
            return DOTween.Sequence()
                .AppendInterval(pause)
                .Append(canvasGroup.DOFade(targetFade, fadeDuration));
        }

        public Tween PlayHideDialog()
        {
            return DOTween.Sequence()
                .AppendInterval(pause)
                .Append(canvasGroup.DOFade(0f, fadeDuration));
        }
        
    }
}