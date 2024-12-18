using System;

namespace _Main.Scripts.Scenes.GameScene.Services.Level.Status
{
    public interface ILevelPlayStatusService
    {
        event Action OnUsedMove;

        bool IsPlaying { get; }
        
        int AllMoves { get; }
        int UsedMoves { get; }
        int LeftMoves { get; }
        
        int AllShapes { get; }
        int BusyShapes { get; }
        int FreeShapes { get; }
        
        void InitNewLevel(int shapesCount);
        void UseMove(bool isNewShape = true);
        void MarkAsFinished();
    }
}