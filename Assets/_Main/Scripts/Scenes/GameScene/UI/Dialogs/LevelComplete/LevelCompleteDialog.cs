using _Main.Scripts.Global.DialogSystem.Services;
using _Main.Scripts.Global.GameStateMachine;
using _Main.Scripts.Global.UI.Buttons.Handlers;
using _Main.Scripts.Scenes.GameScene.Gameplay.LevelComplete.Configs;
using _Main.Scripts.Scenes.GameScene.GameSceneStates;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.UI.Dialogs.LevelComplete
{
    public class LevelCompleteDialog : PoolableDialog
    {
        [SerializeField] private TextMeshProUGUI gameOverLabel;
        [SerializeField] private TextMeshProUGUI percentLabel;
        [SerializeField] private TextMeshProUGUI nextLevelLabel;
        [SerializeField] private string nextLevelText;
        [SerializeField] private string restartText;
        [SerializeField] private StarsAnimator starsAnimator;
        [SerializeField] private IButtonHandler nextLevelButtonHandler;
        [SerializeField] private IButtonHandler mainMenuButtonHandler;

        private IGameStateMachine _gameStateMachine;
        private LevelCompleteConfig _levelCompleteConfig;
        private ICurrentLevelService _currentLevelService;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, LevelCompleteConfig levelCompleteConfig, ICurrentLevelService currentLevelService)
        {
            _gameStateMachine = gameStateMachine;
            _levelCompleteConfig = levelCompleteConfig;
            _currentLevelService = currentLevelService;
        }

        protected override void OnDialogStartShow()
        {
            base.OnDialogStartShow();
            percentLabel.text = "0%";
            nextLevelLabel.text = _currentLevelService.CanLevelUp ? nextLevelText : restartText;
            starsAnimator.Reset();
        }

        protected override void OnDialogFinishShow()
        {
            base.OnDialogFinishShow();
            nextLevelButtonHandler.Button.onClick.AddListener(StartNextLevel);
            mainMenuButtonHandler.Button.onClick.AddListener(GoToMainMenu);
            Animate();
        }

        protected override void OnDialogReset()
        {
            base.OnDialogReset();
            nextLevelButtonHandler.Button.onClick.RemoveListener(StartNextLevel);
            mainMenuButtonHandler.Button.onClick.RemoveListener(GoToMainMenu);
        }

        private void Animate()
        {
            int percent = _currentLevelService.LastCompletedLevel.Percent;
            AnimateCounter(percent);
            InitStarsAnimator(percent);
        }

        private void StartNextLevel()
        {
            Close(() => _gameStateMachine.Enter<StartLevelState>());
        }

        private void GoToMainMenu()
        {
            Close(() => mainMenuButtonHandler.Execute());
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