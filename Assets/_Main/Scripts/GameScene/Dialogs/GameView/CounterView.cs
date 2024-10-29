using TMPro;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.GameScene.Dialogs.GameView
{
    public class CounterView : MonoBehaviour
    {
        [SerializeField] private string movesLabelText;
        [SerializeField] private TextMeshProUGUI movesCountLabel;
        [SerializeField] private string shapeLabelText;
        [SerializeField] private TextMeshProUGUI shapeCountLabel;
        
        private ILevelPlayStatusService _levelPlayStatusService;

        [Inject]
        public void Construct(ILevelPlayStatusService levelPlayStatusService)
        {
            _levelPlayStatusService = levelPlayStatusService;
        }

        private void OnEnable()
        {
            UpdateView();
            _levelPlayStatusService.OnUsedMove += UpdateView;
        }
        
        private void OnDisable()
        {
            _levelPlayStatusService.OnUsedMove -= UpdateView;
        }

        private void UpdateView()
        {
            movesCountLabel.text = string.Format(movesLabelText, _levelPlayStatusService.LeftMoves.ToString());
            shapeCountLabel.text = string.Format(shapeLabelText, _levelPlayStatusService.FreeShapes.ToString());
        }
    }
}