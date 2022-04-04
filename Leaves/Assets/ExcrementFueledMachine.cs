using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class ExcrementFueledMachine : Interactable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _visualEffect;
        [SerializeField] private Interactable _targetInteractable;
        [SerializeField] private Transform _excrementPlace;

        private GameObject _excrement;

        private bool _isOn;
        private int _energy;

        private float _counter;

        private void Start()
        {
            _animator.SetInteger("State", 0);
        }

        private void Update()
        {
            
            if(_isOn)
            {
                _counter += Time.deltaTime;
                while (_counter > 1)
                {
                    _counter -= 1;
                    _energy = Mathf.Max(_energy - 1, 0);
                    GameManager.AddUsedEnergy(1);
                }

                if (_energy == 0)
                {
                    _isOn = false;
                    _targetInteractable.SetCanInteract(false);
                    SetCanInteract(true);
                    _animator.SetInteger("State", 0);
                    _visualEffect.Stop();
                    Destroy(_excrement);
                }
            }
        }

        public override void SetCanInteract(bool canInteract)
        {
            CanInteract = canInteract;
        }

        public override bool Highlight()
        {
            return base.Interact();
        }

        public override bool Interact()
        {
            if (base.Interact())
            {
                _targetInteractable.SetCanInteract(true);
                SetCanInteract(false);
                _isOn = true;
                _animator.SetInteger("State", 1);
                _visualEffect.Play();
                var excrement = PlayerController.CarriedItem;
                switch (excrement.ItemType)
                {
                    case ItemTypes.EXCREMENT_1:
                        _energy = GameManager.ExcrementStatistics.SmallEnergy;
                        break;
                    case ItemTypes.EXCREMENT_2:
                        _energy = GameManager.ExcrementStatistics.AverageEnergy;
                        break;
                    case ItemTypes.EXCREMENT_3:
                        _energy = GameManager.ExcrementStatistics.LargeEnergy;
                        break;
                    default:
                        _energy = GameManager.ExcrementStatistics.SmallEnergy;
                        break;
                }
                excrement.SetCanInteract(false);
                _excrement = excrement.gameObject;
                _excrement.transform.SetParent(_excrementPlace);
                _excrement.transform.localPosition = Vector3.zero;
                Destroy(excrement);
                return true;
            }

            return false;
        }
    }
}
