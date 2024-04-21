using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class Pie : MonoBehaviour
    {
        [SerializeField] private RectTransform _line2;
        
        [Header("Prize")]
        [SerializeField] private RectTransform _prizeRt;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Text _countText;

        [InjectField] private ResourcesService _resourcesService;
        
        private RectTransform _rt;
        private Image _image;
        private float _angleStep;
        
        public ResourceData Data { get; private set; }

        public bool IsWin => ThisIsWin();

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        public void Init(RewardResourcesConfig data, int index, int count, Color color)
        {
            InjectService.BindFields(this);
            
            RefreshData(data);
            
            _angleStep = 360f / count;
            
            _rt.localEulerAngles = Vector3.forward * index * _angleStep;
            _image.fillAmount = 1f / count;
            _image.color = color;

            _line2.localEulerAngles = Vector3.forward * (-360f / count);
            _prizeRt.localEulerAngles = Vector3.forward * (180 - _angleStep / 2);
        }

        private void RefreshData(RewardResourcesConfig data)
        {
            Data = data.Resources[0];
            _countText.text = Data.Value.ToString();
            _iconImage.sprite = _resourcesService.GetResourceSprite(Data.Id);
        }

        private bool ThisIsWin()
        {
            var angle = _rt.eulerAngles.z;
            return angle >= 180 && angle <= 180 + _angleStep;
        }
    }
}
