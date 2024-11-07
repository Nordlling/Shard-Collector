using System;
using _Main.Scripts.Global.UI.Buttons.Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.DragAndDrop.Systems
{
    public class LayerSelectorView : MonoBehaviour
    {
        [SerializeField] private SimpleButtonHandler layerUpButton;
        [SerializeField] private SimpleButtonHandler layerDownButton;
        [SerializeField] private SimpleButtonHandler resetButton;
        [SerializeField] private TextMeshProUGUI layerLabel;
        
        private ILayerService _layerService;


        [Inject]
        public void Construct(ILayerService layerService)
        {
            _layerService = layerService;
        }
        
        private void OnEnable()
        {
            layerUpButton.Button.onClick.AddListener(LayerUp);
            layerDownButton.Button.onClick.AddListener(LayerDown);
            resetButton.Button.onClick.AddListener(ResetLayer);
            _layerService.OnLayersReset += ResetView;
        }

        private void OnDisable()
        {
            layerUpButton.Button.onClick.RemoveListener(LayerUp);
            layerDownButton.Button.onClick.RemoveListener(LayerDown);
            resetButton.Button.onClick.RemoveListener(ResetLayer);
            _layerService.OnLayersReset -= ResetView;
        }

        private void LayerUp()
        {
            int currentLayer = _layerService.CurrentLayer;
            int layersCount = _layerService.LayersCount;

            currentLayer++;

            if (currentLayer >= layersCount)
            {
                currentLayer = 0;
            }
            
            _layerService.ChangeLayerView(currentLayer);
            layerLabel.text = currentLayer.ToString();
        }

        private void LayerDown()
        {
            int currentLayer = _layerService.CurrentLayer;
            int layersCount = _layerService.LayersCount;

            currentLayer--;

            if (currentLayer < 0)
            {
                currentLayer = layersCount - 1;
            }
            
            _layerService.ChangeLayerView(currentLayer);
            layerLabel.text = currentLayer.ToString();
        }

        private void ResetLayer()
        {
            _layerService.ChangeLayerView(-1);
            ResetView();
        }

        private void ResetView()
        {
            layerLabel.text = "-1";
        }
        
    }
}