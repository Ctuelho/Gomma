using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private string[] _lore;
    [SerializeField] private Text _loreText;

    [SerializeField] List<GameObject> _objectsToHide;
    [SerializeField] List<GameObject> _objectsToShow;

    private int _currentLore;

    private Coroutine _adavanceDelay;

    private bool _allowControls = false;

    private void Update()
    {
        if (!_allowControls)
            return;

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_adavanceDelay != null)
                StopCoroutine(_adavanceDelay);
            _adavanceDelay = StartCoroutine(AdvanceDelay());

            _currentLore++;
            if (_currentLore >_lore.Length - 1)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                _loreText.text = _lore[_currentLore];
            }
        }
    }

    public void ClickStart()
    {
        if (_adavanceDelay != null)
            StopCoroutine(_adavanceDelay);
        _adavanceDelay = StartCoroutine(AdvanceDelay());

        _objectsToHide.ForEach(o => o.SetActive(false));
        _objectsToShow.ForEach(o => o.SetActive(true));
    }

    IEnumerator AdvanceDelay()
    {
        _allowControls = false;
        yield return new WaitForSeconds(0.5f);
        _allowControls = true;
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
