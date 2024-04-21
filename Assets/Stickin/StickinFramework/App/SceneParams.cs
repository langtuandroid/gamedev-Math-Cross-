using System;
using System.Collections;
using UnityEngine;

namespace stickin
{
    public class SceneParams : MonoBehaviour
    {
        public event Action<Hashtable> OnSetData;

        public void SetData(Hashtable data)
        {
            OnSetData?.Invoke(data);
        }

    }
}