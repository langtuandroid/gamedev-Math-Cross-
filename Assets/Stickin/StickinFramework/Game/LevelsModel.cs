using System.Collections.Generic;

namespace stickin
{
    public class LevelsModel<T>
    {
        public List<T> Levels = new List<T>();
        
        public T GetLevelModel(int index) => Levels[index % Levels.Count];
    }
    
    [System.Serializable]
    public class LangLevelsModel<T> : LevelsModel<T>
    {
        public List<string> Langs;
    }
}