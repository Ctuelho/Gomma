using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gomma
{
    public class GrubBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _grubBar;
        [SerializeField] private RectTransform _grubIcon;
        [SerializeField] private Transform _grubHead;
        [SerializeField] private Transform _exit;

        private float _maxDistance;

        private void Awake()
        {
            _maxDistance = _exit.position.x - _grubHead.position.x;
        }

        private void Update()
        {
            var dist = _exit.position.x - _grubHead.position.x;
            var relativeDist = Mathf.Clamp(dist / _maxDistance, 0, 1);

            var pos = _grubBar.rect.width * (1 - relativeDist);
            _grubIcon.anchoredPosition = new Vector2(pos, 0);

            var viewPortPos = Camera.main.WorldToViewportPoint(_grubHead.position);
            var hudOn = viewPortPos.x > 0 && viewPortPos.x < 1 && viewPortPos.y > 0 && viewPortPos.y < 1;
            _grubBar.gameObject.SetActive(!hudOn);
            _grubIcon.gameObject.SetActive(!hudOn);
        }
    }
}