using UnityEngine;
using UnityEngine.UI;

namespace stickin.menus
{
    [RequireComponent(typeof(Text))]
    public class TextResourceValue : MonoBehaviour
    {
        [SerializeField] private string _resourceId;
        [SerializeField] private string _format;
        [SerializeField] private bool _updateFromEvent = true;
    
        [InjectField] private ResourcesService _resourcesService;
        
        private Text _txt;

        public void Init(string resourceId)
        {
            InjectService.BindFields(this);
            
            _resourceId = resourceId;
            Refresh();
        }
        
        private void Awake()
        {
            InjectService.BindFields(this);
            
            _txt = GetComponent<Text>();
        }

        private void OnEnable()
        {
            _resourcesService.OnUserUpdate += OnChangeResource;
            Refresh();
        }

        private void OnDisable()
        {
            _resourcesService.OnUserUpdate -= OnChangeResource;
        }

        public void Refresh()
        {
            var value = _resourcesService.GetResourceValue(_resourceId);
            SetValue(value);
        }

        public void SetValue(double value)
        {
            var str = string.IsNullOrEmpty(_format) ? value.ToString() : string.Format(_format, value);

            if (_txt != null)
                _txt.text = str;
        }

        private void OnChangeResource(string id, double value, Transform tr)
        {
            if (_updateFromEvent && id == _resourceId)
                Refresh();
        }
    }
}