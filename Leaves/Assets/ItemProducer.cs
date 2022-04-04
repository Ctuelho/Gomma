using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class ItemProducer : Interactable
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private ParticleSystem _visualEffect;

        private void Start()
        {
            SetCanInteract(false);
        }

        public override void SetCanInteract(bool canInteract)
        {
            base.SetCanInteract(canInteract);

            if (CanInteract)
                _visualEffect.Play();
            else
                _visualEffect.Stop();
        }

        public override bool Highlight()
        {
            return PlayerController.CarriedItem == null;
        }

        public override bool Interact()
        {
            if (base.Interact())
            {
                var gelObj = Instantiate(_itemPrefab);
                var interactable = gelObj.gameObject.GetComponent<Interactable>();
                PlayerController.DownlightItem();
                PlayerController.SetHighlightedItem(interactable);
                PlayerController.Interact();
                return true;
            }

            return false;
        }
    }
}