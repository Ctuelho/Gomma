using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace Gomma
{
    public class FadeManager : MonoBehaviour
    {
        public delegate void OnFadeOutComplete();
        public OnFadeOutComplete FadeOutComplete;

        public static FadeManager Instance { get; private set; } = null;
        public static bool Busy { get; private set; }

        [SerializeField] private Image _curtain;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }

        public void FadeOut(float duration = 0.25f)
        {
            if (Busy)
                return;

            Busy = true;
            StartCoroutine(_FadeOut(duration));
        }

        public void FadeIn(float duration = 0.25f)
        {
            if (Busy)
                return;

            Busy = true;
            StartCoroutine(_FadeIn(duration));
        }

        IEnumerator _FadeOut(float duration)
        {
            _curtain.enabled = true;
            _curtain.color = new Color(0, 0, 0, 0);

            while (duration >= 0)
            {
                duration -= Time.deltaTime;
                _curtain.color = new Color(0, 0, 0, _curtain.color.a + (Time.deltaTime / duration));
                yield return new WaitForEndOfFrame();
            }

            _curtain.color = new Color(0, 0, 0, 1);
            Busy = false;
            FadeOutComplete?.Invoke();
        }

        IEnumerator _FadeIn(float duration)
        {
            _curtain.enabled = true;
            Busy = true;
            _curtain.color = new Color(0, 0, 0, 1);

            while (duration >= 0)
            {
                duration -= Time.deltaTime;
                _curtain.color = new Color(0, 0, 0, _curtain.color.a - (Time.deltaTime / duration));
                yield return new WaitForEndOfFrame();
            }

            _curtain.color = new Color(0, 0, 0, 0);
            _curtain.enabled = false;
            Busy = false;
        }
    }
}