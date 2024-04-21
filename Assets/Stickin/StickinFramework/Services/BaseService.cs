using System;
using UnityEngine;


namespace stickin
{
    public class BaseService : MonoBehaviour
    {
        [SerializeField] private bool _immediateComplete;
        
        protected AppData _appData;
        private Action<BaseService, bool> _callbackComplete;

        public virtual void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            Debug.Log($"Start init service: {name}");

            _appData = appData;
            _callbackComplete = callbackComplete;
            
            if (_immediateComplete)
                InitComplete(true);
        }

        protected void InitComplete(bool success)
        {
            Debug.Log($"End init service: {name}   success = {success}");
            
            _callbackComplete?.Invoke(this, success);
            _callbackComplete = null;
        }
    }
}