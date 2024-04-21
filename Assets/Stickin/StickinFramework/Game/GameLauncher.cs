using System.Collections;
using stickin;
using UnityEngine;

namespace stickin
{
    public static class GameLauncher
    {
        public static void ReplayCurrentLevel()
        {
            var gameView = GameObject.FindObjectOfType<GameView>();
            PlayLevel(gameView.GameParams);
        }
        
        public static void Restart()
        {
            var gameView = GameObject.FindAnyObjectByType<GameView>();
            PlayLevel(gameView.GameParams);
        }

        public static void PlayLevel(int levelNumber, OrderAssetType orderType, Hashtable customData = null)
        {
            PlayLevel(new GameParams(levelNumber, orderType, customData));
        }
        
        public static void PlayLevel(int levelNumber, string levelTitle, OrderAssetType orderType, Hashtable customData = null)
        {
            PlayLevel(new GameParams(levelNumber, levelTitle, orderType, customData));
        }
        
        public static void PlayLevel(GameParams gameParams)
        {
            var hashtable = new Hashtable {["gameParams"] = gameParams};
            
            SceneLoader.LoadScene(3, hashtable); // game scene
        }
    }
}