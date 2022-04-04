using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class NegativeGravityElevator : Interactable
    {
        [SerializeField] private ParticleSystem _visualEffect;
        [SerializeField] [Range(1f, 100f)] private float _negativeGravityForce = 2;

        private void Start()
        {
            SetCanInteract(false);
        }

        public override bool Highlight()
        {
            if (CanInteract)
            {
                PlayerController.SetNegativeGravity(_negativeGravityForce);
            }

            return CanInteract;
        }

        public override void Downlight()
        {
            PlayerController.SetNegativeGravity(0);
        }

        public override void SetCanInteract(bool canInteract)
        {
            CanInteract = canInteract;
            if (CanInteract)
            {
                _visualEffect.Play();
            }
            else
            {
                _visualEffect.Stop();
            }
        }
    }
}