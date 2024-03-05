using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDKFramework.Utils
{
    public class HabbyText
    {
        private Text _text;
        private Action _callback;
        private float _time;
        private Coroutine _animationCoroutine;

        public void Show(string value, Action onComplete, float time)
        {
            Clear();

            _time = time;

            _text = GameObject.Instantiate(HabbyFramework.UI.textTip,
                HabbyFramework.UI.textTip.transform.parent);

            _text.text = value;
            _callback = onComplete;

            var startPosition = _text.rectTransform.position;
            var targetPosition = startPosition + new Vector3(0, 100, 0);

            var startColor = _text.material.color;
            var endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

            _animationCoroutine =
                CoroutineScheduler.Instance.StartCoroutine(AnimateTextTip(startPosition, targetPosition, startColor,
                    endColor));

            _text.gameObject.SetActive(true);
        }

        IEnumerator AnimateTextTip(Vector3 startPosition, Vector3 targetPosition, Color startColor, Color endColor)
        {
            float timer = 0f;

            CanvasGroup group = _text.GetComponent<CanvasGroup>();

            while (timer < _time)
            {
                timer += Time.deltaTime;

                float progress = timer / _time;

                _text.rectTransform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                if (timer >= (_time - 0.2f))
                    group.alpha = Mathf.Lerp(1f, 0f, (timer - (_time - 0.2f)) / 0.2f);

                yield return null;
            }

            OnAnimationDone();
        }

        private void OnAnimationDone()
        {
            if (_text != null)
            {
                _text.gameObject.SetActive(false);
                GameObject.Destroy(_text.gameObject);
            }

            _callback?.Invoke();
            Clear();
        }

        public void Clear()
        {
            if (_animationCoroutine != null)
            {
                CoroutineScheduler.Instance.StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            if (_text != null)
            {
                GameObject.Destroy(_text.gameObject);
                _text = null;
            }

            _callback = null;
        }
    }

    public class HabbyTextHelper : Singleton<HabbyTextHelper>
    {
        private List<HabbyText> _tips = new List<HabbyText>();

        public void ShowTip(string value, float time = 1f)
        {
            if (string.IsNullOrEmpty(value) || time <= 0)
            {
                return;
            }

            HabbyText textTip = new HabbyText();
            _tips.Add(textTip);
            textTip.Show(value, (() =>
            {
                textTip.Clear();
                _tips.Remove(textTip);
            }), time);
        }

        public void ClearAll()
        {
            foreach (HabbyText textTip in _tips)
            {
                textTip.Clear();
            }

            _tips.Clear();
        }
    }
}