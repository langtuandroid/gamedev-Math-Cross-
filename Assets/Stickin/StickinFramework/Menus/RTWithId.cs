using UnityEngine;

namespace stickin
{
    public class RTWithId : MonoBehaviour
    {
        [SerializeField] private string _id;

        public string Id => _id;
    }
}