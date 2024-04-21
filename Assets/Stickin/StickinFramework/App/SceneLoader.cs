using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace stickin
{
	public class SceneLoader : MonoBehaviour
	{

		#region Constants

		private const string SCENE_LOADER_NAME = "Loader";

		#endregion

		#region Serialized Fields

		[SerializeField] private Image _fadeCanvasGroup; // @TODO need change to CanvasGroup

		[Header("Timing Settings")] [SerializeField]
		private float _waitOnLoadEnd = 0.2f;

		[SerializeField] private float _fadeDuration = 0.2f;

		[Header("Loading Settings")] [SerializeField]
		private ThreadPriority _loadThreadPriority;

		#endregion

		public static Color FadeColor = Color.black;

		#region Private Properties

		private float lastProgress;
		private AsyncOperation operation;

		private static string sceneToLoad1;
		private static string sceneToLoad2;
		private static Hashtable _data;

		#endregion

		#region Public Methods

		public static void LoadScene(int indexScene, Hashtable data = null)
		{
			var name = StringExtensions.GetSceneName(indexScene);
			LoadScene(name, data);
		}
		
		public static void LoadScene(string loadScene, Hashtable data = null)
		{
			LoadScenes(loadScene, null, data);
		}

		public static void LoadScenes(string loadScene1, string loadScene2, Hashtable data = null)
		{
			// Time.timeScale = 1f;
			
			Application.backgroundLoadingPriority = ThreadPriority.High;
			_data = data;
			sceneToLoad1 = loadScene1;
			sceneToLoad2 = loadScene2;
			SceneManager.LoadScene(SCENE_LOADER_NAME, LoadSceneMode.Additive);
		}

		#endregion

		#region Private Methods

		private void Start()
		{
			_fadeCanvasGroup.canvasRenderer.SetAlpha(0);
			_fadeCanvasGroup.color = FadeColor;

			// yield return LoadAsync(sceneToLoad1);
			StartCoroutine(LoadAsync(sceneToLoad1));
		}

		private IEnumerator LoadAsync(string loadScene)
		{
			// Debug.LogError("SceneLoader.LoadAsync = " + loadScene);
			yield return null;

			FadeOut();
			yield return new WaitForSeconds(_fadeDuration);

			RemoveUnusedScenes();

			var startLoadTime = Time.realtimeSinceStartup;
			StartOperation(loadScene);

			while (operation.isDone == false)
			{
				yield return null;

				if (Mathf.Approximately(operation.progress, lastProgress) == false)
				{
					lastProgress = operation.progress;
				}
			}

			var endLoadTime = Time.realtimeSinceStartup;
			Debug.Log($"time to load scene '{loadScene}' = {endLoadTime - startLoadTime}");

			Camera.main.enabled = false;
			var scene = SceneManager.GetSceneByName(sceneToLoad1);
			SceneManager.SetActiveScene(scene);
			SetSceneData(scene);
			yield return new WaitForSeconds(_waitOnLoadEnd);

			FadeIn();
			yield return new WaitForSeconds(_fadeDuration);

			SceneManager.UnloadSceneAsync(SCENE_LOADER_NAME);

		}

		private void StartOperation(string loadScene)
		{
			Application.backgroundLoadingPriority = _loadThreadPriority;
			operation = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

			if (!string.IsNullOrEmpty(sceneToLoad2))
			{
				SceneManager.LoadScene(sceneToLoad2, LoadSceneMode.Additive);
			}
		}

		private void FadeIn()
		{
			_fadeCanvasGroup.CrossFadeAlpha(0, _fadeDuration, true);
		}

		private void FadeOut()
		{
			_fadeCanvasGroup.CrossFadeAlpha(1f, _fadeDuration, true);
		}

		private void RemoveUnusedScenes()
		{
			var removeScenesNames = new List<string>();

			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				if (SceneManager.GetSceneAt(i).name.CompareTo(SCENE_LOADER_NAME) != 0)
					removeScenesNames.Add(SceneManager.GetSceneAt(i).name);
			}

			for (var i = 0; i < removeScenesNames.Count; i++)
			{
				SceneManager.UnloadSceneAsync(removeScenesNames[i]);
			}
		}

		private void SetSceneData(Scene scene)
		{
			if (_data != null && scene != null)
			{
				var objects = scene.GetRootGameObjects();
				foreach (var obj in objects)
				{
					var sceneParams = obj.GetComponentsInChildren<SceneParams>();
					if (sceneParams != null)
					{
						foreach (var sc in sceneParams)
						{
							sc.SetData(_data);
						}
					}
				}

				_data = null;
			}
		}

		#endregion
	}
}