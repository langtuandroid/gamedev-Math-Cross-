using System.Collections.Generic;
using UnityEngine;

namespace stickin
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static void Remove<T>(this List<T> ts, List<T> removed)
        {
            foreach (var r in removed)
            {
                ts.Remove(r);
            }
        }

        public static T GetRandom<T>(this List<T> ts)
        {
            return ts.Count > 0 ? ts[Random.Range(0, ts.Count)] : default;
        }

        public static List<T> GetRandom<T>(this List<T> ts, int count)
        {
            var result = new List<T>();

            var indexes = new List<int>();
            for (var i = 0; i < ts.Count; i++)
                indexes.Add(i);

            indexes.Shuffle();
            foreach (var index in indexes)
            {
                result.Add(ts[index]);

                if (result.Count == count)
                    break;
            }

            return result;
        }

        public static int GetRandomWeightsIndex(this List<int> ts)
        {
            int sumWeights = 0;
            foreach (int t in ts)
                sumWeights += t;

            var rndWeight = Random.Range(0, sumWeights) + 1;
            sumWeights = 0;

            for (int i = 0; i < ts.Count; i++)
            {
                sumWeights += ts[i];

                if (rndWeight <= sumWeights)
                {
                    return i;
                }
            }

            return 0;
        }

        public static void Add<T>(this List<T> ts, List<T> list, bool addedDuplicate = true)
        {
            foreach (var el in list)
            {
                if (addedDuplicate || ts.Contains(el) == false)
                    ts.Add(el);
            }
        }

        public static string ToText<T>(this List<T> ts)
        {
            var result = string.Empty;

            for (var i = 0; i < ts.Count; i++)
            {
                result += ts[i].ToString();
                if (i < ts.Count - 1)
                    result += ",";
            }

            return result;
        }

        public static T GetElement<T>(this List<T> ts, int index)
        {
            if (ts.Count > 0)
                return ts[index % ts.Count];

            return default;
        }

        public static T Last<T>(this List<T> ts)
        {
            return ts.Count > 0 ? ts[ts.Count - 1] : default;
        }
    }
}