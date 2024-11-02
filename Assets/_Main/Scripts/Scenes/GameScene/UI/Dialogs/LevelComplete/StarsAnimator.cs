using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Scenes.GameScene.UI.Dialogs.LevelComplete
{
    public class StarsAnimator : MonoBehaviour
    {
        [SerializeField] private Image star1;
        [SerializeField] private Image star2;
        [SerializeField] private Image star3;

        [Header("Animation Settings")] 
        [SerializeField] private float starDuration = 0.5f;


        public Sequence PlayAnimation(float star1FillAmount, float star2FillAmount, float star3FillAmount)
        {
            return DOTween.Sequence()
                .Append(star1.DOFillAmount(star1FillAmount, starDuration))
                .Append(star2.DOFillAmount(star2FillAmount, starDuration))
                .Append(star3.DOFillAmount(star3FillAmount, starDuration));
        }

        public void Reset()
        {
            star1.fillAmount = 0f;
            star2.fillAmount = 0f;
            star3.fillAmount = 0f;
        }
    }
}