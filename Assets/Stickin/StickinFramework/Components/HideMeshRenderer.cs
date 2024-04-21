using UnityEngine;

namespace stickin
{
    [RequireComponent(typeof(MeshRenderer))]
    public class HideMeshRenderer : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

    }
}