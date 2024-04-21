using UnityEngine;

namespace stickin
{
    public class RotateGameObject : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotateSpeed;

        private void Update()
        {
            transform.localEulerAngles += _rotateSpeed * Time.deltaTime;
        }
    }
}