using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Main.Scripts.Global.UI.Buttons.Animations
{
    public class ButtonBounceAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private float animationDuration = 0.1f;
        [SerializeField] private Vector3 originalScale = Vector3.one;
        [SerializeField] private Vector3 buttonPressedScale = new(0.9f, 0.9f, 0.9f);

        private Tween _tween;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateButton(buttonPressedScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            AnimateButton(originalScale);
        }
        
        private void AnimateButton(Vector2 scaleTarget)
        {
            _tween?.Kill();
            _tween = image.transform.DOScale(scaleTarget, animationDuration);
        }
        
    }
}