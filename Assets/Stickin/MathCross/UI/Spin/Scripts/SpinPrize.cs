using UnityEngine;
using UnityEngine.UI;

namespace stickin.mathcross
{
    public class SpinPrize : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Text _countText;

        [InjectField] private ResourcesService _resourcesService;

        private Animator _animator;

        public void Init(ResourceData data)
        {
            InjectService.BindFields(this);

            _iconImage.sprite = _resourcesService.GetResourceSprite(data.Id);
            _countText.text = $"+{data.Value}";
            
            _resourcesService.ChangeResource(data.Id, data.Value);

            Show();
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            EndHide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            _animator.ResetTrigger("hide");

//        _animator.SetTrigger("show");

            Invoke("Hide", 2f);
        }

        private void Hide()
        {
            _animator.ResetTrigger("show");

            _animator.SetTrigger("hide");

            Invoke("EndHide", 0.3f);
        }

        private void EndHide()
        {
            gameObject.SetActive(false);
        }
    }
}