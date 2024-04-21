using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    public class RewardResource : MonoBehaviour
    {
        [Header("Params")] 
        [SerializeField] private RewardResourcesConfig _config;
        [SerializeField] private string _resourceId;

        [Header("Views")] 
        [SerializeField] private Image _iconImage;
        [SerializeField] private Text _valueText;

        [InjectField] private ResourcesService _resourcesService;

        private void Start()
        {
            InjectService.BindFields(this);
            
            var resourceData = _config.GetResource(_resourceId);

            if (resourceData != null)
            {
                if (_iconImage != null)
                    _iconImage.sprite = _resourcesService.GetResourceSprite(_resourceId);

                if (_valueText != null)
                    _valueText.text = resourceData.Value.ToString();
            }
            else
            {
                Debug.LogError($"RewardResource: Not find RewardResourcesConfig for id = {_resourceId}");
            }
        }
    }
}