using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.Global.DialogSystem.Services
{
    public class BounceDialogAnimator : MonoBehaviour, IDialogAnimator
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Vector3 targetScale = Vector3.one;
        [SerializeField] private float scaleDuration = 0.3f;
        [SerializeField] private float pause;
        [SerializeField] private float maxScale = 1.1f;

        public Tween PlayShowDialog()
        {
            rectTransform.localScale = new Vector3(0, 0, 1);
            return DOTween.Sequence()
                .AppendInterval(pause)
                .Append(rectTransform.DOScale(targetScale * maxScale, 0.70f * scaleDuration))
                .Append(rectTransform.DOScale(targetScale, 0.30f * scaleDuration));
        }

        public Tween PlayHideDialog()
        {
            return DOTween.Sequence()
                .AppendInterval(pause)
                .Append(rectTransform.DOScale(targetScale * maxScale, 0.30f * scaleDuration))
                .Append(rectTransform.DOScale(Vector3.zero, 0.70f * scaleDuration));
        }
        
    }
}