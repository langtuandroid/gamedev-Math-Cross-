using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    [System.Serializable]
    public class UserData
    {
        public const string SESSIONS = "sessions";
    
        public string Name;
        public List<ResourceData> Resources;

        [System.NonSerialized] private ResourcesConfig _resourcesConfigData;

        public UserData()
        {
            Resources = new List<ResourceData>();
            Name = "";
        }

        public void Init(ResourcesConfig config)
        {
            _resourcesConfigData = config;
        }

        public double ChangeResource(string id, double value)
        {
            var resource = GetResourceData(id);
            resource.Value += value;

            return resource.Value;
        }
    
        public void SetResource(string id, double value)
        {
            var resource = GetResourceData(id);
            resource.Value = value;
        }

        public void DeleteResource(string id)
        {
            foreach (var res in Resources)
            {
                if (res.Id == id)
                {
                    Resources.Remove(res);
                    break;
                }
            }
        }

        public ResourceData GetResourceData(string id)
        {
            foreach (var resource in Resources)
            {
                if (resource.Id == id)
                    return resource;
            }

            var newResource = new ResourceData();
            newResource.Id = id;
            newResource.Value = GetResourceDefaultValue(id);

            Resources.Add(newResource);
        
            return newResource;
        }

        private double GetResourceDefaultValue(string id)
        {
            if (_resourcesConfigData != null)
            {
                foreach (var resource in _resourcesConfigData.ResourcesDouble)
                {
                    if (resource.Id == id)
                        return resource.DefaultValue;
                }
            }

            return 0;
        }

        public Sprite GetResourceSprite(string id)
        {
            if (_resourcesConfigData != null)
            {
                foreach (var resource in _resourcesConfigData.ResourcesDouble)
                {
                    if (resource.Id == id)
                        return resource.Sprite;
                }
            }
        
            Debug.LogError($"UserData.GetResourceSprite: not sprite for resource = {id}");
            return null;
        }
    }
}