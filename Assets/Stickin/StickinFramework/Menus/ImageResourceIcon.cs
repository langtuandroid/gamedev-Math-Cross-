using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Image))]
    public class ImageResourceIcon : MonoBehaviour
    {
        [SerializeField] private string _resourceId;

        [InjectField] private ResourcesService _resourcesService;

        public void Init(string id)
        {
            _resourceId = id;
            RefreshView();
        }

        private void Start()
        {
            RefreshView();
        }

        private void RefreshView()
        {
            InjectService.BindFields(this);

            if (!string.IsNullOrEmpty(_resourceId) && _resourcesService != null)
            {
                var image = GetComponent<Image>();
                image.sprite = _resourcesService.GetResourceSprite(_resourceId);
                image.preserveAspect = true;
            }
        }
    }
}