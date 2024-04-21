using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class SoundConfig
    {
        public string Id;
        public AudioClip Clip;
        public bool Loop;
        public bool IsMusic;
        [Range(0, 1)] public float Volume = 1f;
    }
    
    [CreateAssetMenu(fileName = "SoundsConfig", menuName = "Stickin/Sounds Config")]
    public class SoundsConfig : ScriptableObject
    {
        public SoundConfig[] Sounds;
    }
}