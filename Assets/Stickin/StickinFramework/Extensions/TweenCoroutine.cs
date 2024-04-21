using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace stickin
{
    public static class TweenCoroutine
    {
        public static IEnumerator SizeTo(RectTransform rt, Vector2 toSize, float duration, Action callbackEnd = null,
            float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float currentTime = 0.0f;
            var fromSize = rt.sizeDelta;

            do
            {
                rt.sizeDelta = Vector2.Lerp(fromSize, toSize, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            rt.sizeDelta = toSize;
            callbackEnd?.Invoke();

            yield return null;
        }

        public static IEnumerator MoveTo(RectTransform rt, Vector2 toPos, float duration, Action callbackEnd = null,
            float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float currentTime = 0.0f;
            var fromPos = rt.anchoredPosition;

            do
            {
                rt.anchoredPosition = Vector2.Lerp(fromPos, toPos, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            rt.anchoredPosition = toPos;
            callbackEnd?.Invoke();

            yield return null;
        }

        public static IEnumerator MoveTo(Transform tr, Vector3 toPos, float duration, Action callbackEnd = null,
            float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float currentTime = 0.0f;
            var fromPos = tr.position;

            do
            {
                tr.position = Vector3.Lerp(fromPos, toPos, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            tr.position = toPos;
            callbackEnd?.Invoke();

            yield return null;
        }

        public static IEnumerator LocalScaleTo(Transform tr, Vector3 toScale, float duration, float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            var currentTime = 0.0f;
            var fromScale = tr.localScale;

            do
            {
                tr.localScale = Vector3.Lerp(fromScale, toScale, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            tr.localScale = toScale;

            yield return null;
        }

        public static IEnumerator JumpTo(RectTransform rt, float height, float duration, float delay = 0)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float currentTime = 0.0f;
            var startPos = rt.anchoredPosition;
            var fromPos = new Vector2(0, 0);
            var toPos = new Vector2(0, height);

            do
            {
                var angle = currentTime / duration * Mathf.PI;
                rt.anchoredPosition =
                    startPos + Vector2.Lerp(fromPos, toPos, currentTime / duration) * Mathf.Sin(angle);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            rt.anchoredPosition = startPos;

            yield return null;
        }

        public static IEnumerator FillAmount(Image img, float toValue, float duration, float delay = 0,
            Action callback = null)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            float currentTime = 0.0f;
            var fromValue = img.fillAmount;

            do
            {
                img.fillAmount = Mathf.Lerp(fromValue, toValue, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            img.fillAmount = toValue;
            callback?.Invoke();

            yield return null;
        }


        public static IEnumerator ToValue(Action<float> callback, float fromValue, float toValue, float duration,
            float delayBegin, float delayEnd, Action callbackEnd, bool unscaled)
        {
            if (delayBegin > 0)
                yield return new WaitForSeconds(delayBegin);

            var currentTime = 0.0f;
            var value = fromValue;

            do
            {
                value = Mathf.Lerp(fromValue, toValue, currentTime / duration);

                if (unscaled)
                    currentTime += Time.unscaledDeltaTime;
                else
                    currentTime += Time.deltaTime;

                callback?.Invoke(value);
                yield return null;
            } while (currentTime <= duration);

            value = toValue;
            callback?.Invoke(value);

            if (delayEnd > 0)
                yield return new WaitForSeconds(delayEnd);

            callbackEnd?.Invoke();

            yield return null;
        }

        public static IEnumerator ToColor(RawImage image, Color toColor, float duration)
        {
            var currentTime = 0.0f;
            var fromColor = image.color;

            do
            {
                image.color = Color.Lerp(fromColor, toColor, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= duration);

            image.color = toColor;

            yield return null;
        }

        public static IEnumerator ToColor(MeshRenderer meshRenderer, Color toColor, float duration,
            bool withReverse = false, bool loop = false)
        {
            if (withReverse)
                duration /= 2;

            var fromColors = new List<Color>();
            for (var i = 0; i < meshRenderer.materials.Length; i++)
                fromColors.Add(meshRenderer.materials[i].color);

            do
            {
                var currentTime = 0.0f;

                do
                {
                    for (var i = 0; i < meshRenderer.materials.Length; i++)
                        meshRenderer.materials[i].color = Color.Lerp(fromColors[i], toColor, currentTime / duration);

                    currentTime += Time.deltaTime;
                    yield return null;
                } while (currentTime <= duration);

                for (var i = 0; i < meshRenderer.materials.Length; i++)
                    meshRenderer.materials[i].color = toColor;

                if (withReverse)
                {
                    currentTime = 0;

                    do
                    {
                        for (var i = 0; i < meshRenderer.materials.Length; i++)
                            meshRenderer.materials[i].color =
                                Color.Lerp(toColor, fromColors[i], currentTime / duration);

                        currentTime += Time.deltaTime;
                        yield return null;
                    } while (currentTime <= duration);

                    for (var i = 0; i < meshRenderer.materials.Length; i++)
                        meshRenderer.materials[i].color = fromColors[i];
                }

                yield return null;
            } while (loop);

            yield return null;
        }

        public static IEnumerator ToColor(MeshRenderer meshRenderer, Color toColor, float duration, int shakeCount)
        {
            var currentTime = 0.0f;
            duration = duration / shakeCount / 2;
            var fromColor = meshRenderer.material.color;

            for (var i = 0; i < shakeCount; i++)
            {
                currentTime = 0;
                do
                {
                    meshRenderer.material.color = Color.Lerp(fromColor, toColor, currentTime / duration);
                    currentTime += Time.deltaTime;
                    yield return null;
                } while (currentTime <= duration);

                currentTime = 0;
                do
                {
                    meshRenderer.material.color = Color.Lerp(toColor, fromColor, currentTime / duration);
                    currentTime += Time.deltaTime;
                    yield return null;
                } while (currentTime <= duration);
            }

            meshRenderer.material.color = fromColor;

            yield return null;
        }
    }
}