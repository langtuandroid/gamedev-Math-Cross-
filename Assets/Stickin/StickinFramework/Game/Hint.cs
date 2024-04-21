using System;
using stickin;
using UnityEngine;

namespace stickin
{
    public class GameTimer : ITick, IGameModule
    {
        private TimerType _timerType;
        private TimerUpdateType _timerUpdateType;
        private float _seconds;
        private bool _isPause;
        private bool _isStarted;

        public int Seconds => (int) _seconds;

        private event Action<float> _changeCallbacks;
        
        public GameTimer(float seconds, TimerType timerType, TimerUpdateType timerUpdateType, Action<float> changeCallback)
        {
            _seconds = seconds;
            _timerType = timerType;
            _timerUpdateType = timerUpdateType;
            AddedCallback(changeCallback);

            Start();
        }

        public void AddedCallback(Action<float> callback)
        {
            _changeCallbacks += callback;
            _changeCallbacks?.Invoke(_seconds);
        }

        public void RemoveCallback(Action<float> callback)
        {
            _changeCallbacks -= callback;
        }

        public void Start()
        {
            if (_isStarted)
                return;

            _isStarted = true;
            
            if (Updater.Instance != null)
                Updater.Instance.Add(this);
            
            Resume();
        }
        
        public void Stop()
        {
            Pause();
            
            if (_isStarted)
                Updater.Instance.Remove(this);

            _isStarted = false;
        }
        
        public void Resume()
        {
            _isPause = false;
            // Debug.LogError("GameTimer Resume");
        }
        
        public void Pause()
        {
            _isPause = true;
            // Debug.LogError("GameTimer Pause");
        }
        
        private void OnChangeTimer()
        {
            if (_isPause)
                return;

            var change = _timerUpdateType == TimerUpdateType.Seconds ? 1 : Time.deltaTime;
            
            if (_timerType == TimerType.Increase)
                _seconds += change;
            else if (_timerType == TimerType.Decrease)
            {
                _seconds = Math.Max(_seconds - change, 0);

                if (_seconds <= 0)
                    Stop();
            }

            _changeCallbacks?.Invoke(_seconds);
        }

        public void Tick()
        {
            if (_timerUpdateType == TimerUpdateType.Milliseconds)
                OnChangeTimer();
        }

        public void TickSecond()
        {
            if (_timerUpdateType == TimerUpdateType.Seconds)
                OnChangeTimer();
        }

        public void SetSeconds(float seconds)
        {
            _seconds = seconds;
            _changeCallbacks?.Invoke(_seconds);
        }
        
        public void AddedSeconds(float addedSeconds)
        {
            _seconds += addedSeconds;
            _changeCallbacks?.Invoke(_seconds);
        }

        public void Destroy()
        {
            Stop();
        }
    }
    
    public abstract class Hint
    {
        public HintSO HintSo { get; private set; }

        public Hint SetConfig(HintSO hintSo)
        {
            HintSo = hintSo;
            return this;
        }
        
        public abstract bool Run(Game g);

        public virtual bool CanUse(Game g)
        {
            return true;
        }

        public virtual void Destroy()
        {
            
        }
    }
}