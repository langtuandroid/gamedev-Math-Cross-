using UnityEngine;

namespace stickin
{
    [CreateAssetMenu(fileName = "AppData", menuName = "Stickin/App Data")]
    public class AppData : ScriptableObject
    {
        public Object[] ScenesMobile;
    }
}