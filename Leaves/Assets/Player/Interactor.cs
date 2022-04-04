using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Gomma
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _actionText;

        private void ResolveContact(GameObject contact)
        {
            var interactable = contact.GetComponent<Interactable>();

            if (interactable == null)
                return;

            if (interactable.CanInteract)
            {
                PlayerController.TryHighlighItem(interactable);
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ResolveContact(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            ResolveContact(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var interactable = collider.gameObject.GetComponent<Interactable>();

            if (interactable == null)
                return;

            if (PlayerController.HighlightedItem != null && PlayerController.HighlightedItem == interactable)
                PlayerController.DownlightItem();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var interactable = collision.gameObject.GetComponent<Interactable>();

            if (interactable == null)
                return;

            if (PlayerController.HighlightedItem != null && PlayerController.HighlightedItem == interactable)
                PlayerController.DownlightItem();
        }

        public void SetAction(string action)
        {
            _actionText.gameObject.SetActive(true);
            _actionText.text = action;
        }

        public void ClearAction()
        {
            _actionText.gameObject.SetActive(false);
        }
    }
}