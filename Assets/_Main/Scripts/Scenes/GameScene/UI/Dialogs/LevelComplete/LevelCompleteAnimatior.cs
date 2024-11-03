using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Scripts.Scenes.GameScene.UI.Dialogs.LevelComplete
{
    public class LevelCompleteAnimatior : MonoBehaviour
    {
        [SerializeField] private Image star1;
        [SerializeField] private Image star2;
        [SerializeField] private Image star3;
        
        [SerializeField] private TextMeshProUGUI percentLabel;
        [SerializeField] private Color startColor;
        [SerializeField] private Color finishColor;

        [Header("Animation Settings")] 
        [SerializeField] private float allStarsDuration = 0.5f;


        public Sequence PlayAnimation(int percent, float star1FillAmount, float star2FillAmount,
            float star3FillAmount)
        {
            return DOTween.Sequence()
                .Join(AnimateCounter(percent))
                .Join(AnimateStars(star1FillAmount, star2FillAmount, star3FillAmount));
        }

        private Tween AnimateCounter(int percent)
        {
            int oldCounter = 0;
            int newCounter = percent;
            
            return DOTween.Sequence()
                .AppendInterval(0f)
                .Append(DOTween
                    .To(() => oldCounter, x => oldCounter = x, newCounter, 1.5f)
                    .OnUpdate(() =>
                    {
                        percentLabel.text = $"{oldCounter}%";
                        percentLabel.color = Color.Lerp(startColor, finishColor, oldCounter / 100f);
                    }))
                .OnKill(() =>
                {
                    percentLabel.text = $"{newCounter}%";
                });
        }

        private Tween AnimateStars(float star1FillAmount, float star2FillAmount, float star3FillAmount)
        {
            return DOTween.Sequence()
                .Append(star1.DOFillAmount(star1FillAmount, allStarsDuration / 3))
                .Append(star2.DOFillAmount(star2FillAmount, allStarsDuration / 3))
                .Append(star3.DOFillAmount(star3FillAmount, allStarsDuration / 3));
        }

        public void Reset()
        {
            star1.fillAmount = 0f;
            star2.fillAmount = 0f;
            star3.fillAmount = 0f;
        }
    }
}