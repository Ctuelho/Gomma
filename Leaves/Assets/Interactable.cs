using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public enum ItemTypes { NONE = -1, GEL = 0, EXCREMENT_1 = 1, EXCREMENT_2 = 2, EXCREMENT_3 = 3, FRUIT_SMALL = 4, FRUIT_AVERAGE = 5, FRUIT_LARGE = 6 }

    public class Interactable : MonoBehaviour
    {
        public delegate void OnStateChangedEvent(bool state);
        public OnStateChangedEvent StateChanged;

        public ItemTypes ItemType = ItemTypes.NONE;
        public List<ItemTypes> MustBeCarryingToInteract;
        public bool ConsumesCarriedItem = true;
        public string Action = "Use";
        public Rigidbody2D Rigidbody;
        public List<Collider2D> Colliders;

        public bool CanInteract { get; protected set; } =  true;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public virtual void OnStateChanged(bool state)
        {
            StateChanged?.Invoke(state);
        }

        public virtual void SetCanInteract(bool canInteract)
        {
            CanInteract = canInteract;
            if(Colliders != null)
            {
                Colliders.ForEach(c => c.enabled = canInteract);
            }
            Rigidbody.isKinematic = !canInteract;
        }

        public virtual bool Highlight()
        {
            return true;
        }

        public virtual void Downlight()
        {

        }

        public virtual bool Interact()
        {
            if (MustBeCarryingToInteract == null || MustBeCarryingToInteract.Count == 0)
                return true;

            if (PlayerController.CarriedItem != null && MustBeCarryingToInteract.Contains(PlayerController.CarriedItem.ItemType))
                return true;

            return false;
    }

        public virtual bool Carry()
        {
            if (PlayerController.CarriedItem != null)
                return false;

            SetCanInteract(false);
            Rigidbody.velocity = Vector3.zero;
            return true;
        }

        public virtual void Drop()
        {
            SetCanInteract(true);
            transform.localScale = _originalScale;
        }

        public virtual void Consume()
        {

        }
    }
}