using System.Collections;
using UnityEngine;

namespace stickin.mathcross
{
    public class LevelsGeneratorTest : MonoBehaviour
    {
        [SerializeField] private GameView _gameViewPrefab;
        [SerializeField] private LevelDifficult _levelDifficult = LevelDifficult.Expert;

        private GameView _currentGameView;
        
        private void Start()
        {
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            if (_currentGameView != null)
            {
                Destroy(_currentGameView.gameObject);
                _currentGameView = null;
            }

            var gameParams = new GameParams(1);
            gameParams.CustomData = new Hashtable
            {
                ["difficult"] = _levelDifficult,
                ["allCellsToBoard"] = true
            };
            
            _currentGameView = Instantiate(_gameViewPrefab);
            yield return new WaitForSeconds(0.02f);
            _currentGameView.InitWithLevelNumber(gameParams);

            var canvas = _currentGameView.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                StartCoroutine(Generate());
        }
    }
}