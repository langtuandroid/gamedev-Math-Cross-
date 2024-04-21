using stickin.menus;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using stickin;

namespace stickin
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(SceneParams))]
    public class GameView : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] protected GameConfig _gameConfig;
        [SerializeField] private BaseMenu _gameMenu;
        
        protected Game _game;
        protected GameParams _gameParams;

        private CanvasGroup _canvasGroup;
        private TouchMonoBehaviour[] _touchedElements;

        public Game Game => _game;
        public GameParams GameParams => _gameParams;
        public GameStateType GameState { get; private set; } = GameStateType.Play;
        public bool IsInitComplete { get; private set; }
        public virtual RectTransform BoardRt => null;
        public OrderAssetType OrderType => _gameParams.OrderType;
        
        public event Action OnInitComplete;
        
        #region Game
        public virtual void InitWithLevelNumber(GameParams gameParams)
        {
        }
        
        private void InitGameStep2()
        {
            _game.OnEndGame += OnEndGame;
            _game.OnLockedTouch += OnLockedTouch;
            
            InitHints();

            if (_gameMenu != null)
            {
                MenusService.OnShowMenuRegistr(OnShowMenu);
                MenusService.OnHideMenuRegistr(OnHideMenu);
            }
        }

        protected virtual void Awake()
        {
            var sceneParams = GetComponent<SceneParams>();
            sceneParams.OnSetData += OnSetData;

            _canvasGroup = GetComponent<CanvasGroup>();
            _touchedElements = GetComponentsInChildren<TouchMonoBehaviour>();
            
            InjectService.Bind<GameView>(this);
            InjectService.BindFields(this);
        }
        
        protected virtual void OnDestroy()
        {
            if (_game != null)
            {
                _game.OnEndGame -= OnEndGame;
                _game.OnLockedTouch -= OnLockedTouch;
                
                _game.Destroy();
            }

            if (_gameMenu != null)
            {
                MenusService.OnShowMenuUnregistr(OnShowMenu);
                MenusService.OnHideMenuUnregistr(OnHideMenu);
            }
        }

        private void InitHints()
        {
            var hintsView = GetComponentInChildren<HintsView>();
            if (hintsView != null)
                hintsView.Init(_game, _gameConfig.Hints);

            var oneHints = GetComponentsInChildren<OneHintView>();
            if (oneHints != null)
            {
                foreach (var oneHint in oneHints)
                    oneHint.Init(_game);
            }
        }
        
        private void OnEndGame(GameStateType gameState, GameEndReasonType gameEndReason)
        {
            EndGame(gameState, gameEndReason);
        }

        private void OnLockedTouch(bool locked)
        {
            _canvasGroup.interactable = !locked;

            // if (_touchedElements != null)
            // {
            //     foreach (var touchedElement in _touchedElements)
            //     {
            //         if (touchedElement != null)
            //             touchedElement.SetInteractable(!locked);
            //     }
            // }
        }
        #endregion

        private void OnSetData(Hashtable data)
        {
            GameParams gameParams = (GameParams) data["gameParams"];
            _gameParams = gameParams;
            
            InitWithLevelNumber(gameParams);
            InitGameStep2();
            InitComplete();
        }

        protected void InitComplete()
        {
            IsInitComplete = true;
            OnInitComplete?.Invoke();
        }

        #region Interfaces
        public event Action<GameStateType, GameEndReasonType> OnChangeGameState;

        public void EndGame(GameStateType gameState, GameEndReasonType gameEndReason = GameEndReasonType.Unknown)
        {
            GameState = gameState;
            OnChangeGameState?.Invoke(gameState, gameEndReason);
        }
        #endregion
        
        #region Resume / Pause
        private void OnShowMenu(BaseMenu menu)
        {
            if (_gameMenu != null && _gameMenu.GetType() == menu.GetType())
                _game.Resume();
        }
        private void OnHideMenu(BaseMenu menu)
        {
            if (_gameMenu != null && _gameMenu.GetType() == menu.GetType())
                _game.Pause();
        }
        #endregion
        
#if UNITY_EDITOR
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                _game.SimulateEndGame();
        }
#endif
    }
}