using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class GreatGrub : MonoBehaviour
    {
        [SerializeField] [Range(0.01f, 100f)] private float _walkSpeed = 0.1f;
        [SerializeField] private Transform _butt;
        [SerializeField] private Animator _animator;

        private int _state = 0;
        private ItemTypes _lastFruitEaten;
        private Coroutine _eatCoroutine;

        private void Update()
        {
            _animator.SetInteger("State", _state);

            if (_state == 0)
            {
                transform.position = transform.position + (Vector3.right * Time.deltaTime * _walkSpeed);
            }
        }

        IEnumerator Eat(float delay)
        {
            yield return new WaitForSeconds(delay);

            _state = 0;

            //poop
            var prefab = GameManager.ExcrementStatistics.GetExcrementPrefab(_lastFruitEaten);
            var excrement = Instantiate(prefab);
            excrement.transform.position = _butt.position;
            GameManager.AddFarmedEnergy(GameManager.ExcrementStatistics.GetEnergyByTypeOfExcrement(_lastFruitEaten));
        }

        //collision part
        private void ResolveContact(GameObject contact)
        {
            if (_state == 1)
                return;

            var interactable = contact.GetComponent<Interactable>();

            if (!interactable.CanInteract)
            {
                return;
            }

            if(interactable is GrubKiller)
            {
                EndMenu.Instance.Show("The great grub is too far now. Keep an eye for the next one!", false);
                return;
            }
            if (interactable is Fruit)
            {
                _state = 1;
                _lastFruitEaten = interactable.ItemType;
                var delay = GameManager.ExcrementStatistics.GetFruitEatingTime(_lastFruitEaten);

                if (_eatCoroutine != null)
                    StopCoroutine(_eatCoroutine);

                _eatCoroutine = StartCoroutine(Eat(delay));

                interactable.SetCanInteract(false);
                Destroy(interactable.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ResolveContact(collider.gameObject);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            ResolveContact(collider.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var interactable = collider.gameObject.GetComponent<Interactable>();

            if (interactable == null)
                return;

            if (PlayerController.HighlightedItem != null && PlayerController.HighlightedItem == interactable)
                PlayerController.DownlightItem();
        }
    }
}