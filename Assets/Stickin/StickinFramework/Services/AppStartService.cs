using System;
using UnityEngine;

namespace stickin
{
    public class AppStartService : BaseService
    {
        public override void Init(AppData appData, Action<BaseService, bool> callbackComplete)
        {
            base.Init(appData, callbackComplete);
            
#if UNITY_IOS || UNITY_IPHONE || UNITY_ANDROID
            Application.targetFrameRate = 60;
#else
            Application.targetFrameRate = -1;
#endif

            gameObject.AddComponent<Updater>();

            InitComplete(true);
        }
    }
}