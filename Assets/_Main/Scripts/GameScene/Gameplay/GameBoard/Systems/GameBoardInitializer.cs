using System;
using _Main.Scripts.GameScene.MonoInstallers;
using _Main.Scripts.Toolkit.Screen;
using App.Scripts.Modules.EcsWorld.Infrastructure.Services;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Gameplay.GameBoard
{
    public class GameBoardInitializer
    {
        private readonly IScreenService _screenService;
        private readonly ShapeSelectorConfig _shapeSelectorConfig;
        private readonly IWorldRunner _worldRunner;
        private readonly GameBoardContent _gameBoardContent;

        public GameBoardInitializer(IScreenService screenService, ShapeSelectorConfig shapeSelectorConfig, 
            IWorldRunner worldRunner, GameBoardContent gameBoardContent)
        {
            _screenService = screenService;
            _shapeSelectorConfig = shapeSelectorConfig;
            _worldRunner = worldRunner;
            _gameBoardContent = gameBoardContent;
        }

        public void Init()
        {
            Rect shapeSelectorRect = _screenService.CalculateRect(
                _shapeSelectorConfig.RelativePositionX,
                _shapeSelectorConfig.RelativePositionY,
                _shapeSelectorConfig.RelativeWidth,
                _shapeSelectorConfig.RelativeHeight);

            _gameBoardContent.ShapeSelectorContent.transform.position = shapeSelectorRect.center;

            float relativeHeightWithoutPaddings = 1f - _shapeSelectorConfig.RelativePaddingTop - _shapeSelectorConfig.RelativePaddingBottom;
            relativeHeightWithoutPaddings = Math.Max(0f, relativeHeightWithoutPaddings);
            float relativeCellHeight = (relativeHeightWithoutPaddings / _shapeSelectorConfig.MaxCount) - (_shapeSelectorConfig.CellOffset * 2f);
            relativeCellHeight = Math.Max(0f, relativeCellHeight);
            
            Entity entity = _worldRunner.CreateEntity();
            
            entity.SetComponent(new ShapeSelectorComponent
            {
                Transform = _gameBoardContent.ShapeSelectorContent,
                Rect = shapeSelectorRect,
                PaddingTop = _shapeSelectorConfig.RelativePaddingTop * shapeSelectorRect.width,
                PaddingBottom = _shapeSelectorConfig.RelativePaddingBottom * shapeSelectorRect.height,
                CellOffset = _shapeSelectorConfig.CellOffset * shapeSelectorRect.height,
                CellSize = relativeCellHeight * shapeSelectorRect.height,
                MaxCount = _shapeSelectorConfig.MaxCount
            });

            entity.AddComponent<AllShapesInSelectorSignal>();
        }
    }
}