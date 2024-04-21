using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Image))]
    public class AlphaImage : MonoBehaviour
    {
        [SerializeField] private float _alpha = 0f;

        private void Start()
        {
            var image = GetComponent<Image>();
            var color = image.color;
            color.a = _alpha;
            image.color = color;
        }
    }
}