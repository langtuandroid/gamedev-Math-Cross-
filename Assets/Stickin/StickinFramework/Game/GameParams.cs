using System.Collections;

namespace stickin
{
    public struct GameParams
    {
        public int LevelNumber;
        public string LevelTitle;
        public OrderAssetType OrderType;
        public Hashtable CustomData;

        public GameParams(int levelNumber, Hashtable customData = null)
        {
            LevelNumber = levelNumber;
            LevelTitle = $"Level {levelNumber}";
            OrderType = OrderAssetType.Levels;
            CustomData = customData;
        }
        
        public GameParams(int levelNumber, OrderAssetType orderType, Hashtable customData = null)
        {
            LevelNumber = levelNumber;
            LevelTitle = $"Level {levelNumber}";
            OrderType = orderType;
            CustomData = customData;
        }
        
        public GameParams(int levelNumber, string levelTitle, OrderAssetType orderType, Hashtable customData = null)
        {
            LevelNumber = levelNumber;
            LevelTitle = levelTitle;
            OrderType = orderType;
            CustomData = customData;
        }
    }
}