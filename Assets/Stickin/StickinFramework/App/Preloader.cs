using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class Preloader : MonoBehaviour
    {
        [SerializeField] private LogoView _logoView;
        
        [Space(20)] 
        [SerializeField] private AppData _appData; 
        [SerializeField] private List<BaseService> _services;
        
        private bool _logoComplete;
        private int _currentServiceIndex;
        private bool AllServicesInit => _currentServiceIndex >= _services.Count;

        private IEnumerator Start()
        {
            _logoView.Init(OnLogoComplete);

            _currentServiceIndex = 0;

            yield return null;
            InitNextService();
        }

        private void InitNextService()
        {
            if (_currentServiceIndex >= 0 && _currentServiceIndex < _services.Count)
            {
                Debug.Log($"Start init service: {_services[_currentServiceIndex].name}");
                _services[_currentServiceIndex].Init(_appData, OnInitServiceComplete);
            }
            else
                EndInitServices();
        }

        private void OnInitServiceComplete(BaseService service, bool success)
        {
            Debug.Log($"Finish init service: {service.name}     success = {success}");
            
            _currentServiceIndex++;

            if (_currentServiceIndex >= 0 && _currentServiceIndex < _services.Count)
                InitNextService();
            else
                EndInitServices();
        }

        private void EndInitServices()
        {
            Debug.Log("Preloader.EndInitServices");
            CheckAllComplete();
        }

        public void OnLogoComplete()
        {
            Debug.Log("Preloader.OnLogoComplete");
            
            _logoComplete = true;
            CheckAllComplete();
        }
        
        private void CheckAllComplete()
        {
            if (AllServicesInit && _logoComplete)
                GoNextScene();
        }
        
        private void GoNextScene()
        {
            SceneLoader.LoadScene(2);
        }
    }
}