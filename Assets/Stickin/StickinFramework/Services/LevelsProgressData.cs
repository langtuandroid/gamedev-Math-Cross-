using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public enum LevelProgressType
    {
        None = 0,
        Started = 1,
        Done = 2
    }
    
    [System.Serializable]
    public class  LevelProgressData
    {
        [SerializeField] private int l; // level number
        [SerializeField] private int p; // progress type
        [SerializeField] private int pp; // progress percentage
        [SerializeField] private string c; // custom json data

        public int LevelNumber => l;
        public LevelProgressType ProgressType => (LevelProgressType) p;
        public string CustomData => c;

        public LevelProgressData(int index)
        {
            l = index;
        }
        
        public void SetCustomStr(string str)
        {
            c = str;
        }

        public void SetProgressType(LevelProgressType type)
        {
            p = (int) type;
        }
    }
    
    [System.Serializable]
    public class LevelsProgressData
    {
        public List<LevelProgressData> Levels;
    }
}