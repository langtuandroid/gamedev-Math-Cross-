using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    [RequireComponent(typeof(Button))]
    public class ButtonGotoScene : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SceneLoader.LoadScene(_sceneIndex);
        }
    }
}