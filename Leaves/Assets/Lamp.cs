using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class Lamp : Interactable
    {
        [SerializeField] private ParticleSystem _lightEffect;

        private void Start()
        {
            SetCanInteract(false);
        }

        private void Update()
        {
            if(CanInteract && (transform.position - PlayerController.Instance.transform.position).sqrMagnitude < 2.5f)
            {
                Highlight();
            }
            else
            {
                Downlight();
            }
        }

        public override void SetCanInteract(bool canInteract)
        {
            //base.SetCanInteract(canInteract);
            CanInteract = canInteract;

            if (CanInteract)
                _lightEffect.Play();
            else
                _lightEffect.Stop();
        }

        public override bool Highlight()
        {
            PlayerController.AddLamp(this);
            return false;
        }

        public override void Downlight()
        {
            PlayerController.RemoveLamp(this);
        }
    }
}