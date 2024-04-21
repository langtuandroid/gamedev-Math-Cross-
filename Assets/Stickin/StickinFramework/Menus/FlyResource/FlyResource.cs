using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin.menus
{
    [System.Serializable]
    public class FlyResourceDestination
    {
        [SerializeField] private string _id;
        [SerializeField] private Transform _transform;
        [SerializeField] private TextResourceValue _resourceText;

        public string Id => _id;
        public Transform Transform => _transform;
        public TextResourceValue ResourceText => _resourceText;
    }

    public class FlyResource : MonoBehaviour
    {
        [SerializeField] private List<FlyResourceDestination> _destinations;
        [SerializeField] private int _maxCount = 10;
        [SerializeField] private ImageResourceIcon _prefab;
        [SerializeField] private BaseFlyAnimation _flyAnimation;

        [InjectField] private ResourcesService _resourcesService;

        private void Awake()
        {
            InjectService.BindFields(this);
            _prefab.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            _resourcesService.OnChangeResource += OnChangeResource;
        }
        
        private void OnDisable()
        {
            if (_resourcesService != null)
                _resourcesService.OnChangeResource -= OnChangeResource;
        }
        

        private void OnChangeResource(string id, double changeValue, Transform fromTransform)
        {
            if (fromTransform != null)
            {
                var destination = GetResourceDestination(id);
                if (destination != null)
                {
                    var count = (int) changeValue;
                    var countPrefabs = Mathf.Min(count, _maxCount);
                    // var resourceValue = UserDataController.Instance.GetResourceValue(id);

                    for (var i = 0; i < countPrefabs; i++)
                    {
                        var fromPos = fromTransform != null ? fromTransform.position : Vector3.zero;
                        var view = Instantiate(_prefab, transform);
                        view.gameObject.SetActive(true);
                        view.transform.position = fromPos;
                        view.Init(id);

                        _flyAnimation.Fly(i, view.transform, fromPos, destination.Transform.position);
                    }
                }
            }
        }

        private void RefreshCountText(int changeCount)
        {
            // localizedResourceTextView.Refresh();
            // transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 2);

            // AddedCoinsDrop(changeCount);
        }

        private FlyResourceDestination GetResourceDestination(string id)
        {
            foreach (var destination in _destinations)
            {
                if (destination.Id == id)
                    return destination;
            }

            return null;
        }
    }
}