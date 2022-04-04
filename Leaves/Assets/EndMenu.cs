using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Gomma {
    public class EndMenu : MonoBehaviour
    {
        public static EndMenu Instance;

        [SerializeField] private Text _loreText;
        [SerializeField] private Text _profitText;
        [SerializeField] private Text _bestProfitText;

        [SerializeField] GameObject _panel;
        [SerializeField] GameObject _resumeBtn;
        [SerializeField] GameObject _playAgainBtn;

        private Coroutine _adavanceDelay;

        private bool _allowControls = false;

        private void Awake()
        {
            Instance = this;  
        }

        public void Show(string message, bool resumeBtnOn = true)
        {
            _loreText.text = message;

            if (PlayerPrefs.HasKey("BestScore"))
            {
                _bestProfitText.gameObject.SetActive(true);
                var bs = PlayerPrefs.GetInt("BestScore");
                _bestProfitText.text = "Best profit: " + bs.ToString();
                if (GameManager.Profit > bs)
                {
                    PlayerPrefs.SetInt("BestScore", GameManager.Profit);
                }
            }
            else
            {
                _bestProfitText.gameObject.SetActive(false);
                PlayerPrefs.SetInt("BestScore", GameManager.Profit);
            }

            _profitText.text = "Your profit: " + GameManager.Profit.ToString();

            _resumeBtn.SetActive(resumeBtnOn);
            _playAgainBtn.SetActive(!resumeBtnOn);

            ResetDelay();
            DisplayMessage(message);
            Time.timeScale = 0;
        }

        public void DisplayMessage(string message)
        {
            _panel.SetActive(true);
            _loreText.text = message;
        }

        public void ClickPause()
        {
            Show("Paused");
        }

        private void ResetDelay()
        {
            if (_adavanceDelay != null)
                StopCoroutine(_adavanceDelay);
            _adavanceDelay = StartCoroutine(AdvanceDelay());
        }

        IEnumerator AdvanceDelay()
        {
            _allowControls = false;
            yield return new WaitForSecondsRealtime(0.5f);
            _allowControls = true;
        }

        public void ClickResume()
        {
            if (!_allowControls)
                return;

            _panel.SetActive(false);
            Time.timeScale = 1;
        }

        public void ClickPlayAgain()
        {
            if (!_allowControls)
                return;

            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }

        public void ClickQuit()
        {
            if (!_allowControls)
                return;

            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}