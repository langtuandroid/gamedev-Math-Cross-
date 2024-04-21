using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace stickin
{
    public abstract class Game
    {
        protected List<Hint> _hints = null;
        protected bool _isLocked;
        protected Hint _currentHint;
        protected List<IGameModule> _gameModules;
        protected ISavable _savable;

        public GameStateType GameState = GameStateType.Play;
        
        public event Action<GameStateType, GameEndReasonType> OnEndGame;
        public event Action<bool> OnLockedTouch;
        public event Action<Hint> OnSelectHint;
        public event Action<Hint> OnUseHint;

        public event Action OnPause;
        public event Action OnResume;
        
        public Game()
        {
            _gameModules = new List<IGameModule>();
            InitHints();
        }

        public void RegistrGameModule(IGameModule module)
        {
            _gameModules.Add(module);
        }

        public T GetGameModule<T>() where T : IGameModule
        {
            foreach (var gameModule in _gameModules)
            {
                if (gameModule.GetType() == typeof(T))
                    return (T)gameModule;
            }

            return default;
        }
        
        protected abstract void InitHints();

        public virtual void SimulateEndGame()
        {
            
        }

        public void RebornGame()
        {
            GameState = GameStateType.Play;
        }
        
        protected void EndGame(GameStateType gameState, GameEndReasonType reason = GameEndReasonType.Unknown)
        {
            GameState = gameState;

            foreach (var gameModule in _gameModules)
                gameModule.Stop();
            
            OnEndGame?.Invoke(gameState, reason);
        }

        protected async Task LockedTouch(bool locked)
        {
            _isLocked = locked;
            OnLockedTouch?.Invoke(locked);
        }

        protected async Task Delay(float duration)
        {
            await Task.Delay(DurationToMilliseconds(duration));
        }
        
        public static int DurationToMilliseconds(float duration) => (int) (duration * 1000);

        public bool CanUseHint(string logicClass)
        {
            // if (!_isLocked)
            {
                var hint = GetHint(logicClass);
                if (!_isLocked && hint != null)
                    return hint.CanUse(this);
            }

            return false;
        }
        
        public bool UseHint(string logicClass)
        {
            // if (!_isLocked)
            {
                var hint = GetHint(logicClass);
                if (hint != null)
                    return hint.Run(this);
            }

            return false;
        }

        protected Hint GetHint(string logicClass)
        {
            if (string.IsNullOrEmpty(logicClass))
                return null;
            
            if (_hints != null)
            {
                foreach (var hint in _hints)
                {
                    if (hint.GetType().Name == logicClass)
                    {
                        if (GameState != GameStateType.Lose)
                            return hint;
                    }
                }
                
                Debug.LogError($"Error use hint. Not find hint class = {logicClass}");
            }
            else
            {
                Debug.LogError("Error use hint. Hints is null");
            }
            
            return null;
        }

        public virtual void SetCurrentHint(string logicClass)
        {
            _currentHint = GetHint(logicClass);
            OnSelectHint?.Invoke(_currentHint);
        }

        public void Pause()
        {
            foreach (var module in _gameModules)
                module.Pause();
            
            OnPause?.Invoke();
        }

        public void Resume()
        {
            foreach (var module in _gameModules)
                module.Resume();
            
            OnResume?.Invoke();
        }

        protected void UseHintComplete(string hintClassName)
        {
            var hint = GetHint(hintClassName);
            if (hint != null)
                UseHintComplete(hint);
        }

        protected void UseHintComplete(Hint hint)
        {
            OnUseHint?.Invoke(hint);
        }

        public virtual void Destroy()
        {
            foreach (var module in _gameModules)
                module.Destroy();

            if (_hints != null)
            {
                foreach (var hint in _hints)
                    hint.Destroy();
            }
        }

        protected void Save() => _savable?.Save();
    }
}