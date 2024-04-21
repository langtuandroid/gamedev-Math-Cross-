using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace stickin
{
    [CustomEditor(typeof(AppData))]
    public class AppDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var appData = target as AppData;

            if (GUILayout.Button("Set games scenes in Build Settings"))
            {
                SetScenesInSettings(appData);
                
                var scenePath = EditorBuildSettings.scenes[0].path;
                EditorSceneManager.OpenScene(scenePath);
            }
        }

        private void SetScenesInSettings(AppData appData)
        {
            var scenesPaths = GetScenes(appData);

            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            foreach (var path in scenesPaths)
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            Debug.Log("Set games scenes in Build Settings - SUCCESS");
        }
        
        public static string[] GetScenes(AppData appData)
        {
            var result = new string[appData.ScenesMobile.Length];

            Debug.Log("__________________________________________");
            Debug.Log("SCENES:");
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = AssetDatabase.GetAssetPath(appData.ScenesMobile[i]);
                Debug.Log(result[i]);
            }

            Debug.Log("__________________________________________");

            return result;
        }
    }
}