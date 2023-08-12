using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class TargetArrow : MonoBehaviour
    {
        [SerializeField] private float _hideDistance = 1f;
        [SerializeField] private GameObject _arrowHolder;
        [SerializeField] private Transform _target;
        [SerializeField] private Interactable _interactable;
        private bool _act = true;

        private void OnEnable()
        {
            if(_interactable != null)
            {
                _interactable.StateChanged += OnInteractableStateChanged;
            }
        }

        private void OnDisable()
        {
            if (_interactable != null)
            {
                _interactable.StateChanged -= OnInteractableStateChanged;
            }
        }

        private void OnInteractableStateChanged(bool state)
        {
            _act = !state;
            _arrowHolder.SetActive(!state);
        }

        private void Update()
        {
            if (!_act)
                return;

            if (_target != null)
            {
                var dir = _target.position - transform.position;

                var inDistance = dir.magnitude >= _hideDistance;
                _arrowHolder.SetActive(inDistance && _act);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle + 90.0f, Vector3.forward);
            }
        }
    }
}