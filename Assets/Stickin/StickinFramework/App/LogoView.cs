using System;
using UnityEngine;

namespace stickin
{
    public class LogoView : MonoBehaviour
    {
        public Action _animationCompleteCallback;

        public void Init(Action completeCallback)
        {
            _animationCompleteCallback = completeCallback;
        }
        
        public void AnimationComplete() // unity animator event
        {
            _animationCompleteCallback?.Invoke();
            _animationCompleteCallback = null;
        }
    }
}