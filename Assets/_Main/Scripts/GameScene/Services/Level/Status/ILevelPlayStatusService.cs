using System;

namespace _Main.Scripts.GameScene
{
    public interface ILevelPlayStatusService
    {
        event Action OnUsedMove;
        
        int AllMoves { get; }
        int UsedMoves { get; }
        int LeftMoves { get; }
        
        int AllShapes { get; }
        int BusyShapes { get; }
        int FreeShapes { get; }
        
        void InitNewLevel(int shapesCount);
        void UseMove(bool isNewShape = true);
    }
}