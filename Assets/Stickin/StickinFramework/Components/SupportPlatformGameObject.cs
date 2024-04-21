using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class SupportPlatformGameObject : MonoBehaviour
    {
        [SerializeField] private List<RuntimePlatform> _supportedPlatforms;
        
        private void Start()
        {
            var isSupported = _supportedPlatforms != null && _supportedPlatforms.Contains(Application.platform);
            gameObject.SetActive(isSupported);
        }
    }
}