using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace stickin
{
    public static class StringExtensions
    {
        public static string MinutesToText(int minutes)
        {
            var result = "";

            var mins = minutes % 60;
            var hours = minutes / 60 % 24;
            var days = minutes / 60 / 24;

            if (days > 0)
                result += days + "d ";

            if (hours > 0 || result.Length > 0)
                result += hours + "h ";

            if (mins > 0 || result.Length > 0)
                result += mins + "m";

            return result;
        }

        public static string SecondsToText(int seconds, bool withZero)
        {
            var minutes = seconds / 60;

            if (withZero)
                return string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);

            return string.Format(minutes >= 10 ? "{0:D2}:{1:D2}" : "{0:D1}:{1:D2}", seconds / 60, seconds % 60);
        }

        // StringExtensions.FormatCount(_data.Count, "сканворд", "сканворда", "сканвордов");
        public static string FormatCount(int count, string var1, string var234, string var056789)
        {
            var result = "";

            var ostatok = count % 100;
            if (ostatok >= 10 && ostatok <= 19)
            {
                result = var056789;
            }
            else
            {
                ostatok = count % 10;
                switch (ostatok)
                {
                    case 0:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        result = var056789;
                        break;

                    case 1:
                        result = var1;
                        break;

                    case 2:
                    case 3:
                    case 4:
                        result = var234;
                        break;
                }
            }

            return result;
        }

        #region For scenes

        public static string GetSceneName(int index)
        {
            var name = SceneUtility.GetScenePathByBuildIndex(index);
            var i1 = name.LastIndexOf("/");
            var i2 = name.LastIndexOf(".");

            name = name.Substring(i1 + 1, i2 - i1 - 1);

            return name;
        }

        #endregion

        #region For parsers

        public static string RemoveInBegin(this string ts, char ch)
        {
            while (ts.Length > 0 && ts[0] == ch)
                ts = ts.Substring(1);

            return ts;
        }

        public static char GetRandomChar(this string ts)
        {
            if (string.IsNullOrEmpty(ts) == false)
                return ts[Random.Range(0, ts.Length)];

            return '?';
        }

        public static string RemoveEndAfterSubstring(this string ts, string sub)
        {
            var index = ts.LastIndexOf(sub);
            if (index >= 0 && index < ts.Length)
                return ts.Substring(0, index);

            return ts;
        }

        public static string RemoveInEnd(this string ts, char ch)
        {
            while (ts.Length > 0 && ts[ts.Length - 1] == ch)
                ts = ts.Substring(0, ts.Length - 1);

            return ts;
        }

        public static string RemoveSubString(this string ts, string removeStr)
        {
            var index = ts.IndexOf(removeStr);
            if (index >= 0 && index < ts.Length)
                return ts.Remove(index, removeStr.Length);

            return ts;
        }

        public static string GetSubString(this string ts, string begin, string end)
        {
            var beginIndex = ts.IndexOf(begin) + begin.Length;
            var endIndex = ts.IndexOf(end, beginIndex);

            if (beginIndex >= begin.Length && endIndex >= 0 && endIndex > beginIndex && beginIndex < ts.Length &&
                endIndex < ts.Length)
            {
                return ts.Substring(beginIndex, endIndex - beginIndex);
            }

            return "";
        }

        public static string GetSubStringFromEnd(this string ts, string begin, string end)
        {
            var endIndex = ts.LastIndexOf(end);
            if (endIndex > 0 && endIndex < ts.Length)
            {
                var beginIndex = ts.LastIndexOf(begin, endIndex) + begin.Length;

                if (beginIndex >= 0 && endIndex >= 0 && endIndex > beginIndex && beginIndex < ts.Length &&
                    endIndex < ts.Length)
                {
                    return ts.Substring(beginIndex, endIndex - beginIndex);
                }
            }

            return "";
        }

        public static string RemoveTagHTML(this string ts, string tag, bool withMid = false, bool once = false)
        {
            var maxCount = 50;
            while (ts.Contains("<" + tag) && maxCount > 0)
            {
                if (withMid)
                {
                    ts = ts.RemoveTagStepHTML("<" + tag, "</" + tag + ">", withMid);
                }
                else
                {
                    ts = ts.RemoveTagStepHTML("<" + tag, ">", withMid);
                    ts = ts.RemoveTagStepHTML("</" + tag, ">", withMid);
                }

                maxCount--;
                if (once)
                    break;
            }

            return ts;
        }

        private static string RemoveTagStepHTML(this string ts, string beginTag, string endTag, bool withMid = false)
        {
            var beginIndex = ts.IndexOf(beginTag);
            if (beginIndex >= 0)
            {
                var endIndex = ts.IndexOf(endTag, beginIndex) + endTag.Length;

                if (endIndex >= 0 && endIndex > beginIndex && beginIndex < ts.Length &&
                    endIndex <= ts.Length)
                {
                    return ts.Remove(beginIndex, endIndex - beginIndex);
                }
            }

            return ts;
        }

        #endregion

        public static string GetSubString(this string ts, string begin, string end, int startIndex, ref int resultIndex)
        {
            var beginIndex = ts.IndexOf(begin, startIndex) + begin.Length;
            var endIndex = ts.IndexOf(end, beginIndex);
            // Debug.LogError($"beginIndex = {beginIndex}      endIndex = {endIndex}");

            if (beginIndex >= begin.Length && endIndex >= 0 && endIndex > beginIndex && beginIndex < ts.Length &&
                endIndex < ts.Length)
            {
                resultIndex = beginIndex; //beginIndex;
                return ts.Substring(beginIndex, endIndex - beginIndex);
            }

            return "";
        }

        public static string Shuffle(this string str)
        {
            var list = str.ToList();
            list.Shuffle();

            var result = string.Empty;
            foreach (var l in list)
                result += l;

            return result;
        }

        public static bool IsEmpty(this string str)
        {
            return str.Length <= 0;
        }

        public static string ToUpperFirst(this string str)
        {
            if (str != null && str.Length > 0)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str;
        }
    }
}