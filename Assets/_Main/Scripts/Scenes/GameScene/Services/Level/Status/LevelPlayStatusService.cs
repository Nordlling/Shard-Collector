using System;
using _Main.Scripts.Scenes.GameScene.Services.Level.CurrentLevel;

namespace _Main.Scripts.Scenes.GameScene.Services.Level.Status
{
    public class LevelPlayStatusService : ILevelPlayStatusService
    {
        private readonly ICurrentLevelService _currentLevelService;

        public LevelPlayStatusService(ICurrentLevelService currentLevelService)
        {
            _currentLevelService = currentLevelService;
        }

        public event Action OnUsedMove;

        public int AllMoves { get; private set; }
        public int UsedMoves { get; private set; }
        public int LeftMoves => AllMoves - UsedMoves;

        public int AllShapes { get; private set; }
        public int BusyShapes { get; private set; }
        public int FreeShapes => AllShapes - BusyShapes;

        public void InitNewLevel(int shapesCount)
        {
            AllMoves = shapesCount + _currentLevelService.GetCurrentLevel().ExtraMoves;
            AllShapes = shapesCount;
            BusyShapes = 0;
            UsedMoves = 0;
            OnUsedMove?.Invoke();
        }

        public void UseMove(bool isNewShape = true)
        {
            UsedMoves++;
            BusyShapes += isNewShape ? 1 : 0;
            OnUsedMove?.Invoke();
        }
    }
}