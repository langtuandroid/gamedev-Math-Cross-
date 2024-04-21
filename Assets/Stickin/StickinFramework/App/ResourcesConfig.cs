using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class ResourceConfigDouble
    {
        public string Id;
        public Sprite Sprite;
        public double DefaultValue = 0;
    }

    [System.Serializable]
    public class ResourceConfigString
    {
        public string Id;
        public Sprite Sprite;
        public string DefaultValue;
    }

    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "Stickin/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        public List<ResourceConfigDouble> ResourcesDouble;
        public List<ResourceConfigString> ResourcesString;
    }
}