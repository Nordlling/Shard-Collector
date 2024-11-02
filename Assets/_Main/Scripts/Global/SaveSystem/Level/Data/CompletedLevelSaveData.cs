using System;
using System.Collections.Generic;

namespace _Main.Scripts.Global.SaveSystem.Level.Data
{
    [Serializable]
    public class CompletedLevelsSaveData
    {
        public List<CompletedLevelSaveData> CompletedLevels;
    }

    [Serializable]
    public class CompletedLevelSaveData
    {
        public int LevelIndex;
        public int Percent;
        public int Stars;
    }
}