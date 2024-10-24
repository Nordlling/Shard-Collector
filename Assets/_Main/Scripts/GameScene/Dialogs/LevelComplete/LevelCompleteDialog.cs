using _Main.Scripts.Common.Dialogs.DialogElements;
using App.Scripts.Modules.Dialogs.Interfaces;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.Dialogs
{
    public class LevelCompleteDialog : PoolableDialog
    {
        [SerializeField] private TextMeshProUGUI gameOverLabel;
        [SerializeField] private TextMeshProUGUI percentLabel;
        [SerializeField] private StarsAnimator starsAnimator;
        [SerializeField] private ButtonHandler closeButton;
        [SerializeField] private ButtonHandler nextLevelButton;

        private LevelCompleteConfig _levelCompleteConfig;
        private IGameSceneCreator _gameSceneCreator;
        private int _percent;

        [Inject]
        public void Construct(LevelCompleteConfig levelCompleteConfig, IGameSceneCreator gameSceneCreator)
        {
            _levelCompleteConfig = levelCompleteConfig;
            _gameSceneCreator = gameSceneCreator;
        }

        public void Setup(int percent)
        {
            _percent = percent;
        }

        protected override void OnDialogShown()
        {
            base.OnDialogShown();
            nextLevelButton.Button.onClick.AddListener(StartNextLevel);
            closeButton.Button.onClick.AddListener(CloseDialog);
            Animate();
        }

        protected override void OnDialogReset()
        {
            base.OnDialogReset();
            nextLevelButton.Button.onClick.RemoveListener(StartNextLevel);
            closeButton.Button.onClick.RemoveListener(CloseDialog);
        }

        private void Animate()
        {
            AnimateCounter(_percent);
            InitStarsAnimator(_percent);
        }

        private void StartNextLevel()
        {
            Close(null);
            _gameSceneCreator.Recreate();
        }

        private void CloseDialog()
        {
            Close(null);
        }


        private void AnimateCounter(int percent)
        {
            int oldCounter = 0;
            int newCounter = percent;
            
            DOTween.Sequence()
                .AppendInterval(0f)
                .Append(DOTween
                    .To(() => oldCounter, x => oldCounter = x, newCounter, 1.5f)
                    .OnUpdate(() => percentLabel.text = $"{oldCounter}%"))
                .OnKill(() =>
                {
                    percentLabel.text = $"{newCounter}%";
                });
        }

        private void InitStarsAnimator(float percent)
        {
            float star1FillAmount = percent / _levelCompleteConfig.FirstStarFullPercent;
            float star2FillAmount = (percent - _levelCompleteConfig.FirstStarFullPercent) /
                                    (_levelCompleteConfig.SecondStarFullPercent - _levelCompleteConfig.FirstStarFullPercent);
            float star3FillAmount = (percent - _levelCompleteConfig.SecondStarFullPercent) /
                                    (_levelCompleteConfig.ThirdStarFullPercent - _levelCompleteConfig.SecondStarFullPercent);

            starsAnimator.PlayAnimation(star1FillAmount, star2FillAmount, star3FillAmount);
        }
        
    }
}