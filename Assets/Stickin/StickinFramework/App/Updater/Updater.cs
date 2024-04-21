using System;
using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public class Updater : MonoBehaviour
    {
        private class CallDelay
        {
            public Action Action;
            public float Delay;
            public bool IsUnscaled;

            public CallDelay(Action action, float delay, bool isUnscaled)
            {
                Action = action;
                Delay = delay;
                IsUnscaled = isUnscaled;
            }
        }

        private List<ITick> _ticks;
        private List<ITickFixed> _fixedTicks;
        private List<ITickLater> _laterTicks;

        private List<CallDelay> _delayedCalls;
        private List<CallDelay> _removedCalls;

        private List<ITick> _removedTicks;
        private List<ITickFixed> _removedFixedTicks;
        private List<ITickLater> _removedLaterTicks;

        private List<ITick> _addedTicks;
        private List<ITickFixed> _addedFixedTicks;
        private List<ITickLater> _addedLaterTicks;

        private float _oneSecondTimer;
        private bool _isRun;

        public event Action<bool> OnFocusApp;

        public static Updater Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                _oneSecondTimer = 1f;
                _isRun = true;
            }
            else
                Destroy(gameObject);
        }

        #region Public Methods

        public void Pause()
        {
            _isRun = false;
        }

        public void Resume()
        {
            _isRun = true;
        }

        public void Add(ITick tick)
        {
            if (tick != null)
            {
                if (_ticks == null)
                {
                    _ticks = new List<ITick>();
                }

                if (_removedTicks != null)
                {
                    _removedTicks.Remove(tick);
                }

                if (_addedTicks == null)
                {
                    _addedTicks = new List<ITick>();
                }

                if (!_addedTicks.Contains(tick))
                {
                    _addedTicks.Add(tick);
                }
            }
        }

        public void Add(ITickFixed tick)
        {
            if (_fixedTicks == null)
            {
                _fixedTicks = new List<ITickFixed>();
            }

            if (_removedFixedTicks != null)
            {
                _removedFixedTicks.Remove(tick);
            }

            if (!_fixedTicks.Contains(tick))
            {
                // @TODO need refactor
                _fixedTicks.Add(tick);
            }
        }

        public void Add(ITickLater tick)
        {
            if (_laterTicks == null)
            {
                _laterTicks = new List<ITickLater>();
            }

            if (_removedLaterTicks != null)
            {
                _removedLaterTicks.Remove(tick);
            }

            if (!_laterTicks.Contains(tick))
            {
                // @TODO need refactor
                _laterTicks.Add(tick);
            }
        }

        public void Remove(ITick tick)
        {
            if (_removedTicks == null)
            {
                _removedTicks = new List<ITick>();
            }

            _removedTicks.Add(tick);
        }

        public void Remove(ITickFixed tick)
        {
            if (_removedFixedTicks == null)
            {
                _removedFixedTicks = new List<ITickFixed>();
            }

            _removedFixedTicks.Add(tick);
        }

        public void Remove(ITickLater tick)
        {
            if (_removedLaterTicks == null)
            {
                _removedLaterTicks = new List<ITickLater>();
            }

            _removedLaterTicks.Add(tick);
        }

        public void AddDelayedCall(float delay, Action action, bool isUnscaled = false)
        {
            if (_delayedCalls == null)
            {
                _delayedCalls = new List<CallDelay>();
                _removedCalls = new List<CallDelay>();
            }

            if (delay <= 0)
            {
                action?.Invoke();
            }
            else
            {
                _delayedCalls.Add(new CallDelay(action, delay, isUnscaled));
            }
        }

        public void RemoveDelayedCall(Action action)
        {
            if (_delayedCalls != null)
            {
                foreach (var call in _delayedCalls)
                {
                    if (call.Action == action)
                    {
                        _delayedCalls.Remove(call);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void Update()
        {
            if (!_isRun)
                return;

            if (_ticks != null)
            {
                if (_addedTicks != null)
                {
                    foreach (var tick in _addedTicks)
                    {
                        _ticks.Add(tick);
                    }

                    _addedTicks.Clear();
                }

                if (_removedTicks != null)
                {
                    foreach (var tick in _removedTicks)
                    {
                        _ticks.Remove(tick);
                    }

                    _removedTicks.Clear();
                }

                foreach (var tick in _ticks)
                {
                    tick.Tick();
                }

                _oneSecondTimer -= Time.unscaledDeltaTime;
                if (_oneSecondTimer <= 0)
                {
                    _oneSecondTimer = 1f;

                    foreach (var tick in _ticks)
                        tick.TickSecond();
                }
            }

            if (_delayedCalls != null && _removedCalls != null)
            {
                foreach (var call in _delayedCalls)
                {
                    if (call.IsUnscaled)
                        call.Delay -= Time.unscaledDeltaTime;
                    else
                        call.Delay -= Time.deltaTime;

                    if (call.Delay <= 0)
                    {
                        _removedCalls.Add(call);
                    }
                }

                foreach (var call in _removedCalls)
                {
                    _delayedCalls.Remove(call);
                    call.Action?.Invoke();
                }

                _removedCalls.Clear();
            }
        }

        private void LateUpdate()
        {
            if (!_isRun)
                return;

            if (_laterTicks != null)
            {
                if (_removedLaterTicks != null)
                {
                    foreach (var tick in _removedLaterTicks)
                    {
                        _laterTicks.Remove(tick);
                    }

                    _removedLaterTicks.Clear();
                }

                foreach (var tick in _laterTicks)
                {
                    tick.TickLater();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_isRun)
                return;

            if (_fixedTicks != null)
            {
                if (_removedFixedTicks != null)
                {
                    foreach (var tick in _removedFixedTicks)
                    {
                        _fixedTicks.Remove(tick);
                    }

                    _removedFixedTicks.Clear();
                }

                foreach (var tick in _fixedTicks)
                {
                    tick.TickFixed();
                }
            }
        }

        #endregion

        private void OnApplicationFocus(bool hasFocus)
        {
            OnFocusApp?.Invoke(hasFocus);
        }
    }
}