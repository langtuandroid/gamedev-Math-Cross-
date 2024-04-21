using System.Collections.Generic;
using stickin;
using UnityEngine;

namespace stickin.mathcross
{
    [System.Serializable]
    public class DifficultConfig
    {
        public LevelDifficult Difficult;
        public Vector2Int Size;
        public int MaxNumber;
        [Range(0, 100)]
        public int PercentageNumbersToPocket;
        [Range(0, 100)]
        public int PercentageRewards;
    }
    
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Stickin/Math Cross/Game Config")]
    public class MathCrossGameConfig : GameConfig
    {
        [Header("MathCross params:")]
        public List<DifficultConfig> DifficultConfigs;

        public DifficultConfig GetDifficultConfig(LevelDifficult difficult)
        {
            foreach (var config in DifficultConfigs)
            {
                if (config.Difficult == difficult)
                    return config;
            }

            return DifficultConfigs.GetRandom();
        }
    }
}