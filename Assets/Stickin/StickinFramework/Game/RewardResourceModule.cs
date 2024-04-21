using stickin;
using UnityEngine;

namespace stickin
{
    public class RewardResourceModule : IGameModule
    {
        public string Id { get; private set; }
        public int Value { get; private set; }

        private ResourcesService _resourcesService;

        public RewardResourceModule(string id, int value)
        {
            Id = id;
            Value = value;
        }
        
        public void SetResourcesService(ResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }
        
        public void IncResource(int value, Transform fromTransform)
        {
            Value += value;

            if (_resourcesService != null)
                _resourcesService.ChangeResource(Id, value, fromTransform);
        }

        public void Stop() { }
        public void Pause() { }
        public void Resume() { }
        public void Destroy() { }
    }
}