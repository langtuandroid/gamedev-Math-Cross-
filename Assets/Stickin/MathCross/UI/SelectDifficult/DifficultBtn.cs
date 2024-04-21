using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace stickin.mathcross
{
    [RequireComponent(typeof(Button))]
    public class DifficultBtn : MonoBehaviour
    {
        [SerializeField] private LevelDifficult _difficult;
        [SerializeField] private UnityEvent<LevelDifficult> _onClick;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _onClick?.Invoke(_difficult);
        }
    }
}
