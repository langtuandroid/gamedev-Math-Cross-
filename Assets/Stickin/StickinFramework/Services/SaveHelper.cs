using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace stickin
{
    public static class SaveHelper
    {
        #region Public Methods

        public static string LoadText(string filename)
        {
            var path = GetPath(filename);

            if (File.Exists(path))
            {
                var result = File.ReadAllText(path);
                return result;
            }

            return default;
        }

        public static T Load<T>(string filename)
        {
            var path = GetPath(filename);

            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);

                var result = (T) bf.Deserialize(file);
                file.Close();

                return result;
            }

            return default;
        }

        public static void Save<T>(T data, string filename)
        {
            var path = GetPath(filename);

            try
            {
                CreateDirectoryRecursive(path, true);

                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(path);
                bf.Serialize(file, data);

                file.Close();
            }
            catch (Exception e)
            {
            }
        }

        public static void SaveJson<T>(T data, string filename, bool removeSpaces = false, bool needAddedPath = true)
        {
            SaveText(JsonUtility.ToJson(data, true), filename, needAddedPath, removeSpaces);
        }

        public static void SaveText(string txt, string filename, bool needAddedPath = true, bool removeSpaces = false)
        {
            if (removeSpaces)
            {
                txt = txt.Replace(" ", string.Empty);
                txt = txt.Replace("\n", string.Empty);
            }

            SaveText(txt, filename, Encoding.UTF8, needAddedPath);
        }

        public static void SaveText(string txt, string filename, System.Text.Encoding encoding,
            bool needAddedPath = true)
        {
            var path = needAddedPath ? GetPath(filename) : filename;

            CreateDirectoryRecursive(path, true);

            File.WriteAllText(path, txt, encoding);
        }

        public static T LoadJson<T>(string filename, bool needAddedPath = true)
        {
            var path = needAddedPath ? GetPath(filename) : filename;

            if (File.Exists(path))
            {
                var txt = File.ReadAllText(path);
                return JsonUtility.FromJson<T>(txt);
            }

            return default;
        }

        public static T LoadJsonResource<T>(string filename)
        {
            var textAsset = Resources.Load<TextAsset>(filename);

            if (textAsset != null)
                return JsonUtility.FromJson<T>(textAsset.text);

            Debug.LogError($"Not find resource file = {filename}");
            return default;
        }

        public static void Save(Texture2D texture, string filename, bool isPng, bool needCorrectPath)
        {
            var path = needCorrectPath ? GetPath(filename) : filename;

            CreateDirectoryRecursive(path, true);

            File.WriteAllBytes(path, isPng ? texture.EncodeToPNG() : texture.EncodeToJPG());
        }

        public static void SaveTexture(string filename, Texture2D texture2D)
        {
            var path = GetPath(filename);
            CreateDirectoryRecursive(path, true);
            File.WriteAllBytes(path, texture2D.EncodeToPNG());
        }

        public static Texture2D LoadTexture(string filename)
        {
            var path = GetPath(filename);

            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var result = new Texture2D(1, 1);
                result.LoadImage(bytes);

                return result;
            }

            return null;
        }

        public static void RemoveDirectory(string folder)
        {
            var path = GetPath(folder);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static void RemoveFile(string filename)
        {
            var path = GetPath(filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool IsExists(string filename)
        {
            var path = GetPath(filename);
            return File.Exists(path);
        }

        public static T LoadResourceJson<T>(string filename)
        {
            var res = Resources.Load<TextAsset>(filename);
            if (res != null)
                return JsonUtility.FromJson<T>(res.text);

            return default;
        }

        public static List<string> GetFilenamesInFolder(string folder)
        {
            var result = new List<string>();

            var path = GetPath(folder);
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var newFilename = file.Replace($"{path}/", "");
                    result.Add(newFilename);
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        public static string GetPath(string filename)
        {
            return $"{Application.persistentDataPath}/{Application.identifier}/{filename}";
        }

        public static void CreateDirectoryRecursive(string path, bool needRemoveFilename)
        {
            try
            {
                string[] pathParts = path.Split('/');

                var result = "";
                for (int i = 0; i < pathParts.Length; i++)
                {
                    if (needRemoveFilename && i == pathParts.Length - 1)
                        break;

                    if (i > 0)
                        result += "/" + pathParts[i];

                    if (!string.IsNullOrEmpty(result) && !Directory.Exists(result))
                        Directory.CreateDirectory(result);
                }
            }
            catch (Exception e)
            {
            }
        }

        public static string CombinePath(params string[] arr)
        {
            var result = "";
            for (var i = 0; i < arr.Length; i++)
            {
                var ar = arr[i];
                result += ar;

                if (i < arr.Length - 1 && ar.Length > 0 && ar[ar.Length - 1] != '/')
                    result += "/";
            }

            return result;
        }

        #endregion
    }
}