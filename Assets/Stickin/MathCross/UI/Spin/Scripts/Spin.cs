using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace stickin.mathcross
{
    public class Spin : MonoBehaviour
    {
        [SerializeField] private Pie _piePrefab;
        [SerializeField] private Color[] _colors;
        [SerializeField] private RectTransform _bgRt;
        [SerializeField] private List<RewardResourcesConfig> _prizes;
        [SerializeField] private SpinPrize _spinPrize;

        public event Action OnStart;
        public event Action OnEnd;

        public bool IsRun { get; private set; }

        private List<Pie> _pies = new List<Pie>();

        public void Run()
        {
            var speed = Random.Range(4f, 6f);
            var duration = Random.Range(2.5f, 3.5f);

            StartCoroutine(Rotate(speed, duration));
            
            IsRun = true;
            OnStart?.Invoke();
        }

        private void Start()
        {
            for (int i = 0; i < _prizes.Count; i++)
            {
                var pie = Instantiate(_piePrefab, _piePrefab.transform.parent);
                pie.Init(_prizes[i], i, _prizes.Count, _colors[i % _colors.Length]);

                _pies.Add(pie);
            }

            _piePrefab.gameObject.SetActive(false);
        }

        private IEnumerator Rotate(float speed, float duration)
        {
            var currentSpeed = 0f;
            var accelerationUp = 3f;
            var accelerationDown = 2f;
            var angle = _bgRt.localEulerAngles.z;
            
            float currentTime = 0.0f;

            do
            {
                currentSpeed = Mathf.Min(speed, currentSpeed + accelerationUp * Time.deltaTime);
                angle += currentSpeed;
                _bgRt.localEulerAngles = Vector3.forward * angle;

                currentTime += Time.deltaTime;
                
                yield return null;
            } while (currentTime <= duration);

            while (currentSpeed > 0)
            {
                currentSpeed -= accelerationDown * Time.deltaTime;
                angle += currentSpeed;
                _bgRt.localEulerAngles = Vector3.forward * angle;
                
                yield return null;
            }
            
            // centered
            currentTime = 0f;
            duration = 0.2f;
            var step = 360 / _pies.Count;
            var startAngle = _bgRt.localEulerAngles.z;
            var needAngle = ((int)(_bgRt.localEulerAngles.z / step) + 0.5f) * step;
            
            do
            {
                angle = Mathf.Lerp(startAngle, needAngle, currentTime / duration);
                _bgRt.localEulerAngles = Vector3.forward * angle;

                currentTime += Time.deltaTime;
                
                yield return null;
            } while (currentTime <= duration);
            
            _bgRt.localEulerAngles = Vector3.forward * needAngle;

            RotateEnd();
        }

        private void RotateEnd()
        {
            ShowPrize();
            
            IsRun = false;
            OnEnd?.Invoke();
        }

        private void ShowPrize()
        {
            foreach (var pie in _pies)
            {
                if (pie.IsWin)
                {
                    Debug.Log("WIN:    " + pie.Data.Id + "  " + pie.Data.Value);

                    _spinPrize.Init(pie.Data);

                    break;
                }
            }
            
        }
    }
}